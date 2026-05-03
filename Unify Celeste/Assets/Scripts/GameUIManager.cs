using Mono.Cecil;
using System;
using System.Diagnostics.CodeAnalysis;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class GameUIManager : MonoBehaviour
{


    [SerializeField] String height;
    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] GameObject ui_popup;
    [SerializeField] TextMeshProUGUI heightText;

    [SerializeField] GameObject pauseMenu; //menu for the pause menu



    [SerializeField] GameObject optionsMenu; //menu for the options menu
    [SerializeField] TextMeshProUGUI soundText;
    [SerializeField] TextMeshProUGUI volInidicators;

    [SerializeField] GameObject controlsMenu; //menu for the controls





    public string menuOpen = "";

    public float vol;
    public bool soundEnabled;

    string menuAction;
    void Start()
    {
        ToggleUIvisible();
        heightText.text = height;
      
    }

    // Update is called once per frame
    void Update()
    {
        TimeSpan time = TimeSpan.FromSeconds(LevelStorage.instance.timeElapsed);
        timeText.text = time.ToString(@"hh\:mm\:ss");
        //timeText.text = (Mathf.FloorToInt(LevelStorage.instance.timeElapsed / 3600) + ":" + Mathf.FloorToInt((LevelStorage.instance.timeElapsed % 3600) / 60) + ":" + Mathf.FloorToInt(LevelStorage.instance.timeElapsed % 60));


        //if you press enter, then the game should pause. If you press enter when the menu is open, then whatever u are over should be clicked. UNLESS ur in the control menu, and then the game should unpause.
        //pressing x or c or any of the keys i believe will also select whatever u are over in the menu

        //if you press up and down, the 

    }

  


    public void ToggleUIvisible()
    {
        ui_popup.SetActive(true);
        Invoke("ToggleUIOff", 1);
    }

    public void ToggleUIOff()
    {
        ui_popup.SetActive(false);
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        foreach(var ps in FindObjectsOfType<ParticleSystem>())
        {
            ps.Pause();
        }
        AudioListener.pause = true;
        pauseMenu.SetActive(true);
        menuOpen = "pause";

        // having menu open needs to disable the player movement
        

    }

    public void UnPause()
    {
        Time.timeScale = 1f;
        foreach (var ps in FindObjectsOfType<ParticleSystem>())
        {
            ps.Play();
        }
        AudioListener.pause = false;
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(false);
        controlsMenu.SetActive(false);
        //need to enable player movmeent
    }


    public void ToggleSound()
    {
        // there should be icons for the volume level that are changed when u toggle sound on and off
        soundEnabled = !soundEnabled;
        if (soundEnabled)
        {
            soundText.text = "sound:on";
        }
        else
        {
            soundText.text = "sound:off";
        }
    }

    //for changing the volume, i believe you can change it by setting the AudioListener volume.

    public void OpenControls()
    {
        optionsMenu.SetActive(false);
        controlsMenu.SetActive(true);
        pauseMenu.SetActive(false);
        menuOpen = "controls";

    }

    public void OpenOptions()
    {
        optionsMenu.SetActive(true);
        pauseMenu.SetActive(false);
        controlsMenu.SetActive(false);
        menuOpen = "options";
    }

}
