using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class SmallChest : MonoBehaviour
{
   
    public GameObject strawberryPrefab;
    SpriteRenderer sr;
    private bool chestOpened = false;
   
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
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
        yield return new WaitForSeconds(1.5f);
        sr.enabled = false;
        Instantiate(strawberryPrefab, transform.position, Quaternion.identity);
    }
}
