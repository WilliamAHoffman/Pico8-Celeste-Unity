using UnityEngine;

public class LevelStorage : MonoBehaviour
{
    public static LevelStorage instance;
    public int totalStrawberries;
    public float totalTime;
    private AudioSource audioSource;
    private AudioClip currSong;
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
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySong(AudioClip newSong)
    {
        if(newSong != currSong)
        {
            currSong = newSong;
            audioSource.Stop();
            audioSource.PlayOneShot(currSong);

        }
    }

}
