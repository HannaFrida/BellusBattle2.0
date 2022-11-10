using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardMover : MonoBehaviour
{
    [SerializeField] private Transform highestPoint;
    [SerializeField] private float timeBetweenMoving;
    [SerializeField] private float movingSpeed;
    [SerializeField] private float smoothTime;
    private BoxCollider boxCollider;
    private float lowestPoint;
    private float timer;
    private Vector3 moveVector;
    private Vector3 lowestPosition, highestPosition;
    private bool hasReachedHighestPoint;
    private bool runTimer = true;
    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        lowestPoint = boxCollider.bounds.max.y;
        moveVector = new Vector3(0f, movingSpeed, 0f);
        lowestPosition = new Vector3(transform.position.x, lowestPoint - boxCollider.size.y / 2, 0f);
        lowestPosition = new Vector3(transform.position.x, lowestPoint - boxCollider.size.y / 2, 0f);
        highestPosition = new Vector3(transform.position.x, highestPoint.transform.position.y - boxCollider.size.y / 2, 0f);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (runTimer == true)
        {
            timer += Time.deltaTime;
            if(timer >= timeBetweenMoving)
            {
                runTimer = false;
                timer = 0;
            }
        }
        else
        {
            MoveHazard();
        }
    }
    private void MoveHazard()
    {
        if(hasReachedHighestPoint == false)
        {
            transform.position = Vector3.SmoothDamp(transform.position, highestPosition, ref moveVector, smoothTime);
            if (boxCollider.bounds.max.y >= highestPoint.position.y - 0.2f)
            {
                Debug.Log(boxCollider.bounds.max.y);
                hasReachedHighestPoint = true;
            }
        }
        else
        {
            transform.position = Vector3.SmoothDamp(transform.position, lowestPosition, ref moveVector, smoothTime);
            if (boxCollider.bounds.max.y <= lowestPoint + 0.2f)
            {
                hasReachedHighestPoint = false;
                runTimer = true;
            }
        }
    }
}
