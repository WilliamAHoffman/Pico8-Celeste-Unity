using Unity.VisualScripting;
using UnityEngine;

public class Spring : MonoBehaviour
{

    bool active=true;
    AudioSource ap;
    [SerializeField] GameObject fade_block;
    [SerializeField] Sprite pressed_sprite;
    [SerializeField] Sprite normal_sprite;
    [SerializeField] float strength;
   

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ap = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>() && active)
        {
            Rigidbody2D playerRB = collision.gameObject.GetComponent<Rigidbody2D>();
            //launch the player up here

            playerRB.linearVelocity = new Vector2(playerRB.linearVelocity.x, strength);
            active = false;
            GetComponent<SpriteRenderer>().sprite = pressed_sprite;
            if (fade_block != null)
            {
                fade_block.GetComponent<Animator>().Play("FadingBlockAnim");
            }
            else
            {
                Invoke("ResetSpring", .25f);
            }
        }
    }

    public void ResetSpring()
    {
        gameObject.SetActive(true);
        active = true;
        GetComponent <SpriteRenderer>().sprite = normal_sprite;
        ap.PlayOneShot(Resources.Load<AudioClip>("Spring"));

    }
}
