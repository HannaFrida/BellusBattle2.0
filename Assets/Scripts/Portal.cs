using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private GameObject teleportDestination;
    [SerializeField] private List<GameObject> teleportedPlayers;
    // Start is called before the first frame update


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("hohoho");
        if (other.gameObject.tag == "Player" && teleportedPlayers.Contains(other.gameObject) == false)
        {
            Debug.Log("hahaha");
            teleportedPlayers.Add(other.gameObject);
            other.gameObject.transform.position = teleportDestination.transform.position;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        teleportedPlayers.Remove(other.gameObject);
    }

    private void OnDestroy()
    {
        teleportedPlayers.Clear();
    }
}
