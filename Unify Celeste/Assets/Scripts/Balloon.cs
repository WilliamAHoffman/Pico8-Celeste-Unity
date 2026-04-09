using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class Balloon : MonoBehaviour
{
    public PlayerController pc;
    private SpriteRenderer sr;
    private Collider2D cl;

    void Start()
    {
        pc = FindFirstObjectByType<PlayerController>();
        sr = GetComponent<SpriteRenderer>();
        cl = GetComponent<Collider2D>();
        
    }
    
    private void OnTriggerEnter2D(Collider2D c)
    {
        if (c.gameObject.CompareTag("Player"))
        {
            pc.isAbleToDash = true;
            sr.enabled = false;
            cl.enabled = false;
            StartCoroutine(RespawnBalloon());
        }
    }

    IEnumerator RespawnBalloon()
    {
        yield return new WaitForSeconds(5);
        sr.enabled = true;
        cl.enabled = true;
    }
}
