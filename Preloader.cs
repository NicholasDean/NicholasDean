using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Preloader : MonoBehaviour
{
    private CanvasGroup fadeGroup;
    private float loadTime;
    private float minLogoTime = 1.5f; //Show logo to help load the game min amount of time logo will be shown

    private void Start()
    {
        //Grab ONLY CanvasGroup in scene
        fadeGroup = FindObjectOfType<CanvasGroup>();

        //Start with fade from white to scene
        fadeGroup.alpha = 1;

        if (Time.time < minLogoTime) //If the game takes short to load than the logo time, just play logo normally, if not take all the load time needed
            loadTime = minLogoTime;
        else
            loadTime = Time.time;

    }

    private void Update()
    {
        //Fade in
        if (Time.time < minLogoTime)
            fadeGroup.alpha = 1 - Time.time;

        //Fade out
        if (Time.time > minLogoTime && loadTime != 0)
        {
            fadeGroup.alpha = Time.time - minLogoTime;
            if (fadeGroup.alpha >= 1)
            {
                SceneManager.LoadScene("Menu");
            }
        }
    }



}
