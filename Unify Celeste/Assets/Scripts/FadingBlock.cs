using UnityEngine;

public class FadingBlock : MonoBehaviour
{

    Animator anim;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim= GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        anim.Play("FadingBlockAnim");
    }

    public void Fade()
    {
        gameObject.SetActive(false);
        Invoke("ResetBlock", 3);
    }

    private void ResetBlock()
    {
        
        gameObject.SetActive(true);
        anim.Play("Idle");
    }
}
