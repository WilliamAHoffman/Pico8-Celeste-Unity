using System.Collections;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private TrailRenderer tr;

    [Header("Movement")]
    [SerializeField] float speed = 8f;
    [SerializeField] float jump = 5f;
    private Rigidbody2D rb;
    private bool onGround;

    [Header("Dashing")]
    [SerializeField] float dashingVelocity = 10f;
    [SerializeField] float dashingTime = 0.5f;
    private Vector2 dashingDirection;
    private bool isDashing;
    private bool isAbleToDash = true;
        
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if(rb == null)
        {
            Debug.LogError("player is missing a rigid body");
        }

        tr = GetComponent<TrailRenderer>();
    }

    // Consistently calling movement functions.
    void FixedUpdate()
    {
        HorizontalMovement();
        Jump();

    }

    void Update()
    {
        var dashInput = Input.GetButtonDown("Dash");

        if (dashInput && isAbleToDash)
        {
            isDashing = true;
            isAbleToDash = false;
            tr.emitting = true;
            dashingDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            if (dashingDirection == Vector2.zero)
            {
                dashingDirection = new Vector2(transform.localScale.x, 0); // we want atlas to stay in place if theres no direction, change later
            }
            StartCoroutine(StopDashing());
        }

        // animator.SetBool("IsDashing", isDashing)

        if (isDashing)
        {
            rb.linearVelocity = dashingDirection.normalized * dashingVelocity;
            return;
        }

        if (onGround)
        {
            isAbleToDash = true;
        }
    }

    private IEnumerator StopDashing()
    {
        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;
        isDashing = false;
    }

    void HorizontalMovement()
    {
        float horizontalMovement = Input.GetAxis("Horizontal");
        Vector3 movement = new Vector3(horizontalMovement, 0f, 0f);
        transform.Translate(movement * speed * Time.deltaTime); 
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.C) && onGround)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jump);
        }
    }

    //THIS NEEDS TO VERIFY THAT THE PLAYER IS NOT TOUCHING A WALL
    //Dont use collision enter and exit
    //Instead raycast directly down every frame to confirm that there is a floor directly below the player

    private void OnCollisionEnter2D(Collision2D c)
    {
        Debug.Log(c);
        if (c.gameObject.CompareTag("Ground"))
        {
            onGround = true;
        }
    }

    private void OnCollisionExit2D(Collision2D c)
    {
         if (c.gameObject.CompareTag("Ground"))
         {
             onGround = false;
         }
    }
}