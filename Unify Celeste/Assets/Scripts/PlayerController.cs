using System.Collections;
using System.Reflection.Emit;
using Unity.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float speed = 4f;
    [SerializeField] float jump = 5f;

    [Header("Wall")]
    [SerializeField] float wallJumpTime = 0.3f;
    [SerializeField] float wallSpeed = 1f;

    [Header("Dashing")]
    [SerializeField] float dashingVelocity = 10f;
    [SerializeField] float dashingTime = 0.5f;
    private Vector2 dashingDirection;
    public bool isAbleToDash = true;
    public bool isAbleToDoubleDash = false;
    public bool unlockedDoubleDash = false;

    [Header("Input")]
    [SerializeField] InputActionAsset inputActions;
    public Vector2 moveInput;
    private bool jumpPressed;
    private bool dashPressed;

    [Header("Jump Buffer")]
    [SerializeField] float jumpGraceTime = 0.1f;
    private float jumpGraceTimer;

    [Header("Raycasts")]
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float groundCheckDistance = 0.6f;
    [SerializeField] float wallCheckDistance = 0.6f;
    [SerializeField] float groundRayOffset = 0.4f;
    [SerializeField] float bottomWallRayOffset = 0.4f;
    [SerializeField] float topWallRayOffset = 0.4f;

    [Header("Extra")]
    [SerializeField] int deadly;

    [Header("States")]
    public bool onGround;
    public bool walking;
    public bool onWall;
    public bool wallCling;
    public int wallDirection;
    public bool isDashing;
    public bool crouching;
    public bool lookingUp;
    public bool wallJumping;
    public bool faceRight;


    private Rigidbody2D rb;
    private AudioSource ap;

    void Awake()
    {
        inputActions["Move"].performed += OnMovePerformed;
        inputActions["Move"].canceled += OnMoveCanceled;

        inputActions["Jump"].started += OnJumpStarted;
        inputActions["Dash"].started += OnDashStarted;

        ap = GetComponent<AudioSource>();
    }

    void OnEnable()
    {
        inputActions.Enable();
    }

    void OnDisable()
    {
        inputActions.Disable();
    }

    void OnDestroy()
    {
        inputActions["Move"].performed -= OnMovePerformed;
        inputActions["Move"].canceled -= OnMoveCanceled;

        inputActions["Jump"].started -= OnJumpStarted;
        inputActions["Dash"].started -= OnDashStarted;
    }

    private void OnMovePerformed(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
    }

    private void OnMoveCanceled(InputAction.CallbackContext ctx)
    {
        moveInput = Vector2.zero;
    }

    private void OnJumpStarted(InputAction.CallbackContext ctx)
    {
        jumpPressed = true;
    }

    private void OnDashStarted(InputAction.CallbackContext ctx)
    {
        dashPressed = true;
    }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        CheckGround();
        CheckWall();

        HorizontalCheck();
        CheckCling();
        JumpCheck();
        LookCheck();
        DashCheck();

        jumpPressed = false;
        dashPressed = false;

        if (onGround && (!isAbleToDash || (!isAbleToDoubleDash && unlockedDoubleDash)) && !isDashing)
        {
            ap.PlayOneShot(Resources.Load<AudioClip>("RegainDash"));
            isAbleToDash = true;
            if (unlockedDoubleDash)
            {
                isAbleToDoubleDash = true;
            }
        }
    }
    void CheckCling()
    {
        if (wallCling)
        {
            rb.linearVelocity = new Vector2(0, -wallSpeed);
            if(wallDirection == 1)
            {
                faceRight = true;
            }
            if(wallDirection == -1)
            {
                faceRight = false;
            }
        }
    }
    void HorizontalCheck()
    {
        wallCling = false;
        if (isDashing) return;
        if (wallJumping && !onGround) return;
        if (onWall && !onGround)
        {
            if (moveInput.x >= 0.5f && wallDirection == 1) wallCling = true;
            else if (moveInput.x <= -0.5f && wallDirection == -1) wallCling = true;
        }

        if (!wallCling)
        {
            rb.linearVelocity = new Vector2(moveInput.x * speed, rb.linearVelocity.y);
            if (moveInput.x > 0)
            {
                faceRight = true;
            }
            if (moveInput.x < 0)
            {
                faceRight = false;
            }
            if (moveInput.x == 0)
            {
                walking = false;
            }
            else
            {
                walking = true;
            }
        }
    }

    void JumpCheck()
    {
        if (jumpPressed)
        {
            if (onGround || onWall)
            {
                Jump();
            }
            else
            {
                jumpGraceTimer = jumpGraceTime;
            }
        }

        if (jumpGraceTimer > 0)
        {
            if (onGround || onWall)
            {
                Jump();
                jumpGraceTimer = 0;
            }
            else
            {
                jumpGraceTimer -= Time.fixedDeltaTime;
            }
        }
    }

    void Jump()
    {
        if (onGround)
        {
            ap.PlayOneShot(Resources.Load<AudioClip>("Jump"));
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jump);
        }
        else if (wallCling)
        {
            WallJump();
        }
    }

    void WallJump()
    {
        wallCling = false;
        ap.PlayOneShot(Resources.Load<AudioClip>("WallJump"));
        rb.linearVelocity = new Vector2(-wallDirection * jump, jump);
        wallJumping = true;
        StartCoroutine(StopWallJumping());
    }

    IEnumerator StopWallJumping()
    {
        yield return new WaitForSeconds(wallJumpTime);
        wallJumping = false;
    }

    void DashCheck()
    {
        if (dashPressed && isAbleToDash && !isDashing)
        {
            StartDash();
            ScreenShake shake = Camera.main.GetComponent<ScreenShake>();
            if (shake) shake.Shake();

        }

        if (isDashing)
        {
            if (dashingDirection.x > 0)
            {
                faceRight = true;
            }
            if (dashingDirection.x < 0)
            {
                faceRight = false;
            }
            rb.linearVelocity = dashingDirection.normalized * dashingVelocity;
        }
    }

    void StartDash()
    {
        isDashing = true;
        ap.PlayOneShot(Resources.Load<AudioClip>("Dash"));
        if (isAbleToDoubleDash)
        {
            isAbleToDoubleDash = false;
        }
        else
        {
            isAbleToDash = false;
        }

        dashingDirection = moveInput;

        if (dashingDirection == Vector2.zero)
        {
            dashingDirection = new Vector2(transform.localScale.x, 0);
        }

        StartCoroutine(StopDashing());
    }

    IEnumerator StopDashing()
    {
        yield return new WaitForSeconds(dashingTime);
        rb.linearVelocity = rb.linearVelocity.normalized * speed;
        isDashing = false;
    }

    void LookCheck()
    {
        if (!onGround)
        {
            lookingUp = false;
            crouching = false;
            return;
        }
        lookingUp = onGround && moveInput.y > 0.5f;
        crouching = onGround && moveInput.y < -0.5f;
    }

    void CheckGround()
    {
        Vector2 pos = transform.position;

        RaycastHit2D left = Physics2D.Raycast(pos + Vector2.left * groundRayOffset, Vector2.down, groundCheckDistance, groundLayer);
        RaycastHit2D right = Physics2D.Raycast(pos + Vector2.right * groundRayOffset, Vector2.down, groundCheckDistance, groundLayer);

        onGround = left.collider != null || right.collider != null;
    }

    void CheckWall()
    {
        Vector2 pos = transform.position;

        RaycastHit2D leftWall = Physics2D.Raycast(pos, Vector2.left, wallCheckDistance, groundLayer);
        RaycastHit2D rightWall = Physics2D.Raycast(pos, Vector2.right, wallCheckDistance, groundLayer);

        if (!leftWall)
        {
            leftWall = Physics2D.Raycast(pos, Vector2.left + Vector2.down * bottomWallRayOffset, wallCheckDistance, groundLayer);
        }
        if (!leftWall)
        {
            leftWall = Physics2D.Raycast(pos, Vector2.left + Vector2.up * topWallRayOffset, wallCheckDistance, groundLayer);
        }

        if (!rightWall)
        {
            rightWall = Physics2D.Raycast(pos, Vector2.right + Vector2.down * bottomWallRayOffset, wallCheckDistance, groundLayer);
        }
        if (!rightWall)
        {
            rightWall = Physics2D.Raycast(pos, Vector2.right + Vector2.up * topWallRayOffset, wallCheckDistance, groundLayer);
        }

        if (leftWall.collider != null)
        {
            onWall = true;
            wallDirection = -1;
        }
        else if (rightWall.collider != null)
        {
            onWall = true;
            wallDirection = 1;
        }
        else
        {
            onWall = false;
            wallDirection = 0;
        }
    }

    public void ReloadPlayer()
    {
        faceRight = true;
        isDashing = false;
        onGround = true;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector2 pos = transform.position;

        // ground
        Gizmos.DrawLine(pos + Vector2.left * groundRayOffset, pos + Vector2.left * groundRayOffset + Vector2.down * groundCheckDistance);
        Gizmos.DrawLine(pos + Vector2.right * groundRayOffset, pos + Vector2.right * groundRayOffset + Vector2.down * groundCheckDistance);

        // walls
        Gizmos.DrawLine(pos, pos + Vector2.left * wallCheckDistance);
        Gizmos.DrawLine(pos, pos + Vector2.right * wallCheckDistance);

        Gizmos.DrawLine(pos + Vector2.up * topWallRayOffset, pos + Vector2.up * topWallRayOffset + Vector2.left * wallCheckDistance);
        Gizmos.DrawLine(pos + Vector2.up * topWallRayOffset, pos + Vector2.up * topWallRayOffset + Vector2.right * wallCheckDistance);

        Gizmos.DrawLine(pos + Vector2.down * bottomWallRayOffset, pos + Vector2.down * bottomWallRayOffset + Vector2.left * wallCheckDistance);
        Gizmos.DrawLine(pos + Vector2.down * bottomWallRayOffset, pos + Vector2.down * bottomWallRayOffset + Vector2.right * wallCheckDistance);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == deadly)
        {
            ap.PlayOneShot(Resources.Load<AudioClip>("Death"));

            //there should be a delay here and a chance for the particle anim on death to play
            FindFirstObjectByType<GameManager>().StartReload();
            LevelStorage.instance.numDeaths++;
        }
    }
}