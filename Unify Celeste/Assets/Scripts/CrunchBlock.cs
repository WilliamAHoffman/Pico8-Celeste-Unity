using UnityEngine;

public class CrunchBlock : MonoBehaviour
{
    [SerializeField] GameObject spawn;
    AudioSource ap;

    private void Start()
    {
        ap = GetComponent<AudioSource>();
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>() != null)
        {
            if (collision.gameObject.GetComponent<PlayerController>().isDashing)
            {
                Instantiate(spawn, transform.position + new Vector3(0.5f,0.5f,0), Quaternion.identity);
                if (LevelStorage.instance) LevelStorage.instance.PlaySFX(Resources.Load<AudioClip>("CrunchBlock"));
            
                Destroy(gameObject);
            }
        }
    }
}


