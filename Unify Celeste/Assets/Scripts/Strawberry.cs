using UnityEngine;

public class Strawberry : MonoBehaviour
{
    private SpriteRenderer sr;
    private Collider2D cl;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        cl = GetComponent<Collider2D>();
    }
    
    private void OnTriggerEnter2D(Collider2D c)
    {
        if (c.gameObject.GetComponent<PlayerController>())
        {
            sr.enabled = false;
            cl.enabled = false;
            if(LevelStorage.instance) LevelStorage.instance.totalStrawberries++;
        }
    }
}
