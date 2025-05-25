using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// Baby's first coding - excuse how simple and janky it may be
public class BluMovement : MonoBehaviour
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
    [SerializeField] private float wallClimbingSpeed;
    // How fast the character climbs walls
    [SerializeField] private Transform groundCheck;
    // Trigger that checks if the character is grounded
    [SerializeField] private LayerMask groundLayer;
    // Layer that allows groundCheck to do its, y'know, 'groundCheck'ing
    [SerializeField] private Transform wallCheck;
    // Trigger that Check if the character is against a wall
    [SerializeField] private LayerMask wallLayer;
    // Wall-flavored groundLayer
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
    //
    // .........................................................................
    //
    Rigidbody2D body;
    //
    // .........................................................................
    private bool isJumping;
    private bool isWallClimbing;
    //
    // .........................................................................
    //
    Vector2 vecGravity;
    //
    // .........................................................................
    // ~


    // On startup:
    private void Awake()
    {
        // Resets booleans
        isJumping = false;
        airActionsLeft = airActions;

        // "Fetches" (not as in git, mind you) the RigidBody2D 
        body = GetComponent<Rigidbody2D>();
        Animator animator = GetComponent<Animator>();
        // "Fetches" Unity's (Physics2D) gravity
        vecGravity = new Vector2(0f, -Physics2D.gravity.y);
    }

    // On every frame:
    private void Update()
    {
            // No idea honestly, just horizontal - what else could you ask for?
        // -------------------
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

            // --- Walking
        // -------------------
        Vector2 vel = body.linearVelocity;
        vel.x = horizontal * speed;
        body.linearVelocity = vel;

            // Triggers wall climbing
        // -------------------
        isWallClimbing = isWalled() && Input.GetKey(KeyCode.K);

            // --- Jump
        // -------------------
        if (Input.GetKeyDown(KeyCode.K) && !isJumping)
        {
            isJumping = true;
            Debug.Log("Le jump but Blu");

            body.AddForce(new Vector2(0f, jump), ForceMode2D.Impulse);

            airActionsLeft--;
        }

            // --- Resets values when landing
        // -------------------
        if (isGrounded() == true)
        {
            isJumping = false;

            body.gravityScale = 1f;

            airActionsLeft = airActions;
        }
    }

    // On every frame - but fancy and with physics!
    void FixedUpdate()
    {
            // --- Fall down after jump
        // -------------------
        if (body.linearVelocity.y < 0f && isJumping || !isWallClimbing)
        {
            body.linearVelocity -= vecGravity * fallMultiplier * Time.fixedDeltaTime;
            //body.linearVelocity += Vector2.up * Physics2D.gravity.y
            //* (fallMultiplier - 1) * Time.fixedDeltaTime;
        }

            // --- Wall climb
        // -------------------
        if (isWallClimbing)
        {
            body.gravityScale = 0f;
            body.linearVelocity = new Vector2(horizontal, vertical * wallClimbingSpeed);
            //body.AddForce(new Vector2(0f, wallClimbingSpeed), ForceMode2D.Force);
        }
    }

        // Puts groundCheck to work 
    // -------------------
    private bool isGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

        // Puts wallCheck to work 
    // -------------------
    private bool isWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 70f, wallLayer);
    }
}
