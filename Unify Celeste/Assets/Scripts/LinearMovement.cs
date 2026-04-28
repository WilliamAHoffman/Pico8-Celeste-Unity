using Unity.Mathematics;
using UnityEngine;

public class LinearMovement : MonoBehaviour
{
    [SerializeField] float xSpeed;
    [SerializeField] float ySpeed;
    [SerializeField] float deathX;

    void FixedUpdate()
    {
        if(math.abs(transform.position.x) >= deathX)
        {
            Destroy(gameObject);
        }
        transform.position += new Vector3(xSpeed * Time.deltaTime,ySpeed * Time.deltaTime,0);
    }
}
