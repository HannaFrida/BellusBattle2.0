using UnityEngine;

public class Teleporter : MonoBehaviour
{
	[SerializeField] private GameObject playPanel;
    
	private int _playerCountOnTeleporter;
	private GameManager _gameManager;

	private void Start()
	{
		_gameManager = GameManager.Instance;
		playPanel.SetActive(false);
	}

	private void OnTriggerEnter(Collider other)
	{
		// Adds to player count on teleporter when entering.
		if (other.gameObject.CompareTag("Player")){_playerCountOnTeleporter++;}

		// Changes level once the player count on teleporter equals the total player count.
		if (_gameManager.PlayerCount() >= 2 && _playerCountOnTeleporter == _gameManager.PlayerCount())
		{
			playPanel.SetActive(true);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		// Subtracts to player count on teleporter when exiting.
		if (other.gameObject.CompareTag("Player")){_playerCountOnTeleporter--;}
	}
}
