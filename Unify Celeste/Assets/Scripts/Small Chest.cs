using System.Collections;
using UnityEngine;
public class SmallChest : MonoBehaviour
{
   
    public GameObject strawberryPrefab;
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
            StartCoroutine(SpawnStrawberry());
            chestOpened = true;
        }
    }

    IEnumerator SpawnStrawberry()
    {

        ap.PlayOneShot(Resources.Load<AudioClip>("OpenChest"));
        yield return new WaitForSeconds(1.5f);
        sr.enabled = false;
        Instantiate(strawberryPrefab, transform.position + Vector3.up, Quaternion.identity);
    }
}
