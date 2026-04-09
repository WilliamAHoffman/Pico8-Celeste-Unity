using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class Balloon : MonoBehaviour
{
    public PlayerController pc;

    void Start()
    {
        pc = FindFirstObjectByType<PlayerController>();
        SRPLensFlareBlendMode 
    }
    
    private void OnTriggerEnter2D(Collider2D c)
    {
        if (c.gameObject.CompareTag("Player"))
        {
            pc.isAbleToDash = true;
            StartCoroutine(RespawnBalloon());
            gameObject.SetActive(false);
        }
    }

    IEnumerator RespawnBalloon()
    {
        yield return new WaitForSeconds(5);
        gameObject.SetActive(true);
    }
}
