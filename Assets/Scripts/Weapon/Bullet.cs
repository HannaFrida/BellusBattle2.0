using System.Collections;
using UnityEngine;
using Random = System.Random;

public class Bullet : Projectile
{
	[SerializeField] [Tooltip("For how long the bullet will exist for in seconds.")]
	private float lifeSpan = 5.0f;
	[SerializeField, Tooltip("Type of projectile (e.g. bullet or grenade)")]
	private string projectileName;
	[SerializeField, Tooltip("Sound made when bullet hits something")]
	public AudioSource[] hitSounds;

	public float bulletDamage;

	private GameObject shooter;

	private void Start(){
		StartCoroutine(Shoot(lifeSpan));
	}

	private void OnTriggerEnter(Collider other)
	{
		GameObject playerGo = other.gameObject;
		if (playerGo.CompareTag("Player") && Shooter != playerGo)
		{
			playerGo.GetComponent<PlayerHealth>().TakeDamage(bulletDamage);

            if (playerGo.GetComponent<PlayerHealth>().Health <= 0)
            {
				PlayerDeathEvent pde = new PlayerDeathEvent{
					kille = other.gameObject,
                	killer = shooter,
                	killedWith = projectileName,
                };
                pde.FireEvent();
			}
			Die();
		}

		if(other.gameObject.CompareTag("Target"))
        {
			GetComponent<Destroy>().gone();
		}

		if (other.gameObject.CompareTag("Obstacle"))
		{
			Debug.Log("Obstacle");
			Die();
			return;
		}

        if (other.gameObject.CompareTag("Breakable"))
        {
			Destroy(other.gameObject);
			Die();
        }

		if (hitSounds.Length > 0)
		{
			hitSounds[UnityEngine.Random.Range(0, hitSounds.Length)].Play();
		}

	}

	public void SetDamage(float setTo)
    {
		bulletDamage = setTo;
    }

	private IEnumerator Shoot(float seconds){
		yield return new WaitForSeconds(seconds);
		Die();
	}

	private void Die(){
		Destroy(gameObject);
	}
}