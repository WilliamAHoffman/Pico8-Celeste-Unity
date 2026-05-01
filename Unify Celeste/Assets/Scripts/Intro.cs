using System.Collections;
using System.Diagnostics.CodeAnalysis;
using TMPro;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class Intro : MonoBehaviour
{
    [SerializeField] InputActionAsset inputActions;
   

    [SerializeField] AudioClip introMusic;
    [SerializeField] AudioClip startSFX;
    [SerializeField] GameObject textObj;
    [SerializeField] GameObject logoObj;

    [SerializeField] Color c1;
    [SerializeField] Color c2;  
    [SerializeField] Color c3;
    [SerializeField] Color c4;

    [SerializeField] Color clear;


    float dur = .2f;
 
  

    AudioSource ap;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ap= GetComponent<AudioSource>();
        ap.clip = introMusic;
        ap.Play();
        Invoke("StartGame", 3);
       
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            StartGame();
        }

    }


    void StartGame()
    {
        ap.clip= startSFX;
        ap.Play();
        ap.loop = false;
        StartCoroutine("FlashColors");
     


    }


    void ChangeLogoColor(Color c)
    {
        
        if (c == c2)
        {
            logoObj.GetComponent<Tilemap>().color = clear ;
        }
        else
        {
            logoObj.GetComponent<Tilemap>().color = c;
         
        }
        textObj.GetComponent<Tilemap>().color = c;
    }

 

    IEnumerator FlashColors()
    {
        
        ChangeLogoColor(c1);
        yield return new WaitForSeconds(dur);

        ChangeLogoColor(c2);
        yield return new WaitForSeconds(dur);
       
        ChangeLogoColor(c1);
        yield return new WaitForSeconds(dur);

        ChangeLogoColor(c2);
        yield return new WaitForSeconds(dur);

        ChangeLogoColor(c1);
        yield return new WaitForSeconds(dur);
       
        ChangeLogoColor(c2);
        yield return new WaitForSeconds(dur);

        ChangeLogoColor(c1);
        yield return new WaitForSeconds(dur);

        ChangeLogoColor(c2);
        yield return new WaitForSeconds(dur);

        ChangeLogoColor(c1);
        yield return new WaitForSeconds(dur);


        ChangeLogoColor(c3);
        yield return new WaitForSeconds(dur);


        ChangeLogoColor(c4);
        yield return new WaitForSeconds(dur);

        SceneManager.LoadScene("Level 1");


    }

}
