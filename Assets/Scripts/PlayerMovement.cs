using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player Movement")]
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpHeight;
    [SerializeField] float reducedGravityScale;
    [SerializeField] float gravityScale = 1f;
    [SerializeField] float dashSpeed = 8f;
    [SerializeField] float decelRate;
    [SerializeField] float coyoteThreshold;

    [Header("ClimbingMovement")]
    [SerializeField] GameObject wallPoint;
    [SerializeField] float climbSpeed;
    [SerializeField] float stamina;


    [Header("Collision Checker")]
    [SerializeField] LayerMask groundLayer;
    [SerializeField] GameObject point;
    [SerializeField] float radiusCollision;

    [Header("Death Point")]
    [SerializeField] GameObject deathPoint;

    [Header("Managers")]
    [SerializeField] GameObject respawnManager;
    RespawnManager rmControl;

    [Header("Other Points")]
    [SerializeField] GameObject dashpPoint;

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

    //Acceleration varaibles
    private void Awake()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
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
        directionMove = context.ReadValue<Vector2>();
    }
    #endregion

    //Jumping Movement
    #region JUMP INPUT
    public void OnJump(InputAction.CallbackContext context) {
        if (isGrounded && !isDashing || (_timeLeftGrounded + coyoteThreshold > Time.time && rb.linearVelocityY < 0)) {
            rb.linearVelocityY = jumpHeight + platformVelocity.y ;
        }
        if (isClimbing) {
            isClimbing = false;
            rb.linearVelocity = new Vector2(-directionMove.x * moveSpeed, jumpHeight);
        }
    }
    #endregion

    //Dashing Movement
    #region DASH INPUT
    public void OnDash(InputAction.CallbackContext context) {
        if(canDash && !isDashing) StartCoroutine(DashFunc());
    }
    #endregion

    private void Update()
    {
        #region Velocity Calculations Mechanic
        if (movingPlatform != null)
        {
            platformVelocity = movingPlatform.GetComponent<VelocityCalculator>().GetVelocity();
        }
        else platformVelocity = Vector2.zero;

        if (!isDashing && !isClimbing)
        {
            if (directionMove.x != 0f)
            {
                Vector2 targetVelocity = new Vector2(directionMove.x * moveSpeed + platformVelocity.x, rb.linearVelocityY + platformVelocity.y);
                if (isGrounded) animator.SetBool("isRunning", true);
                else animator.SetBool("isRunning", false);

                if (Mathf.Abs(rb.linearVelocityX) < moveSpeed || Mathf.Sign(directionMove.x) != Mathf.Sign(rb.linearVelocityX))
                {
                    if (isGrounded)
                    {
                        rb.linearDamping = 0;
                    }
                    
                    rb.linearVelocity = targetVelocity;
                    //rb.linearVelocityX = targetVelocity.x;
                }
            }
            else {
                if (isGrounded) rb.linearDamping = decelRate;
                else rb.linearDamping = 0;

                if (movingPlatform != null) rb.linearVelocity = new Vector2(platformVelocity.x, platformVelocity.y);
                animator.SetBool("isRunning", false);
            }

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
        if (isClimbing && stamina > 0)
        {
                rb.gravityScale = 0;
                rb.linearVelocityX = directionMove.x != 0? directionMove.x * moveSpeed : 0;
                rb.linearVelocityY = directionMove.y * climbSpeed;
                stamina -= 0.5f;
        }
        else
        {
            rb.gravityScale = 1f;
        }
        
        animator.SetBool("isDashing", isDashing);
        animator.SetBool("isClimbing", isClimbing);
        if (isClimbing && directionMove.y == 0) { animator.speed = 0f; }
        else animator.speed = 1f;

        //Sprite and Wall Collision Flipping
        if (directionMove.x > 0 && !rightFlip) Flip();
        else if (directionMove.x < 0 && rightFlip) Flip();

        //'Coyote' Timing
        if (!isGrounded && _timeLeftGrounded <= coyoteThreshold) { _timeLeftGrounded += Time.time;}
        
    }

    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(point.transform.position, radiusCollision, groundLayer);
        canClimb = Physics2D.OverlapCircle(wallPoint.transform.position, 0.3f, groundLayer);
        if (isGrounded) { 
            canDash = true; 
            stamina = origStamina;
            _timeLeftGrounded = 0;
        }
        Debug.Log(canClimb);
    }

    void Flip() {
        if (rightFlip)
        {
            rightFlip = false;
        }
        else {

            rightFlip = true;
        }
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y) ;
        wallPoint.transform.localPosition = new Vector3(wallPoint.transform.localPosition.x * -1, wallPoint.transform.localPosition.y);
    }

    //Dashing Function
    IEnumerator DashFunc() {
        isDashing = true;
        Instantiate(dashpPoint, transform.position, Quaternion.identity);
        //ParticleEmit(transform.position, 1);
        int dashDirection = transform.localScale.x > 0 ? 1 : -1;
        rb.gravityScale = 0;
        if (directionMove == Vector2.zero) rb.linearVelocity =  new Vector2(dashDirection * dashSpeed, 0);
        else {
            rb.linearVelocity = new Vector2(directionMove.x * dashSpeed, directionMove.y * dashSpeed);
        }
        yield return new WaitForSeconds(0.3f);
        canDash = false;
        isDashing = false;
        rb.gravityScale = 1;
        rb.linearVelocity = new Vector2(directionMove.x * moveSpeed, rb.linearVelocityY);
        StopCoroutine(DashFunc());
    }

    //Death Function
    public void OnDeath() {
        Instantiate(deathPoint, transform.position, Quaternion.identity);
        rmControl.runRespawnFunc(gameObject);
    }

    //Colliders
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("MovingPlatform"))
        {
            //transform.parent = collision.transform;
            movingPlatform = collision.gameObject;
        }
    } 
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("MovingPlatform"))
        {
            //transform.parent = null;
            movingPlatform = null;
        }
    }
    private void ParticleEmit(Vector2 position, int count)
    {
        var EmitParams = new ParticleSystem.EmitParams();
        EmitParams.position = position;
        dashParticles.Emit(EmitParams, count);
    }
}
