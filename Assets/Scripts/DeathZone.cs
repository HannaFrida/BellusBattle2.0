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
            Debug.Log("FUCK");
            other.gameObject.GetComponent<PlayerDetails>().isAlive = false;
            CF.RemoveTarget(other.transform);
            other.gameObject.SetActive(false);
        }
    }
}
