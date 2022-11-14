using UnityEngine;

public class WeaponSpawnerManager : MonoBehaviour{
	[SerializeField, Tooltip("Minimum amount of weapons always on the level.")]
	private int minimumWeapons = 2;
	[SerializeField, Tooltip("Delay for when weapons should first spawn.")]
	private float spawnDelay = 10.0f;
	[SerializeField, Tooltip("Delay for when weapon should respawn after being picked up.")]
	private float respawnDelay = 10.0f;
	[SerializeField, Tooltip("Available spawners for this level.")]
	private GameObject[] spawners;

	private int _weaponsAvailable;

	private void Start()
	{
		ForceSpawn();
		PickUpEvent.RegisterListener(DecreaseWeaponCount);
		for (int i = 0; i < spawners.Length; i++){
			WeaponSpawner ws = spawners[i].GetComponent<WeaponSpawner>();
			ws.SetSpawnDelay(spawnDelay);
			ws.SetRespawnDelay(respawnDelay);
		}
	}

	private void DecreaseWeaponCount(PickUpEvent pue){
		_weaponsAvailable--;
		ForceSpawn();
	}

	private void ForceSpawn(){
		if (_weaponsAvailable >= minimumWeapons){return;}
		if (spawners.Length == 0){return;}
        
		int i = 0;
		while(i < minimumWeapons - _weaponsAvailable)
		{
			WeaponSpawner spawner = spawners[Random.Range(0, spawners.Length)].GetComponent<WeaponSpawner>();
			if(spawner != null){
				spawner.SpawnWeapon();
			}
			i++;
		}
	}

	private void OnDestroy(){
		PickUpEvent.UnregisterListener(DecreaseWeaponCount);
	}
}