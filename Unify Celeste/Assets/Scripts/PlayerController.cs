using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 8f;

    public float jump = 5f;
    public Rigidbody2D rb;
    // public bool onGround = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Consistently calling movement functions.
    void Update()
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
        if (Input.GetKeyDown(KeyCode.C))
        //if (Input.GetKeyDown(KeyCode.C) && onGround)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jump);
        }
    }

    // private void OnCollisionEnter2D(Collision2D c)
    // {
    //     if (c.gameObject.CompareTag("Ground"))
    //     {
    //         onGround = true;
    //     }
    // }

    // private void OnCollisionExit2D(Collision2D c)
    // {
    //     if (c.gameObject.CompareTag("Ground"))
    //     {
    //         onGround = false;
    //     }
    // }
}
