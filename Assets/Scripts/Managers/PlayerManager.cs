using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour{
	[SerializeField] private Transform[] spawnLocations;
	[SerializeField] private Material[] materials;
	
	private GameManager _gameManager;

	private static GameObject _singleTon;
	
	private void Start(){
		if (_singleTon == null){_singleTon = gameObject;}
		else{Die();}

		_gameManager = GameManager.Instance;
		
		PlayerSpawnEvent.RegisterListener(OnPlayerSpawn);
		PlayerDeathEvent.RegisterListener(OnPlayerDeath);
	}

	/*
	 * Called upon player joining. Might be displayed as never used.
	 */
	void OnPlayerJoined(PlayerInput playerInput){
		if (_gameManager.GetGameState() != GameState.Menu){
			Debug.Log("menu");
			return;
		}
		PlayerSpawnEvent pse = new PlayerSpawnEvent{
			playerIndex = playerInput.playerIndex,
			playerGo = playerInput.gameObject,
		};
		
		pse.FireEvent();
	}
    
	private void OnPlayerSpawn(PlayerSpawnEvent pse){
		Debug.Log("Spawned player: " + pse.playerIndex + " at " + spawnLocations[pse.playerIndex].position + ".");
		
		GameObject playerGo = pse.playerGo;
		playerGo.gameObject.SetActive(true);
		playerGo.transform.position = spawnLocations[pse.playerIndex].position;
		playerGo.GetComponentInChildren<SkinnedMeshRenderer>().material = materials[pse.playerIndex];
		playerGo.GetComponent<Dash>().ResetValues();
		playerGo.GetComponent<PlayerHealth>().UnkillPlayer();
	}
    
	private void OnPlayerDeath(PlayerDeathEvent pde)
	{
		Debug.Log("Player " + pde.kille + " was killed by " + pde.killer + " using " + pde.killedWith + ".");
		
		pde.kille.gameObject.GetComponent<PlayerHealth>().KillPlayer();
		pde.kille.gameObject.GetComponent<PlayerDetails>().isAlive = false;
		pde.kille.gameObject.GetComponent<PlayerMovement>().StopPlayer(); 
		pde.kille.SetActive(false);
	}
    
	private void OnDestroy(){
		PlayerSpawnEvent.UnregisterListener(OnPlayerSpawn);
		PlayerDeathEvent.UnregisterListener(OnPlayerDeath);
	}

	private void Die()
	{
		Destroy(gameObject);
	}
}