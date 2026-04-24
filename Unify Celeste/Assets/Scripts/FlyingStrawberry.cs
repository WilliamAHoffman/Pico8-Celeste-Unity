using UnityEditor.Callbacks;
using UnityEngine;

public class FlyingStrawberry : MonoBehaviour
{
    private SpriteRenderer sr;
    private Collider2D cl;

    private Rigidbody2D rb;

    private PlayerController player;
    public bool canFly = false;

    // does not regenerate on scene reload upon death

    void Start()
    {
        player = FindFirstObjectByType<PlayerController>();
        sr = GetComponent<SpriteRenderer>();
        cl = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!canFly && player.isDashing)
        {
            canFly = true;
        }

        Flying();
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

    private void Flying()
    {
        if (canFly)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 5f);
        }
    }
}
