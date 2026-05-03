using Unity.Mathematics;
using UnityEngine;

public class LinearMovement : MonoBehaviour
{
    public float xSpeed;
    public float ySpeed;
    [SerializeField] float deathX;
    [SerializeField] float deathY;
    [SerializeField] bool reload;
    [SerializeField] float waitTime = 2;
    [SerializeField] bool destoryer = true;
    public Vector3 startLocation;
    private float waitTimer = -1;

    void Start()
    {
        startLocation = transform.position;
    }
    void FixedUpdate()
    {
        transform.position += new Vector3(xSpeed * Time.deltaTime, ySpeed * Time.deltaTime, 0);

        if (waitTimer >= 0)
        {
            waitTimer -= Time.deltaTime;
        }
        if (math.abs(transform.position.x) >= deathX || math.abs(transform.position.y) >= deathY)
        {
            if (waitTimer < 0)
            {
                if (reload)
                {
                    waitTimer = waitTime;
                    transform.position = new Vector3(-transform.position.x, transform.position.y, 0);
                }
                else
                {
                    if(destoryer) Destroy(gameObject);
                    else
                    {
                        ySpeed = 0;
                        xSpeed = 0;
                    }
                }
            }
        }
    }
}
