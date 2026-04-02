using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float speed = 8f;
    [SerializeField] float jump = 5f;
    private Rigidbody2D rb;
    private bool onGround;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if(rb == null)
        {
            Debug.LogError("player is missing a rigid body");
        }
    }

    // Consistently calling movement functions.
    void FixedUpdate()
    {
        HorizontalMovement();
        Jump();
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