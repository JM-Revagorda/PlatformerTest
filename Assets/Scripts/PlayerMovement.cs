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

    [Header("Timers")]
    public float LastOnGroundTime { get; private set; }

    //Other Essentials
    Rigidbody2D rb;
    PlayerControls playerControls;
    bool isGrounded;
    Vector2 directionMove;
    float origStamina;
    float currentSpeed = 0;
    [HideInInspector] public bool canDash;
    [HideInInspector] public bool isDashing;
    bool canClimb;
    bool isClimbing;
    bool rightFlip;
    bool isDead = false;

    //Acceleration varaibles
    private void Awake()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        isGrounded = false;
        playerControls = new PlayerControls();
        rb.gravityScale = gravityScale;
        canDash = true;
        isDashing = false;
        canClimb = false;
        isClimbing = false;
        origStamina = stamina;
        rightFlip = true;
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
    public void OnJump(InputAction.CallbackContext context) {
        if (isGrounded && !isDashing) {
            rb.linearVelocityY = jumpHeight;
        }
        if (isClimbing) {
            isClimbing = false;
            rb.linearVelocity = new Vector2(-directionMove.x * moveSpeed, jumpHeight);
        }
    }

    //Dashing Movement
    public void OnDash(InputAction.CallbackContext context) {
        if(canDash && !isDashing) StartCoroutine(DashFunc());
    }

    private void Update()
    {
        if (!isDashing && !isClimbing)
        {
            if (directionMove.x != 0f)
            {
                //float targetSpeed = directionMove.x * moveSpeed;
                //float rate = Mathf.Abs(directionMove.x) > 0.01f ? runAccelAmount : runDeccelAmount;
                //currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, rate * Time.fixedDeltaTime * moveSpeed);
                //rb.linearVelocity = new Vector2(currentSpeed, rb.linearVelocityY);
                Vector2 targetVelocity = new Vector2(directionMove.x * moveSpeed, rb.linearVelocityY);
                if (Mathf.Abs(rb.linearVelocityX) < moveSpeed || Mathf.Sign(directionMove.x) != Mathf.Sign(rb.linearVelocityX))
                {
                    if (isGrounded)
                    {
                        rb.linearDamping = 0;
                    }
                    rb.linearVelocityX = targetVelocity.x;
                }
            }
            else {
                if (isGrounded) rb.linearDamping = decelRate;
                else rb.linearDamping = 0;
            }

            if (rb.linearVelocityY > 0 && !isGrounded && Keyboard.current.zKey.wasReleasedThisFrame)
            {
                rb.gravityScale = reducedGravityScale;
            } else rb.gravityScale = gravityScale;
        }

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

        //Sprite and Wall Collision Flipping
        if (directionMove.x > 0 && !rightFlip) Flip();
        else if (directionMove.x < 0 && rightFlip) Flip();
    }

    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(point.transform.position, radiusCollision, groundLayer);
        canClimb = Physics2D.OverlapCircle(wallPoint.transform.position, 0.1f, groundLayer);
        if (isGrounded) { canDash = true; stamina = origStamina; }
    }

    void Flip() {
        if (rightFlip)
        {
            rightFlip = false;
        }
        else {
            rightFlip = true;
        }
        wallPoint.transform.localPosition = new Vector3(wallPoint.transform.localPosition.x * -1, wallPoint.transform.localPosition.y);
    }

    //Dashing Function
    IEnumerator DashFunc() {
        isDashing = true;
        int dashDirection = transform.localPosition.x > 0 ? 1 : -1;
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
        Destroy(gameObject);
    }

    //Colliders
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("MovingPlatform"))
        {
            transform.parent = collision.transform;
        }
    } 
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("MovingPlatform"))
        {
            transform.parent = null;
        }
    }
}
