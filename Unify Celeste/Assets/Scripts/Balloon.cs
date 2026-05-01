using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class Balloon : MonoBehaviour
{
    private SpriteRenderer sr;
    private Collider2D cl;
    AudioSource audioSource;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        cl = GetComponent<Collider2D>();
        audioSource = GetComponent<AudioSource>();
    }
    
    private void OnTriggerEnter2D(Collider2D c)
    {
        if (c.gameObject.GetComponent<PlayerController>())
        {
            c.gameObject.GetComponent<PlayerController>().isAbleToDash = true;
            audioSource.PlayOneShot(Resources.Load<AudioClip>("BallonCollect"));
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
        audioSource.PlayOneShot(Resources.Load<AudioClip>("BalloonRegen"));
    }
}