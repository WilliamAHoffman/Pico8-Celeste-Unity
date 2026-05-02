using UnityEngine;

public class CrunchBlock : MonoBehaviour
{
    [SerializeField] GameObject spawn;
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>() != null)
        {
            if (collision.gameObject.GetComponent<PlayerController>().isDashing)
            {
                Instantiate(spawn, transform.position + new Vector3(0.5f,0.5f,0), Quaternion.identity);
                Destroy(gameObject);
            }
        }
    }
}


