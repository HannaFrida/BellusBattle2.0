using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DeathZone : MonoBehaviour
{
    [SerializeField] private string causeOfDeath = "Death zone";
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerHealth>().TakeDamage(1);
            //other.gameObject.GetComponent<PlayerDetails>().isAlive = false;
            //other.gameObject.GetComponent<PlayerMovement>().StopPlayer();
            PlayerDeathEvent pde = new PlayerDeathEvent
            {
                kille = other.gameObject,
                killer = gameObject,
                killedWith = causeOfDeath,
            };
            pde.FireEvent();
        }
    }
}
