using Unity.VisualScripting;
using UnityEngine;

public class Background : MonoBehaviour
{

    [SerializeField] Color bg_color;
    [SerializeField] Color cloud_color;
    [SerializeField] GameObject cloud_prefab;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InvokeRepeating("SpawnCloud", .1f, .1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void SpawnCloud()
    {
        float minY= Camera.main.ViewportToWorldPoint(new Vector3(0,0,0)).y;
        float maxY = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0)).y;

        Instantiate(cloud_prefab, new Vector3(Camera.main.ViewportToWorldPoint(new Vector3(-.5f, 0, 0)).x, Random.Range(minY, maxY), 0), Quaternion.identity);
    }
}
