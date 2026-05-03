using UnityEngine;

public class Strawberry : MonoBehaviour
{

    [SerializeField] GameObject scoreUI;
    private void OnTriggerEnter2D(Collider2D c)
    {
        if (c.gameObject.GetComponent<PlayerController>())
        {
            if(LevelStorage.instance) LevelStorage.instance.totalStrawberries++;
            if (LevelStorage.instance) LevelStorage.instance.PlaySFX(Resources.Load<AudioClip>("CollectStrawberry"));
            c.gameObject.GetComponent<PlayerController>().isAbleToDash = true;
            if(c.gameObject.GetComponent<PlayerController>().unlockedDoubleDash) c.gameObject.GetComponent<PlayerController>().isAbleToDoubleDash = true;
            Instantiate(scoreUI, transform.position, Quaternion.identity);
            Destroy(gameObject);

            
        }
    }
}
