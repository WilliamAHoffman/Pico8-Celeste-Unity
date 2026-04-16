using System;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;



public class GameManager : MonoBehaviour
{

    
    public static GameManager instance;
    public PlayerController playerController;
    public List<string> levels;
    [SerializeField] AudioClip level_music1;
    AudioSource music_player;

    void Awake()
    {
        if (instance)
        {
            if(instance != this)
            {
               Destroy(this); 
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

    public void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject);
        if (collision.gameObject.GetComponent<PlayerController>())
        {
            if(levels.Count > 0)
            {
                string nextLevel = levels[0];
                levels.RemoveAt(0);
                SceneManager.LoadScene(nextLevel);
            }
        }
    }

    private void PlaySong(AudioClip song)
    {
        music_player.clip = song;
        music_player.Play();
    }
}
