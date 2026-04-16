using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

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

    [Header("Input")]
    [SerializeField] InputActionAsset inputActions;
    private Vector2 moveInput;
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
    [SerializeField] float wallRayOffset = 0.4f;    

    [Header("States")]
    public bool onGround;
    public bool onWall;
    public bool wallCling;
    public int wallDirection;
    public bool isDashing;
    public bool crouching;
    public bool lookingUp;
    public bool wallJumping;

    private Rigidbody2D rb;
    private TrailRenderer tr;

    [Header("Counters")]
    public int strawberryCounter = 0;

    void Awake()
    {
        inputActions["Move"].performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions["Move"].canceled += ctx => moveInput = Vector2.zero;

        inputActions["Jump"].started += ctx => jumpPressed = true;
        inputActions["Dash"].started += ctx => dashPressed = true;
    }

    void OnEnable() => inputActions.Enable();
    void OnDisable() => inputActions.Disable();

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        tr = GetComponent<TrailRenderer>();
    }

    void FixedUpdate()
    {
        CheckGround();
        CheckWall();
        CheckCling();

        HorizontalCheck();
        JumpCheck();
        LookCheck();
        DashCheck();

        jumpPressed = false;
        dashPressed = false;

        if (onGround) isAbleToDash = true;
    }

    void CheckCling()
    {
        if (wallCling)
        {
            rb.linearVelocity = new Vector2(0, -wallSpeed);
        }
    }
    void HorizontalCheck()
    {
        wallCling = false;
        if (isDashing) return;
        if(wallJumping && !onGround) return;
        if (onWall)
        {
            if (moveInput.x == 1 && wallDirection == 1) wallCling = true;
            else if (moveInput.x == -1 && wallDirection == -1) wallCling = true;
        }

        if (!wallCling)
        {
            rb.linearVelocity = new Vector2(moveInput.x * speed, rb.linearVelocity.y);
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
        }

        if (isDashing)
        {
            rb.linearVelocity = dashingDirection.normalized * dashingVelocity;
        }
    }

    void StartDash()
    {
        isDashing = true;
        isAbleToDash = false;
        tr.emitting = true;

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
        isDashing = false;
        tr.emitting = false;
    }

    void LookCheck()
    {
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
            leftWall = Physics2D.Raycast(pos, Vector2.left + Vector2.down * groundRayOffset, wallCheckDistance, groundLayer);
        }
        if (!leftWall)
        {
            leftWall = Physics2D.Raycast(pos, Vector2.left + Vector2.up * groundRayOffset, wallCheckDistance, groundLayer);
        }

        if (!rightWall)
        {
            rightWall = Physics2D.Raycast(pos, Vector2.right + Vector2.down * groundRayOffset, wallCheckDistance, groundLayer);
        }
        if (!rightWall)
        {
            rightWall = Physics2D.Raycast(pos, Vector2.right + Vector2.up * groundRayOffset, wallCheckDistance, groundLayer);
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

        Gizmos.DrawLine(pos + Vector2.up * groundRayOffset, pos + Vector2.up * groundRayOffset + Vector2.left * wallCheckDistance);
        Gizmos.DrawLine(pos + Vector2.up * groundRayOffset, pos + Vector2.up * groundRayOffset + Vector2.right * wallCheckDistance);

        Gizmos.DrawLine(pos + Vector2.down * groundRayOffset, pos + Vector2.down * groundRayOffset + Vector2.left * wallCheckDistance);
        Gizmos.DrawLine(pos + Vector2.down * groundRayOffset, pos + Vector2.down * groundRayOffset + Vector2.right * wallCheckDistance);
    }
}