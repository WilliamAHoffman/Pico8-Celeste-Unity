using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

public class BackgroundClouds : MonoBehaviour
{
    private float speed;
    private void Awake()
    {
        transform.localScale = new Vector3(Random.Range(2f, 4.5f), Random.Range(.25f, .8f), 0);
        speed = Random.Range(5f, 10f);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Invoke("RemoveCloud", 5);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);

        if (Camera.main.WorldToViewportPoint(transform.position).x > 1.4)
        {
            Destroy(gameObject);
        }
    }

    void RemoveCloud()
    {
        Destroy(gameObject);
    }
}

