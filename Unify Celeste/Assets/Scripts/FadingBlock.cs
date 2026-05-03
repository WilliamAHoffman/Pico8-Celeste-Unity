using System.Collections;
using UnityEngine;

public class FadingBlock : MonoBehaviour
{
    private Animator anim;
    private AudioSource audioSource;
    private Collider2D col;
    private SpriteRenderer spriteRenderer;

    [SerializeField] private GameObject spring;
    [SerializeField] private ParticleSystem particles;

    private bool isReady = true;

    private void Start()
    {
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        col = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isReady) return;

        anim.Play("FadingBlockAnim");

        AudioClip clip = Resources.Load<AudioClip>("FadeBlock");
        if (clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    public void Fade()
    {
        if (!isReady) return;

        isReady = false;

        if (spring != null)
        {
            spring.SetActive(false);
        }

        if (col != null)
        {
            col.enabled = false;
        }

        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = false;
        }

        StartCoroutine(ResetBlockAfterDelay(3f));
    }

    private IEnumerator ResetBlockAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ResetBlock();
    }

    public void ResetBlock()
    {
        if(isReady) return;
        isReady = true;

        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = true;
        }

        if (col != null)
        {
            col.enabled = true;
        }

        if (particles != null)
        {
            particles.Emit(1);
        }

        anim.Play("Idle");

        AudioClip clip = Resources.Load<AudioClip>("FadeBlockRegen");
        if (clip != null)
        {
            audioSource.PlayOneShot(clip, 0.5f);
        }

        if (spring != null)
        {
            spring.SetActive(true);

            Spring springScript = spring.GetComponent<Spring>();
            if (springScript != null)
            {
                springScript.ResetSpring();
            }
        }
    }
}