using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    [SerializeField] private float forceAmountY;
    [SerializeField] private float forceAmountX;
    private Vector2 force;
    // Start is called before the first frame update
    void Start()
    {
        force = new Vector2(forceAmountX, forceAmountY);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            other.gameObject.GetComponent<PlayerMovement>().AddExternalForce(force);
        }
        
    }
    
}
