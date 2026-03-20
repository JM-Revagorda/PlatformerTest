using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    //Variables for Player Movement
    [Header("Player Movement")]
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpHeight;
    [SerializeField] float reducedGravityScale;
    [SerializeField] float gravityScale = 1f;
    [SerializeField] float dashSpeed = 8f;
    [SerializeField] float decelRate;
    [SerializeField] float coyoteThreshold;

    //Variables for Player Climbing
    [Header("ClimbingMovement")]
    [SerializeField] GameObject wallPoint;
    [SerializeField] float climbSpeed;
    [SerializeField] float stamina;

    //Variables and GameObject for Player Collision
    [Header("Collision Checker")]
    [SerializeField] LayerMask groundLayer;
    [SerializeField] GameObject point;
    [SerializeField] float radiusCollision;

    //Variables and GameObject for Handling Player's Death
    [Header("Death Point")]
    [SerializeField] GameObject deathPoint;
    [HideInInspector] public GameObject deathParticles;

    //GameObject for Handling Player's Respawn
    [Header("Managers")]
    [SerializeField] GameObject respawnManager;
    RespawnManager rmControl;

    //GameObject for Dash Particles
    [Header("Other Points")]
    [SerializeField] GameObject dashpPoint;
    GameObject dashPointObject;

    //Other Essentials
    Rigidbody2D rb;
    PlayerControls playerControls;
    [HideInInspector]public Animator animator;
    ParticleSystem dashParticles;
    bool isGrounded;
    Vector2 directionMove;
    float origStamina;
    float currentSpeed = 0;
    [HideInInspector] public bool canDash;
    [HideInInspector] public bool isDashing;
    GameObject movingPlatform = null;
    bool canClimb;
    bool isClimbing;
    bool rightFlip;
    bool isDead = false;
    float _timeLeftGrounded = 0;
    Vector2 platformVelocity = Vector2.zero;

    /*
        Notes on All the things above:
        [HideInInspector]   - Prevents public variables from being seen in the Inspector. Works when you simply want to have it only accessible by
                            other Scripts
        [SerializeField]    - Allows any variable regardless of security to be seen in the Inspector (shouldnt theoretically work in public variables 'cos their in 'public', duh!)
        Vector2             - Type of Data that reads in Cartesian coordinates (e.g. (0, 1), (5, -4))
        float               - still the same old float data as the one in Java where you use 'f' or 'F' to differentiate a float from a double
        ParticleSystem      - Wrapper Class for the ParticleSystem Component
        PlayerControls      - Wrapper Class for the generated PlayerControls Script, made using the magical powers of Input System (keep it up baby!)
        GameObject          - GameObject....
        Rigidbody           - Wrapper class for the Rigidbody Component
     */

    //Happens at the start of the Scene
    private void Awake()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>(); //GetComponent<*something*>() = Gets the component of the gameobject. In this case, it gets the RigidBody 2D Component and stores it in 'rb'
        if (animator == null) animator = GetComponent<Animator>();
        isGrounded = false;
        playerControls = new PlayerControls();
        rb.gravityScale = gravityScale;
        canDash = true;
        isDashing = false;
        canClimb = false;
        isClimbing = false;
        origStamina = stamina;
        rightFlip = true;
        _timeLeftGrounded = 0;
        dashParticles = GetComponent<ParticleSystem>();
    }

    //Happens when the object is instantiated with this script!
    private void Start()
    {
        respawnManager = GameObject.FindWithTag("RespawnManager");
        rmControl = respawnManager.GetComponent<RespawnManager>();
    }

    //Movement Inputs

    #region RUN/MOVE INPUT
    //Directional Movement
    public void OnMove(InputAction.CallbackContext context)
    {
        //Gets All the Directions based on Arrow Keys
        directionMove = context.ReadValue<Vector2>();
    }
    #endregion

    //Jumping Movement
    #region JUMP INPUT
    public void OnJump(InputAction.CallbackContext context) {
        int reverser = -1;

        //Regular Jumping Logic
        if (isGrounded && !isDashing || (_timeLeftGrounded + coyoteThreshold > Time.time && rb.linearVelocityY < 0)) {
            rb.linearVelocityY = jumpHeight + platformVelocity.y ;
        }
        //WallJump, When you Jump while holding 'C'
        if (isClimbing) {
            isClimbing = false;
            if (!rightFlip) reverser = 1;
            rb.linearVelocity = new Vector2((reverser * transform.right.x) * moveSpeed, jumpHeight);
        }
    }
    #endregion

    //Dashing Movement
    #region DASH INPUT
    public void OnDash(InputAction.CallbackContext context) {
        if(canDash && !isDashing) StartCoroutine(DashFunc());
    }
    #endregion

    /* *funcName*(InputAction.CallbackContext context)
            - Way to call functions when specific keys are being pressed
            - You add these functions in the 'Player Input' Component in the Inspector, after changing its mode to 'Send Messages' and its Input System to any specified Input System
     */

    private void Update()
    {
        #region Velocity Calculations Mechanic
        //Checks if any object is referenced in movingPlatform 
        if (movingPlatform != null)
        {
            //Gets platform Velocty
            platformVelocity = movingPlatform.GetComponent<VelocityCalculator>().GetVelocity();
        }
        else platformVelocity = Vector2.zero; //Sets it to Zero

        if (!isDashing && !isClimbing)
        {
            if (directionMove.x != 0f)
            {
                //Prevents player from 'sticking' against a wall using the 'canClimb variable'
                if (canClimb && !isGrounded)
                {
                    directionMove.x = 0f;
                    //rb.linearVelocityY = 0.70f;
                }
                else
                {
                    //Horizontal Movement and Velocity
                    Vector2 targetVelocity = new Vector2(directionMove.x * moveSpeed + platformVelocity.x, rb.linearVelocityY + platformVelocity.y);

                    //Running Animation Activation
                    if (isGrounded) animator.SetBool("isRunning", true);
                    else animator.SetBool("isRunning", false);

                    //Allows changing of momentum when the opposite key is pressed
                    if (Mathf.Abs(rb.linearVelocityX) < moveSpeed || Mathf.Sign(directionMove.x) != Mathf.Sign(rb.linearVelocityX))
                    {
                        if (isGrounded)
                        {
                            rb.linearDamping = 0;
                        }

                        rb.linearVelocity = targetVelocity;
                    }
                }
            }
            else {
                if (isGrounded) rb.linearDamping = decelRate; // Stops abruptly
                else rb.linearDamping = 0;

                if (movingPlatform != null) rb.linearVelocity = new Vector2(platformVelocity.x, platformVelocity.y); // Allows the player to move with the platform
                animator.SetBool("isRunning", false);
            }

            //Allows for short hops(I dont think this one works though!)
            if (rb.linearVelocityY > 0 && !isGrounded && Keyboard.current.zKey.wasReleasedThisFrame)
            {
                rb.gravityScale = reducedGravityScale;
            } else rb.gravityScale = gravityScale;
        }
        #endregion
        //Climbing Mechanic
        if (Keyboard.current.cKey.isPressed && canClimb) isClimbing = true;
        else isClimbing = false;

        //Climbing Logic
        if (isClimbing && stamina > 0 && !Keyboard.current.zKey.wasPressedThisFrame)
        {   
                //Makes the Player only move up and down, decreasing their stamina along the way until the climb key is released and their back on the ground again (isGround is true)
                rb.gravityScale = 0;
                rb.linearVelocityY = directionMove.y * climbSpeed;
                stamina -= 0.5f;
        }
        else
        {
            rb.gravityScale = 1f;
        }

        //Other non-walking Animations
        animator.SetBool("isDashing", isDashing);
        animator.SetBool("isClimbing", isClimbing);

        //Freeze when Player is just clinging on the wall, Unfreeze if they aint
        if (isClimbing && directionMove.y == 0) { animator.speed = 0f; }
        else animator.speed = 1f;

        //Sprite and Wall Collision Flipping
        if (!isClimbing)
        {
            if (((directionMove.x > 0) || (!isGrounded && rb.linearVelocityX > 0)) && !rightFlip) Flip();
            else if (((directionMove.x < 0) || (!isGrounded && rb.linearVelocityX < 0)) && rightFlip) Flip();
        }

        //'Coyote' Timing
        //  - Cool mechanic that was definitely not inspired by Looney Tunes. Allows the player to jump even when they go through a ledge for a very short time
        if (!isGrounded && _timeLeftGrounded <= coyoteThreshold) { _timeLeftGrounded += Time.time;} //This works by increasing _timeLeftGrounded by the game's time per frame until its greater than coyoteThreshold
        animator.SetFloat("VerticalMove", rb.linearVelocityY);
        //Debug.Log(animator.GetFloat("VerticalMove"));
    }

    private void FixedUpdate()
    {
        //Collision Points
        isGrounded = Physics2D.OverlapCircle(point.transform.position, radiusCollision, groundLayer);
        canClimb = Physics2D.OverlapCircle(wallPoint.transform.position, 0.1f, groundLayer);

        //Resets everything back to its original place
        if (isGrounded) { 
            canDash = true; 
            stamina = origStamina;
            _timeLeftGrounded = 0;
        }
        animator.SetBool("isGrounded", isGrounded);
        //Debug.Log(canClimb);
    }

    void Flip() {
        //It does what it does, though what I found interesting is that even the axis is inverted
        if (rightFlip)
        {
            rightFlip = false;
        }
        else {

            rightFlip = true;
        }
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y) ;
    }

    //Dashing Function
    IEnumerator DashFunc() {
        //Starts Dashing based on the Directional Input
        isDashing = true;
        dashPointObject = Instantiate(dashpPoint, transform.position, Quaternion.identity); // For the Dash particle
        int dashDirection = transform.localScale.x > 0 ? 1 : -1;
        rb.gravityScale = 0;
        if (directionMove == Vector2.zero) rb.linearVelocity =  new Vector2(dashDirection * dashSpeed, 0);
        else {
            rb.linearVelocity = new Vector2(directionMove.x * dashSpeed, directionMove.y * dashSpeed);
        }
        if (isGrounded && directionMove.y < 0) { Destroy(dashPointObject); StopCoroutine(DashFunc()); }
        yield return new WaitForSeconds(0.3f); //Pauses until 0.3 seconds is up
        Destroy(dashPointObject);
        canDash = false;
        isDashing = false;
        rb.gravityScale = 1;
        rb.linearVelocity = new Vector2(directionMove.x * moveSpeed, rb.linearVelocityY);
        StopCoroutine(DashFunc()); //Stops the function from looping again
    }

    //Death Function
    public void OnDeath() {
        deathParticles = Instantiate(deathPoint, transform.position, Quaternion.identity);
        rmControl.runRespawnFunc(gameObject);
    }

    //Collision Checkers
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("MovingPlatform"))
        {
            movingPlatform = collision.gameObject;
        }
    } 
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("MovingPlatform"))
        {
            movingPlatform = null;
        }
    }
}

