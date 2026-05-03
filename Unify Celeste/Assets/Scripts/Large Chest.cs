using System.Collections;
using UnityEngine;

public class LargeChest : MonoBehaviour
{
    public GameObject greenOrbPrefab;
    SpriteRenderer[] sr;
    private bool chestOpened = false;

    [SerializeField] float speed;
    public GameObject chestLid;

    AudioSource ap;
    void Start()
    {
        sr = chestLid.GetComponentsInChildren<SpriteRenderer>();
        ap = gameObject.AddComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D c)
    {
        if (c.gameObject.GetComponent<PlayerController>() && !chestOpened)
        {
            StartCoroutine(Orb());
            ap.PlayOneShot(Resources.Load<AudioClip>("BigChestOpen"));
            chestOpened = true;
            foreach (var s in sr)
            {
                s.enabled = false;
            }
        }
    }

    IEnumerator Orb()
    {
        yield return new WaitForSeconds(1.5f);
        GameObject orb = Instantiate(greenOrbPrefab, transform.position, Quaternion.identity);
        orb.GetComponent<Collider2D>().enabled = false;
        StartCoroutine(StopMovement(orb.transform));
    }

    IEnumerator StopMovement(Transform o)
    {
        float timer = 1.5f;
        float timeElapsed = 0;

        Vector3 start = o.position;
        Vector3 end = start + Vector3.up * 2f;

        while (timeElapsed < timer)
        {
            o.position = Vector3.Lerp(start, end, timeElapsed / timer);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        o.position = end;
        o.gameObject.GetComponent<Collider2D>().enabled = true;
    }
}
