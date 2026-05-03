using System.Collections;
using UnityEngine;
public class SmallChest : MonoBehaviour
{
   
    public GameObject strawberryPrefab;
    private GameObject strawberry;
    SpriteRenderer sr;
    private bool chestOpened = false;
    AudioSource ap;
   
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        ap = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Key.keyCollected && !chestOpened)
        {
            SpawnStrawberry();
            chestOpened = true;
        }
    }

    void SpawnStrawberry()
    {

        ap.PlayOneShot(Resources.Load<AudioClip>("OpenChest"));
        sr.enabled = false;
        strawberry = Instantiate(strawberryPrefab, transform.position + Vector3.up, Quaternion.identity);
    }

    public void Restart()
    {
        if(!strawberry) return;
        Destroy(strawberry);
        sr.enabled = true;
        chestOpened = false;
        FindFirstObjectByType<Key>().Reset();
    }
}
