using System.Collections;
using UnityEngine;

public class Balloon : MonoBehaviour
{
    private SpriteRenderer sr;
    [SerializeField] SpriteRenderer sr2;
    [SerializeField] ParticleSystem particles;
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
            if(c.gameObject.GetComponent<PlayerController>().unlockedDoubleDash) c.gameObject.GetComponent<PlayerController>().isAbleToDoubleDash = true;
            audioSource.PlayOneShot(Resources.Load<AudioClip>("BallonCollect"));
            sr.enabled = false;
            cl.enabled = false;
            sr2.enabled = false;
            StartCoroutine(RespawnBalloon());
        }
    }

    IEnumerator RespawnBalloon()
    {
        yield return new WaitForSeconds(5);
        Respawn();
    }

    public void Respawn()
    {
        sr.enabled = true;
        cl.enabled = true;
        sr2.enabled = true;
        if (particles)
        {
            particles.Emit(1);
        }
        audioSource.PlayOneShot(Resources.Load<AudioClip>("BalloonRegen"));
    }
    
}