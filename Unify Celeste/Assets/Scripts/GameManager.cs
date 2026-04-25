
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using Unity.Mathematics;

public class GameManager : MonoBehaviour
{
    [SerializeField] Vector2 spawnLocation;
    [SerializeField] AudioClip level_music;
    [SerializeField] float levelYBounds;
    [SerializeField] string nextLevel;
    [SerializeField] GameObject levelStoragePrefab;
    public GameObject playerPrefab;
    private GameObject player;
    private bool gameActive;

    private void Start()
    {
        ResetRoom();
        if(!LevelStorage.instance) Instantiate(levelStoragePrefab, new Vector3(0,0,0), Quaternion.identity);
        if(level_music) LevelStorage.instance.PlaySong(level_music);
    }

    void FixedUpdate()
    {
        if (player && gameActive)
        {
            if (player.transform.position.y <= -levelYBounds)
            {
                StartCoroutine("StartReload");
            }
            else if (player.transform.position.y >= levelYBounds)
            {
                SceneManager.LoadScene(nextLevel);
            }
        }
    }

    void ResetRoom()
    {
        if(!player) player = Instantiate(playerPrefab, new Vector3(0,0,0), Quaternion.identity);
        player.transform.position = spawnLocation;
        gameActive = true;
        player.SetActive(true);
    }

    public void StartReload()
    {
        if (!gameActive) return;

        gameActive = false;
        ScreenShake shake = Camera.main.GetComponent<ScreenShake>();
        if (shake) shake.Shake();

        StartCoroutine("ReloadScene");
        player.SetActive(false);
    }

    IEnumerator ReloadScene()
    {
        yield return new WaitForSeconds(1f);
        Debug.Log("reset");
        ResetRoom();
    }

}
