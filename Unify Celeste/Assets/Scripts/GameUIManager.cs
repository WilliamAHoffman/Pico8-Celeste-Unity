using System;
using TMPro;
using UnityEngine;

public class GameUIManager : MonoBehaviour
{


    [SerializeField] String height;
    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] GameObject ui_popup;
    [SerializeField] TextMeshProUGUI heightText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
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

    

   
}
