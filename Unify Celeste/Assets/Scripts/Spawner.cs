using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] float spawnRate;
    [SerializeField] GameObject spawned;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InvokeRepeating("SpawnThing", 0, spawnRate);
    }

    void SpawnThing()
    {
        if(!spawned) return;
        Instantiate(spawned, transform.position, Quaternion.identity);
    }
}
