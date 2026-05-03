using UnityEngine;

public class CollectBerry : MonoBehaviour
{


    [SerializeField] Color white;
    [SerializeField] Color red;
    [SerializeField] float speed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Invoke("Des", 1f);
        InvokeRepeating("ChangeColor", .05f, .05f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }

    private void Des()
    {
        Destroy(gameObject);
    }

    private void ChangeColor()
    {
        if (GetComponent<SpriteRenderer>().color == white)
        {
            GetComponent<SpriteRenderer>().color = red;
        }
        else
        {
            GetComponent<SpriteRenderer>().color = white;
        }
    }
}
