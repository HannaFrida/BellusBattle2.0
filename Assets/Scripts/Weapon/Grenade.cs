using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Grenade : Projectile
{
	[SerializeField] [Tooltip("Time until the grenade explodes.")] 
	private float fuse = 5.0f;
	[SerializeField] [Tooltip("Size of the explosion.")]
	private float explosionSize = 5.0f;
	[SerializeField]
	private GameObject vfxGo;

	[SerializeField] PickUp_ProtoV1 pickUp_Proto;
	[SerializeField] private GameObject objectToBoom;
	[SerializeField] private GameObject bombMesh;

	private GameObject _owner;

	private IEnumerator StartFuse(){
		yield return new WaitForSeconds(fuse);
		Explode();
	}

	public void OnPickUp(InputAction.CallbackContext ctx)
	{
		if (ctx.started)
		{
			StartCoroutine(StartFuse());
		}
	}

	private void Explode(){
		Instantiate(objectToBoom, transform);
		pickUp_Proto.isHoldingWeapon = false;
		StartCoroutine(VFX());
		KillPlayers();
		Die();
	}

	private IEnumerator VFX(){
		GameObject explosion = Instantiate(vfxGo, transform.position, new Quaternion(0, 0, 0, 0));
		ParticleSystem particles = explosion.GetComponent<ParticleSystem>();
		particles.Play();
		yield return new WaitForSeconds(particles.main.duration);

		Destroy(vfxGo);
	}

	private void KillPlayers(){
		Collider[] hits = Physics.OverlapSphere(transform.position, explosionSize);
		for (int i = 0;  0 < hits.Length; i++){
			if (hits[i].CompareTag("Player")){
				pickUp_Proto.isHoldingWeapon = false;
        		
				PlayerDeathEvent pde = new PlayerDeathEvent{
					kille = hits[i].gameObject,
					killer = _owner,
					killedWith = gameObject.name,
				};
				pde.FireEvent(); 
			}
			if (hits[i].CompareTag("Door"))
			{
				hits[i].GetComponent<Door>().DestroyDoor();
			}
			if (hits[i].CompareTag("Breakable"))
			{
				Destroy(hits[i].gameObject);
			}
		}
	}

	private void Die(){
		bombMesh.SetActive(false);
		Destroy(gameObject, 1.0f);
	}
}