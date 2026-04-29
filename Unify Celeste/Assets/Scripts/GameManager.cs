
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using Unity.Mathematics;

public class GameManager : MonoBehaviour
{
    [SerializeField] Transform spawnLocation;
    [SerializeField] AudioClip level_music;
    [SerializeField] float levelYBounds;
    [SerializeField] string nextLevel;
    [SerializeField] GameObject levelStoragePrefab;
    [SerializeField] float playerUpSpeed;
    public GameObject playerPrefab;
    private GameObject player;
    private bool firstTime;
    private bool playerMoveUp;
    private bool gameActive;

    private void Start()
    {
        firstTime = true;
        gameActive = true;
        StartReload();
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

        if (playerMoveUp)
        {
            float newY = player.transform.position.y + Time.deltaTime * playerUpSpeed;
            player.transform.position = new Vector3(spawnLocation.position.x, newY, 0);
            if(newY >= spawnLocation.position.y)
            {
                ResetRoom();
            }
        }
    }

    void ResetRoom()
    {
        gameActive = true;
        playerMoveUp = false;
        AllowMovement(true);
    }

    public void StartReload()
    {
        if (!gameActive) return;

        gameActive = false;

        if(!player) player = Instantiate(playerPrefab, new Vector3(0,0,0), Quaternion.identity);
        player.GetComponent<PlayerController>().ReloadPlayer();
        AllowMovement(false);
        player.transform.position = new Vector3(spawnLocation.position.x,-9);
        playerMoveUp = true;

        ScreenShake shake = Camera.main.GetComponent<ScreenShake>();
        if (shake && !firstTime) shake.Shake();
        firstTime = false;
    }

    private void AllowMovement(bool state)
    {
        player.GetComponent<PlayerController>().enabled = state;
        player.GetComponent<Rigidbody2D>().simulated = state;
        player.GetComponent<Collider2D>().enabled = state;
    }
}
