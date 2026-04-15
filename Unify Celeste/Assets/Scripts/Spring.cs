using Unity.VisualScripting;
using UnityEngine;

public class Spring : MonoBehaviour
{

    bool active=true;
    [SerializeField] GameObject fade_block;
    [SerializeField] Sprite pressed_sprite;
    [SerializeField] Sprite normal_sprite;
   

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>() != null&& active)
        {
            //launch the player up here


            active = false;
            GetComponent<SpriteRenderer>().sprite = pressed_sprite;
            if (fade_block != null)
            {
                fade_block.GetComponent<Animator>().Play("FadingBlockAnim");

            }
            else
            {
                Invoke("ResetSpring", .5f);
            }
        }
    }

    public void ResetSpring()
    {
        gameObject.SetActive(true);
        active = true;
        GetComponent <SpriteRenderer>().sprite = normal_sprite;
        
    }
}
