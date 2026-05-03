using Unity.VisualScripting;
using UnityEditor.Callbacks;
using UnityEngine;

public class FlyingStrawberry : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] GameObject scoreUI;
    private PlayerController player;
    private bool flying = false;
    bool justStarted=true;
    

    AudioSource audioSource;


    private void Start()
    {
        
        audioSource = GetComponent<AudioSource>();
        
    }
    void Update()
    {
        if (!player)
        {
            player = FindFirstObjectByType<PlayerController>();
        }
        else if (player.isDashing && !flying)
        {
            flying = true;
            if (justStarted)
            {
                justStarted = false;
                audioSource.PlayOneShot(Resources.Load<AudioClip>("StrawberryFlee"));
            }
            GetComponent<LinearMovement>().ySpeed = speed;
        }
    }
        
    
    private void OnTriggerEnter2D(Collider2D c)
    {
        if (c.gameObject.GetComponent<PlayerController>())
        {
            if(LevelStorage.instance) LevelStorage.instance.totalStrawberries++;
            if (LevelStorage.instance) LevelStorage.instance.PlaySFX(Resources.Load<AudioClip>("CollectStrawberry"));
            //c.gameObject.GetComponent<PlayerController>().isAbleToDash = true;
            Instantiate(scoreUI, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
