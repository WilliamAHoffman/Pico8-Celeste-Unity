using UnityEngine;

public class Key : MonoBehaviour
{
    private SpriteRenderer sr;
    private Collider2D cl;
    public static bool keyCollected = false;
    AudioSource ap;


    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        cl = GetComponent<Collider2D>();
        ap = GetComponent<AudioSource>();
    }
    
    private void OnTriggerEnter2D(Collider2D c)
    {
        if (c.gameObject.GetComponent<PlayerController>())
        {
            sr.enabled = false;
            cl.enabled = false;
            keyCollected = true;
            ap.PlayOneShot(Resources.Load<AudioClip>("CollectKey"));
            Debug.Log("key collected");
        }
    }
}
