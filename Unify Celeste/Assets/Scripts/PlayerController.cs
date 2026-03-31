using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 8f;

    // Consistently calling movement functions.
    void Update()
    {
        HorizontalMovement();
        Jump();
        Crouch();
    }

    void HorizontalMovement()
    {
        float horizontalMovement = Input.GetAxis("Horizontal");
        Vector3 movement = new Vector3(horizontalMovement, 0f, 0f);
        transform.Translate(movement * speed * Time.deltaTime); 
    }

    void Jump()
    {
        
    }

    void Crouch()
    {
        
    }
}
