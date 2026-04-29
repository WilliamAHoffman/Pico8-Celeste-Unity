using UnityEngine;

public class Strawberry : MonoBehaviour
{
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
