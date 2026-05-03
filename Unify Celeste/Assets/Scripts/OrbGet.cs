using UnityEngine;

public class OrbGet : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D c)
    {
        if (c.gameObject.GetComponent<PlayerController>())
        {
            c.gameObject.GetComponent<PlayerController>().unlockedDoubleDash = true;
            LevelStorage.instance.canDoubleDash = true;
            LevelStorage.instance.PlaySFX(Resources.Load<AudioClip>("PowerUp"));
            Destroy(gameObject);
        }
    }
}
