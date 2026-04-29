using UnityEditor.Callbacks;
using UnityEngine;

public class FlyingStrawberry : MonoBehaviour
{
    [SerializeField] float speed;
    private PlayerController player;
    private bool flying = false;

    void Update()
    {
        if (!player)
        {
            player = FindFirstObjectByType<PlayerController>();
        }
        else if (player.isDashing && !flying)
        {
            flying = true;
            GetComponent<LinearMovement>().ySpeed = speed;
        }
    }
        
    private void OnTriggerEnter2D(Collider2D c)
    {
        if (c.gameObject.GetComponent<PlayerController>())
        {
            if(LevelStorage.instance) LevelStorage.instance.totalStrawberries++;
            c.gameObject.GetComponent<PlayerController>().isAbleToDash = true;
            Destroy(gameObject);
        }
    }
}
