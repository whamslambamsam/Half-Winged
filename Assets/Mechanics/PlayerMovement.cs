using Unity.VisualScripting;
using UnityEngine;
// Baby's first coding - excuse how simple and janky it may be
public class PlayerMovement : MonoBehaviour
{
    // Values can be edited in Unity's Editor
    [SerializeField]private float speed;
        // The character's movement speed
    [SerializeField]private float gravityScale;
        // The character's gravity, duh - best kept at 1 ("1,0f")
    [SerializeField]private float coyoteTime;
        // Time it takes for the character to fall midair - best kept at 1 too
    [SerializeField]private int maxJumpCount;
        // The number of jumps the character is allowed to do
    [SerializeField]private Transform groundCheck;
        // Checks if the character is grounded
    [SerializeField]private LayerMask groundLayer;
        // Layer that allows groundCheck to do its, y'know, 'groundCheck'ing
    [SerializeField]private Transform wallCheck;
        // Checks if the character is against a wall
    [SerializeField]private LayerMask wallLayer;

    // The junk stash
    private float horizontal;
    private Rigidbody2D body;
    private bool isWallClimbing;
    private float WallClimbingspeed;
    // ~

    private void Awake()
    {
        // "Fetches" (not as in git, mind you) the RigidBody2D
        body = GetComponent<RigidBody2D>();
    }

    // Puts groundCheck to work
    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    // Puts wallCheck to work
    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }

    private void WallClimb()
    {
        if(IsWalled() && !IsGrounded() && horizontal != 0f)
        {
            isWallClimbing = true;
            body.linearVelocity = new Vector2(body.linearVelocityX, Mathf.Clamp(body.linearVelocityY, WallClimbingspeed, float.MaxValue));
        }
        else
        {
            isWallClimbing = false;
        }
    }

    private void Update()
    {
        // No idea honestly, just horizontal - what else could you ask for?
        // It's here just to have wall climbing's code work
        // since i'm pretty much trying to learn through YouTube videos and
        // I have no idea how to do it myself - I'm trying over here!
        horizontal = Input.GetAxisRaw("Horizontal");

        // Walking
        body.linearVelocity = new Vector2(Input.GetAxis("Horizontal") * speed, body.linearVelocityY);

        // Jump        
        if(Input.GetKey(KeyCode.K))
            body.linearVelocity = new Vector2(body.linearVelocityX, speed);
        
        // Wall climbing
        WallClimb();
    }
}
