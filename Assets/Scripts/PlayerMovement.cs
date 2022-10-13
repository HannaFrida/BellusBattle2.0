using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Forces")]
    [SerializeField] [Tooltip("Variabel som p�verkar spelarens hastighet")] [Range(1f, 25f)] private float moveSpeed;
    [SerializeField] [Tooltip("Hur h�gt spelaren kan hoppa")] [Range(1f, 25f)] private float jumpForce;
    [SerializeField] [Tooltip("Hur snabbt spelaren decelererar i luften")] [Range(25f, 150f)] private float airDeceleration;
    [SerializeField] [Tooltip("Hursnabbt spelaren decelererar p� marken")] [Range(25f, 150f)] private float groundDeceleration;
    [SerializeField] [Tooltip("P�verkar hur snabbt spelaren f�rlorar energi i ett hopp")] [Range(10f, 300f)] private float airResistance;
    [SerializeField] [Tooltip("Hur mycket gravitation som p�verkar spelaren i luften")] [Range(-100f, 0f)] private float downwardForce;

    [Header("Layers")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;

    public Vector2 velocity;
    private float movementX, movementY;
    Vector2 movementX2;
    bool moving;

    private float deceleration;
    private float coyoteTime = 0.2f;
    private float coyoteTimer;

    private bool hasCoyoteTime;
    private bool hasDoubleJump;

    private float skinWidth = 0.012f;

    private BoxCollider boxCollider;
    [SerializeField] private Vector3 rayCastBottomLeft, rayCastBottomRight, rayCastTopRight, rayCastTopLeft;

    private float verticalRaySpacing, horizontalRaySpacing;
    private int horizontalRayCount, verticalRayCount = 4;

    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        CalculateRaySpacing();
    }

    // Update is called once per frame
    void Update()
    {
        updateRayCastOrgins();
        //UpdateMovementForce();
        CheckIsGrounded();
        //Jump();
        UpdateCoyoteTime();
        if (!CheckIsGrounded())
        {
            movementY = Mathf.MoveTowards(movementY, downwardForce, airResistance * Time.deltaTime);
        }

        if (CheckIsGrounded() && velocity.y < 0)
        {
            movementY = 0;
            coyoteTimer = 0;
            hasCoyoteTime = true;
            hasDoubleJump = true;
        }
        if (!moving)
        {
            velocity.x = Mathf.MoveTowards(movementX2.x, 0, deceleration * Time.deltaTime);
           // Debug.Log(moving);
        }
        velocity = new Vector2(movementX2.x, movementY);


        //velocity = new Vector2(movementX, movementY);
        HandleVerticalCollisions(ref velocity);
        HandleHorizontalCollisions(ref velocity);


        if (!CheckCollision())
        {
            transform.Translate(velocity * Time.deltaTime);
        }

        //FLipPlayer();

    }

    private void FLipPlayer()
    {
        if (velocity.x < -.01)
        {
            transform.eulerAngles = new Vector3(0, 180, 0); // Flipped
        }
        else if (velocity.x > .01)
        {
            transform.eulerAngles = new Vector3(0, 0, 0); // Normal
        }
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        movementX2 = ctx.ReadValue<Vector2>();// * moveSpeed;
        //Debug.Log(movementX2);
        if (movementX2.x > 0f)
        {
            movementX2.x = 1f;
            moving = true;
        }
        if (movementX2.x < 0f)
        {
            movementX2.x = -1f;
            moving = true;
        }
        if (movementX2.x == 0)
        {
            moving = false;
        }
        //Debug.Log(movementX2);
        if (movementX2.x != 0)
        {
            movementX2.x *= moveSpeed;


        }


        //moving = true;
        // Debug.Log(moving);

        //Debug.Log(movementX2.x);
        //moving = ctx.started;


        if (ctx.started)
        {

            //moving = false;
            //Debug.Log("fun");
        }
    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (CheckIsGrounded() || hasCoyoteTime || hasDoubleJump)
        {
            if (ctx.started)
            {
                if (!hasCoyoteTime && hasDoubleJump) { hasDoubleJump = false; }
                movementY = jumpForce;
            }
        }
    }


    private void UpdateMovementForce()
    {
        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            movementX = moveSpeed * Input.GetAxisRaw("Horizontal");
        }
        else
        {
            movementX = Mathf.MoveTowards(movementX, 0, deceleration * Time.deltaTime);
        }


    }
    private bool CheckIsGrounded()
    {
        if (Physics.Raycast(transform.position, Vector2.down, 0.5f + skinWidth, groundLayer))
        {
            deceleration = groundDeceleration;
            return true;
        }
        deceleration = airDeceleration;
        return false;

    }
    private void Jump()
    {
        if (CheckIsGrounded() || hasCoyoteTime || hasDoubleJump)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (!hasCoyoteTime && hasDoubleJump) { hasDoubleJump = false; }
                movementY = jumpForce;
            }
        }

    }

    private void UpdateCoyoteTime()
    {
        if (CheckIsGrounded() || !hasCoyoteTime) return;
        //Debug.Log("awooo");
        if (coyoteTimer > coyoteTime)
        {
            hasCoyoteTime = false;
        }
        coyoteTimer += Time.deltaTime;
    }

    private bool CheckCollision()
    {
        Debug.DrawRay(transform.position, velocity, Color.green, 0.5f + skinWidth);
        return Physics.Raycast(transform.position, velocity, 0.5f + skinWidth, wallLayer);

    }

    
    private void HandleVerticalCollisions(ref Vector2 velocity)
    {
        float directionY = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Abs(velocity.y) + skinWidth;

        for (int i = 0; i < verticalRayCount; i++)
        {
            //Debug.Log("adada");

            Vector2 rayOrigin = (directionY == -1) ? rayCastBottomLeft : rayCastTopLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i);



            Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red);

            RaycastHit hit;
            if (Physics.Raycast(rayOrigin, Vector2.up * directionY, out hit, 0.5f + skinWidth, wallLayer))//rayOrigin, Vector2.up * directionY, out hit, rayLength, wallLayer))
            {
               // Debug.Log("Hit vert");
                Debug.Log(hit.transform.position.y);
                velocity.y = 0;
                /*
                velocity.y = (hit.distance - skinWidth) * directionY;
                rayLength = hit.distance;
                */

                //movementY = 0;
            }
        }
    }

    private void HandleHorizontalCollisions(ref Vector2 velocity)
    {
        float directionX = Mathf.Sign(velocity.x);
        float rayLength = Mathf.Abs(velocity.x) + skinWidth;

        for (int i = 0; i < horizontalRayCount; i++)
        {
            //Debug.Log("adada");

            Vector2 rayOrigin = (directionX == -1) ? rayCastBottomLeft : rayCastBottomRight;
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);



            Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red);

            RaycastHit hit;
            if (Physics.Raycast(rayOrigin, Vector2.right * directionX, out hit, 0.6f + skinWidth, wallLayer))//rayOrigin, Vector2.right * directionX, out hit, rayLength, wallLayer))
            {
                //Debug.Log("Hit horiz");
                velocity.x = 0;
                /*
                Debug.Log("Hit!");
                velocity.x = (hit.distance - skinWidth);
                rayLength = hit.distance;
                */
                //movementY = 0;
            }
        }
    }
    void CalculateRaySpacing()
    {
        Bounds bounds = boxCollider.bounds;
        bounds.Expand(skinWidth * -2);

        horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
        verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

        horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
        verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
    }


    private void updateRayCastOrgins()
    {
        Bounds bounds = boxCollider.bounds;
        bounds.Expand(skinWidth * -2);

        rayCastBottomLeft = new Vector2(bounds.center.x, bounds.center.y);
        rayCastTopLeft = new Vector2(bounds.center.x, bounds.center.y);
        rayCastBottomRight = new Vector2(bounds.center.x, bounds.center.y);
        rayCastTopRight = new Vector2(bounds.center.x, bounds.center.y);



        /*
        rayCastBottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        rayCastTopLeft = new Vector2(bounds.min.x, bounds.max.y);
        rayCastBottomRight = new Vector2(bounds.max.x, bounds.min.y);
        rayCastTopRight = new Vector2(bounds.max.x, bounds.max.y);
        */

    }

    public float GetDownwardForce()
    {
        return downwardForce;
    }
    public void SetDownwardForce(float value)
    {
        downwardForce = value;


    }


}
