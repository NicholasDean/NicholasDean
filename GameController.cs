using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameController : MonoBehaviour
{
    public static GameController Instance { set; get; } //Setters and getters for game controller


    /* Fade Variables */ 
    private CanvasGroup fadeGroup; 
    private float fadeDuration = 2;
    private bool gameStarted;


    /* End Menu Variables */
    public GameObject endMenu;
    public Text currencyCollected;
    public Text currencyTotal;


    /* In Game Variables */
    public Material playerMaterial;
    public bool levelComplete = false;
    public GameObject[] gameLevels = new GameObject[3];
    private int i = Manager.Instance.currLevel;

    public GameObject trackController;


    private void Start()
    {
        //Grab only canvasGroup
        fadeGroup = FindObjectOfType<CanvasGroup>();
        fadeGroup.alpha = 1; //Fade from white to game scene

        if (i <= 1)
        {
            gameLevels[i].SetActive(true); //Sets the correct level active with the correct button pressed
        }
        else if (i == 2)
        {
            //Start the infinite level if button 3 is pressed
            trackController.SetActive(true);
        }



    }

    private void Update()
    {
        //Function to help fade without issues through loading the game scene
        if(Time.timeSinceLevelLoad <= fadeDuration)
        {
            //Init fade
            fadeGroup.alpha = 1 - (Time.timeSinceLevelLoad / fadeDuration);

        } else if (!gameStarted)
        {
            //Help return alpha to 0 with no issues
            fadeGroup.alpha = 0; 
            gameStarted = true;
        }

        if (i <= 1)
        {
            gameLevels[i].SetActive(true); //Sets the correct level active with the correct button pressed
        }
        else if (i == 2)
        {
            trackController.SetActive(true); //Start the infinite level if button 3 is pressed
        }
        Debug.Log(i);
    }

    public void CompleteLevel()
    {
        //Completed level, save
        SaveManager.Instance.CompleteLevel(Manager.Instance.currLevel);
        levelComplete = true;

        //Focus to menu scene
        Manager.Instance.menuFocus = 1;

        currencyCollected.text = "Collected: " + SaveManager.Instance.state.collectedCurrency.ToString(); //Currency collected within the level
        currencyTotal.text = SaveManager.Instance.state.currency.ToString(); //Total currency the user has overall
            
        EndMenu(); //Enables the end menu screen
    
        //ExitScene(); //Once game level is completed auto go to main menu again
    }

    public void ExitScene() //Called when exit button is pressed
    {
        SaveManager.Instance.state.collectedCurrency = 0; //Resets currency collected
        SaveManager.Instance.state.cpCollected = 0; //Resets CPs collected
        SceneManager.LoadScene("Menu"); //Go to the menu scene when called
    }

    public void EndMenu()
    {
        endMenu.SetActive(true); //Set End Screen to active
    }
}
