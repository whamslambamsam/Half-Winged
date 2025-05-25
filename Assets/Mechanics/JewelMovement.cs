using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// Baby's first coding - excuse how simple and janky it may be
public class JewelMovement : MonoBehaviour
{
        // Values can be edited in Unity's Editor
    // -------------------------------------------------------------------------
    //
    [SerializeField] private float speed;
        // The character's movement speed
    [SerializeField] private float jump;
        // The character's jumping force
    [SerializeField] private int airActions;
        // The number of jumps/dashes the character is allowed to do
    [SerializeField] private float fallMultiplier;
        // How fast the character falls
    [SerializeField] private float dashDuration;
        // How long the dash goes on for
    [SerializeField] private int dashSpeed;
        // How fast the character travels during the dash
    [SerializeField] private Transform groundCheck;
        // Checks if the character is grounded
    [SerializeField] private LayerMask groundLayer;
    // Layer that allows groundCheck to do its, y'know, 'groundCheck'ing
    //
    // -------------------------------------------------------------------------
    // ~

    //

        // The junk stash
    // .........................................................................
    //
    private float horizontal;
    private float vertical;
    private int airActionsLeft;
    private float dashTimer;
    //
    // .........................................................................
    //
    Rigidbody2D body;
    //
    // .........................................................................
    private bool isJumping;
    private bool isDashing;
    //
    // .........................................................................
    //
    private Vector2 movementDirection;
    //
    // .........................................................................
    //
    Vector2 vecGravity;
    Vector2 dashDir;
    //
    // .........................................................................
    // ~


    // On startup:
    private void Awake()
    {
        // Resets booleans
        isJumping = false;
        isDashing = false;

        // Resets counters
        dashTimer = dashDuration;
        airActionsLeft = airActions;

        // "Fetches" (not as in git, mind you) the RigidBody2D 
        body = GetComponent<Rigidbody2D>();
        // "Fetches" Unity's (Physics2D) gravity
        vecGravity = new Vector2(0f, -Physics2D.gravity.y);
    }

        // On every frame:
    private void Update()
    {
        // No idea honestly, just horizontal - what else could you ask for?
        // ------------------------------------------------------------------
        // It's here just to have wall climbing's code work since i'm pretty 
        // much trying to learn through YouTube videos and I have no idea how 
        // to do it myself - I'm trying over here!

        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        movementDirection = new Vector2(horizontal, vertical);
        dashDir = movementDirection;

            // --- Walking
        // -------------------
        if (!isDashing)
        {
            // -- old code
            // body.linearVelocity = new Vector2(horizontal * speed, vertical);
            Vector2 vel = body.linearVelocity;
            vel.x = horizontal * speed;
            body.linearVelocity = vel;
        }

            // --- Jump
        // -------------------
        if (Input.GetKeyDown(KeyCode.J) && (airActionsLeft > 0f) && !isJumping)
        {
            isJumping = true;
            Debug.Log("Le jump");

            // -- old code
            // body.linearVelocity = new Vector2(horizontal, jump * speed);
            // -------------------
            // body.linearVelocity = new Vector2(body.linearVelocity.x, 0);
            body.AddForce(new Vector2(0f, jump), ForceMode2D.Impulse);

            airActionsLeft--;
            isJumping = false;      
        }

            // --- Resets values when landing
        if (isGrounded() == true)
        {
            isJumping = false;
            isDashing = false;

            body.gravityScale = 1f;

            dashTimer = dashDuration;
            airActionsLeft = airActions;
        }
    }

        // On every frame - but fancy and with physics!
    void FixedUpdate()
    {
            // --- Fall down after jump (if there's no jumps left)
        // -------------------
        if (body.linearVelocity.y < 0f)
        {
            if (airActionsLeft <= 0)
            {
                body.linearVelocity -= vecGravity * fallMultiplier * Time.fixedDeltaTime;
                //body.linearVelocity += Vector2.up * Physics2D.gravity.y
                //* (fallMultiplier - 1) * Time.fixedDeltaTime;
            }
        }

            // --- Air dash
        // -------------------
        if (Input.GetKeyDown(KeyCode.Space) && !isDashing)
        {
            if (airActionsLeft > 0 || isJumping)
            {
                isDashing = true;
                Debug.Log("WEe");

                body.gravityScale = 0f;
                dashTimer = dashDuration;

                body.linearVelocity = Vector2.zero;
                body.AddForce(dashDir.normalized * dashSpeed);
                airActionsLeft--;
            }
        }

            // --- Dash continuation and dashTimer
        // -------------------
        if (isDashing)
        {
            if (dashTimer > 0f)
            {
                body.linearVelocity = dashDir.normalized * dashSpeed;
                dashTimer -= Time.fixedDeltaTime;
            }
            else
            {
                isDashing = false;
                body.gravityScale = 1f;
                dashTimer = dashDuration;
            }
        }
    }

        // Puts groundCheck to work 
    // -------------------
    private bool isGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }
}
