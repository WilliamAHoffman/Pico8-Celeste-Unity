
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    [SerializeField] Transform spawnLocation;
    [SerializeField] AudioClip level_music;

    
    [SerializeField] float levelYBounds;
    [SerializeField] string nextLevel;
    [SerializeField] GameObject levelStoragePrefab;
    [SerializeField] float playerUpSpeed = 10;
    private GameUIManager gameUIManager;
    public GameObject playerPrefab;
    private GameObject player;
    private bool firstTime;
    private bool playerMoveUp;
    private bool gameActive;
    AudioSource audioSource;

    private void Start()
    {
        firstTime = true;
        gameActive = true;
        audioSource = GetComponent<AudioSource>();
        if(!LevelStorage.instance)Instantiate(levelStoragePrefab, new Vector3(0,0,0), Quaternion.identity);
        if(SceneManager.GetActiveScene().buildIndex > 22)
        {
            LevelStorage.instance.canDoubleDash = true;
        }
        StartReload();
        if(level_music) LevelStorage.instance.PlaySong(level_music);
        gameUIManager = FindFirstObjectByType<GameUIManager>();
    }

    void FixedUpdate()
    {
        if (player && gameActive)
        {
            if (player.transform.position.y <= -levelYBounds)
            {
                player.GetComponent<PlayerController>().Die();
            }
            else if (player.transform.position.y >= levelYBounds)
            {
                SceneManager.LoadScene(nextLevel);
            }
            LevelStorage.instance.timeElapsed += Time.deltaTime;
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
        RestartObjects();

        if(!player) player = Instantiate(playerPrefab, new Vector3(0,0,0), Quaternion.identity);
        player.GetComponent<PlayerController>().unlockedDoubleDash = LevelStorage.instance.canDoubleDash;
        player.GetComponent<PlayerController>().ReloadPlayer();
        AllowMovement(false);
        player.transform.position = new Vector3(spawnLocation.position.x,-9);
        playerMoveUp = true;

        ScreenShake shake = Camera.main.GetComponent<ScreenShake>();
        if (shake && !firstTime) shake.Shake();
        firstTime = false;
        if (gameUIManager)
        {
            gameUIManager.ToggleUIvisible();
        }
            
        
        audioSource.PlayOneShot(Resources.Load<AudioClip>("StartLevel"));
    }

    private void AllowMovement(bool state)
    {
        player.GetComponent<PlayerController>().enabled = state;
        player.GetComponent<Rigidbody2D>().simulated = state;
        player.GetComponent<Collider2D>().enabled = state;
    }

    private void RestartObjects()
    {
        if(firstTime) return;
        foreach(FadingBlock block in FindObjectsByType<FadingBlock>(sortMode: FindObjectsSortMode.None))
        {
            block.StopAllCoroutines();
            block.gameObject.SetActive(true);
            block.ResetBlock();
        }

        foreach(FlyingStrawberry berry in FindObjectsByType<FlyingStrawberry>(sortMode: FindObjectsSortMode.None))
        {
            berry.gameObject.transform.position = berry.gameObject.GetComponent<LinearMovement>().startLocation;
            berry.gameObject.GetComponent<SinOscillator>().enabled = true;
            berry.flying = false;
        }

        foreach(Balloon balloon in FindObjectsByType<Balloon>(sortMode: FindObjectsSortMode.None))
        {
            balloon.Respawn();
        }

        foreach(LinearMovement obj in FindObjectsByType<LinearMovement>(sortMode: FindObjectsSortMode.None))
        {
            obj.gameObject.transform.position = obj.startLocation;
        }

        foreach(SmallChest chest in FindObjectsByType<SmallChest>(sortMode: FindObjectsSortMode.None))
        {
            chest.Restart();
        }
    }
}
