using System.Collections;
using UnityEngine;

public class WeaponSpawner : MonoBehaviour{
	[SerializeField, Tooltip("Available weapons for this spawner.")]
	private GameObject[] weapons;
    
	private float _spawnDelay;
	private float _respawnDelay;
	private bool _isRunning = true;
	private GameObject _weapon;

	private void Start(){
		StartCoroutine(InitialSpawn());
	}

	public void SetSpawnDelay(float seconds){
		_spawnDelay = seconds;
	}

	public void SetRespawnDelay(float seconds){
		_respawnDelay = seconds;
	}
    
	public bool SpawnWeapon(){
		if (_weapon == null){
			_weapon = weapons[Random.Range(0, weapons.Length)];
			if (_weapon == null){return false;}
			Instantiate(_weapon, transform.position, new Quaternion(0, 0, 0, 0));
			return true;
		}
		return false;
	}

	private IEnumerator InitialSpawn(){
		yield return new WaitForSeconds(_spawnDelay);
		SpawnWeapon();
		StartCoroutine(RespawnWeapon());
	}

	private IEnumerator RespawnWeapon(){
		while (_isRunning){
			yield return new WaitForSeconds(_respawnDelay);
			SpawnWeapon();
		}
	}
}