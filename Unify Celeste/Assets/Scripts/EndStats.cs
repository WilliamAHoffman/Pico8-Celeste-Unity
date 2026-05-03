using System;
using TMPro;
using UnityEngine;

public class EndStats : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] TextMeshProUGUI deathText;
    [SerializeField] TextMeshProUGUI strawberryText;
    [SerializeField] GameObject statPanel;

    


    int timeTaken;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        statPanel.SetActive(false);
        timeTaken = (int)LevelStorage.instance.timeElapsed;
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerController>() != null)
        {
            ToggleStats(true);

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerController>() != null)
        {
            ToggleStats(false);

        }
    }


    public void ToggleStats(bool showing)
    {
        statPanel.SetActive(showing);
        strawberryText.text = "x" + LevelStorage.instance.totalStrawberries;
        deathText.text= "deaths:"+ LevelStorage.instance.numDeaths;
        TimeSpan time = TimeSpan.FromSeconds(LevelStorage.instance.timeElapsed);
        timeText.text = $"{(int)time.TotalHours:00}:{time.Minutes:00}:{time.Seconds:00}";
        
       
        
    }
     
}
