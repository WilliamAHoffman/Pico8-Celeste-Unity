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
        for (int i=0; i<15; i++)
        {
            float minY = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).y;
            float maxY = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0)).y;
            float minX = Camera.main.ViewportToWorldPoint(new Vector3(-.5f, 0, 0)).x;
            float maxX = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0)).x;
            Instantiate(cloud_prefab, new Vector3(Random.Range(minX,maxX), Random.Range(minY, maxY), 0), Quaternion.identity);

        }
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
