using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	[SerializeField, Tooltip("For how long the game shall wait until the round starts.")] 
	private float roundStartingPauseTime = 10.0f;
	[SerializeField, Tooltip("For how long the game shall wait until giving the remaining players their score.")] 
	private float roundEndingPauseTime = 10.0f;
	[SerializeField, Tooltip("Reference to the level manager for telling it when to swap level.")]
	private GameObject levelManagerGo;

	public static GameManager Instance;

	private static GameObject _singleTon;

	private GameState _state;
	private readonly Dictionary<GameObject, int> _players = new();
	private int _scoreToWin;
	private LevelManager levelManager;

	private void Awake()
	{
		if (_singleTon == null){_singleTon = gameObject;}
		else{Die();}
		Instance = this;
	}

	private void Start(){
		PlayerSpawnEvent.RegisterListener(AddPlayer);
		levelManager = levelManagerGo.GetComponent<LevelManager>();
		UpdateGameState(GameState.Menu);
	}

	private void Update()
	{
		if (_state == GameState.RoundOnGoing && _players.Count <= 1){
			UpdateGameState(GameState.RoundEnding);
		}
	}

	public void UpdateGameState(GameState newState)
	{
		_state = newState;
		switch (newState)
		{
			case GameState.Menu:
				HandleMenu();
				break;
			case GameState.RoundStarting:
				HandleRoundStarting();
				break;
			case GameState.RoundOnGoing:
				HandleRoundOnGoing();
				break;
			case GameState.RoundEnding:
				HandleRoundEnding();
				break;
			case GameState.GameEnding:
				HandleGameEnding();
				break;
			case GameState.NewLevel:
				HandleNewLevel();
				break;
			default:
				throw new Exception("Unexpected GameState state.");
		}
	}

	private void AddPlayer(PlayerSpawnEvent pse)
	{
		if (_players.ContainsKey(pse.playerGo)){return;}
		_players.Add(pse.playerGo, 0);
	}

	private void RemovePlayer(PlayerDeathEvent pde)
	{
		if (_players.ContainsKey(pde.kille) == false){return;}
		_players.Remove(pde.kille);
	}

	public int PlayerCount(){
		return _players.Count;
	}

	public void SetScoreToWin(int score){
		_scoreToWin = score;
	}

	public GameState GetGameState(){
		return _state;
	}

	/*
	 * Called while in the menu
	 */
	private void HandleMenu(){
		
	}
	
	/*
	 * Called when the round is starting.
	 */
	private void HandleRoundStarting()
	{
		StartCoroutine(RoundStartingPause(roundStartingPauseTime));
	}
	
	/*
	 * Called after the round has started.
	 */
	private void HandleRoundOnGoing()
	{
		//Handled in the void Update() method.
	}
	
	/*
	 * The waiting period before swapping level.
	 */
	private void HandleRoundEnding(){
		StartCoroutine(RoundEndingPause(roundEndingPauseTime));
	}
	
	/*
	 * Called once a victor as been declared.
	 */
	private void HandleGameEnding(){
		//Load in the end level.
	}
	
	private void HandleNewLevel()
	{
		levelManager.LoadNextScene();
	}

	private IEnumerator RoundStartingPause(float seconds){
		Debug.Log("Round start.");
		yield return new WaitForSeconds(seconds);
		Debug.Log("Fight!");
		
		UpdateGameState(GameState.RoundOnGoing);
	}

	private IEnumerator RoundEndingPause(float seconds){
		Debug.Log("Round has ended, waiting to see if the remainder will die.");
		yield return new WaitForSeconds(seconds);
		Debug.Log("Giving score to the remainder.");
		
		//In a loop in-case we later decide to include teams.
		foreach(var key in _players.Keys){
			//Adds the score to the player and then checks if they have enough to win.
			if (key.GetComponent<PlayerDetails>().isAlive)
			{
				_players[key]++;
				if (CheckForVictor(key))
				{
					PlayerWonEvent pwe = new PlayerWonEvent
					{
						player = key,
						score = _players[key],
					};
					pwe.FireEvent();
					UpdateGameState(GameState.GameEnding);
				}
			}
		}
	}

	private bool CheckForVictor(GameObject killer){
		if (_players[killer] >= _scoreToWin)
		{
			return true;
		}
		return false;
	}

	private void OnDestroy(){
		//Unregister from the event.
		PlayerSpawnEvent.UnregisterListener(AddPlayer);
	}

	private void Die()
	{
		Destroy(gameObject);
	}
}

public enum GameState
{
	Menu, //Players are in the menu.
	RoundStarting, //Round is starting.
	RoundOnGoing, //Round is on going.
	RoundEnding, //Round has ended.
	GameEnding, //Game has ended.
	NewLevel, //Changes the level.
}