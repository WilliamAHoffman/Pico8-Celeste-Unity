
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;




public class GameManager : MonoBehaviour
{


    public static GameManager instance;
    public PlayerController playerController;
    public List<string> levels;
    [SerializeField] AudioClip level_music1;
    [SerializeField] float levelYBounds;
    public int strawberryCounter = 0;
    AudioSource music_player;
    private GameObject player;
    private bool gameActive;

    void Awake()
    {
        if (instance)
        {
            if (instance != this)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(gameObject);
        music_player = GetComponent<AudioSource>();
    }

    private void Start()
    {
        if (!music_player.isPlaying)
        {
            PlaySong(level_music1);
        }
    }

    private void PlaySong(AudioClip song)
    {
        music_player.clip = song;
        music_player.Play();
    }

    void FixedUpdate()
    {
        if (player && gameActive)
        {
            if (player.transform.position.y <= -levelYBounds)
            {
                StartCoroutine("ReloadScene");
            }
            else if (player.transform.position.y >= levelYBounds)
            {
                if (levels.Count > 0)
                {
                    string nextLevel = levels[0];
                    levels.RemoveAt(0);
                    SceneManager.LoadScene(nextLevel);
                }
            }
        }
        else
        {
            player = FindFirstObjectByType<PlayerController>().gameObject;
            gameActive = true;
        }
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