//Very solid Player Controller!

/*
 * Some Tutorial
 * 
 * How to show in [Inspector]:
 * 1. Simply write down global variables at the top, just like how you would write a class' variables down
 *      public int score = 0
 *          - If you omit the starting assignment, Unity will instead show it as a field in Inspector
 * 
 * void Start()
 *      - runs when the instance has been created, or rather "Instantiated"
 * 
 * void Awake()
 *      - runs at the start of the Scene
 * 
 * void Update()
 *      - runs for every frame per second. Your only way to run what you want to run, other than waiting for something else to interact with
 *      Be careful to not place lots of things as this is always done _every_frame_ (basically less than a second)
 *      
 * void FixedUpdate()
 *      - similar to Update(), but instead runs for every 2 or so frames
 *      
 * OnCollisionEnter(Collider collision)
 *      - runs when something is collided with your object. The "collider" or interactor is the collision variable, which can be used to either 
 *      check if it is indeed a certain object or not, or store the object directly into a certain GameObject variable
 *      - use OnCollisionEnter2D(Collider2D) for 2D games and collisions
 * 
 * OnCollisionExit(Collider collision)
 *      - runs when something that had collided with your object stopped colliding.
 *      - use OnCollisionExit2D(Collider2D collision) likewise
 * 
 * rb.LinearVelocity
 *      - the linear velocity of the object, as a Vector2 type; (x, y)
 *      - to change the linearVelocity, simply do:
 *      
 *              rb.LinearVelocity = new Vector2(*the x you want your object to go*, *the y you want your object to go*);
 *              
 *      - also has interesting variables to interact with alone
 *              - rb.LinearVelocityX = to interact only the horizontal velocity
 *              - rb.LinearVelocityY = to interact only the vertical velocity
 *
 * rb.gravityScale
 *      - sets/gets the gravity Scale of the object in the scene
 *
 * rb.linearDamping
 *      - sets/gets the amount of friction of the object, both in land and in air
 *      
 *      
 *      
 *      
 *      
 *      
 *      
 *      
 *      Snake: Colonel!
 *      Colonel: What is it Snake?
 *      Snake: Ive done a stinky *farts*
 *      Colonel: Huh?!
 *      Snake: Yeah, and a big one proper one *farts harder*
 *      Colonel: What the... Snake!
 *      Snake: *rips the big one out* You hear that Colonel? I've done it again! You know what Im saying man? *giggles*
 *      Colonel: Snake you gotta stop that, or else something...
 *      
 *      Enemy: What was that smell?
 *      Snake: Uh oh... Colonel. I think, *gulps*, I think * G U L P S 2x*, I think they spotted me. I think I gotta hide
 *      Colonel: Ddue! WTF,  GET THE FUCK OUT OF THERE!!!!!
 *      *cue to Enemy Spotting Snake, then Snake dying from the shots*
 *      
 *      G A M E ____ O V E R
 */
