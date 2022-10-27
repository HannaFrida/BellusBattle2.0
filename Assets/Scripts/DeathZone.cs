using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DeathZone : MonoBehaviour
{
    [SerializeField] CameraFocus CF;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            // S�tter isAlive bool till false
            other.gameObject.GetComponent<PlayerHealth>().KillPlayer();
            other.gameObject.GetComponent<PlayerDetails>().isAlive = false;
            other.gameObject.GetComponent<PlayerMovement>().StopPlayer();
            CF.RemoveTarget(other.transform);
            //other.gameObject.GetComponent<PlayerInputManager>().gameObject.SetActive(false);
            //other.gameObject.SetActive(false);
        }
    }
}
