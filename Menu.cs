using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    /* Camera */
    private MenuCamera menuCamera;

    /* Control */
    public Button accelerometerOnOffButton;
    public Color accelerometerOn;
    public Color accelerometerOff;


    /*Fade Grouping*/
    private CanvasGroup fadeGroup;
    private float fadeInSpeed = 0.33f;

    /*Shop*/
    public Transform colourPanel;

    public GameObject insufficientFunds;
    private float wait = 0.5f;

    public Text colourBuySetText;
    public Text currencyText;

    private int[] colourCost = new int[] { 0, 10, 5, 15, 5, 15, 10, 400 };

    private int selectedColourIndex;
    private int activeColourIndex;

    /*Level Selection*/
    public RectTransform menuContainer;
    public Transform levelPanel;
    private Vector3 desiredMenuPos;
    public Text levelCompleted;

    /* Level Zoom */
    public AnimationCurve zoomLevelCurve;
    private bool isEnteringLevel = false;
    private float zoomDuration = 3.0f;
    private float zoomTransition;

    /*Settings*/
    public GameObject settingsPanel;
    public GameObject restartGame;

    private void Start()
    {
        settingsPanel.SetActive(false);

        //Check if device supports accelerometer movement
        if (SystemInfo.supportsAccelerometer)
        {
            //Gives users choice between accelerometer or touch movement
            accelerometerOnOffButton.GetComponent<Image>().color = (SaveManager.Instance.state.usingAccelerometer) ? accelerometerOn : accelerometerOff;

        } 
        else
        {
            //No accelerometer in the decive means only touch movement enabled.
            accelerometerOnOffButton.gameObject.SetActive(false);
        }


        //Find MenuCamera
        menuCamera = FindObjectOfType<MenuCamera>();
        
        //Setting Camera Position
        SetCameraTo(Manager.Instance.menuFocus);

        //Display currency value
        UpdateCurrencyText(); 

        /*Fade Grouping*/
        fadeGroup = FindObjectOfType<CanvasGroup>(); //Grab ONLY CanvasGroup in scene
        fadeGroup.alpha = 1; //Start the fade with white into the canvas

        /*Shop*/
        InitShop(); //Add buttion on-click events to the shop buttons

        /*Level Selection*/
        InitLevel(); //Add buttons on-click events to levels

        //Players Prefs for colour and trail
        OnColourSelect(SaveManager.Instance.state.activeColour);
        SetColour(SaveManager.Instance.state.activeColour);

        //Used to show which button the user is selecting
        colourPanel.GetChild(SaveManager.Instance.state.activeColour).GetComponent<RectTransform>().localScale = Vector3.one * 1.125f;

        colourBuySetText.text = "Purchase: " + colourCost[0].ToString();

        levelCompleted.text = "Levels Completed: " + SaveManager.Instance.state.completedLevels.ToString();


    }

    private void Update()
    {
        //Fade-in
        fadeGroup.alpha = 1 - Time.timeSinceLevelLoad * fadeInSpeed;

        //Navigation through panels
        menuContainer.anchoredPosition3D = Vector3.Lerp(menuContainer.anchoredPosition3D, desiredMenuPos, 0.1f);

        //Entering level zoom in
        if (isEnteringLevel)
        {
            zoomTransition += (1 / zoomDuration) * Time.deltaTime;

            //Change scale of zoom
            menuContainer.localScale = Vector3.Lerp(Vector3.one,Vector3.one * 5, zoomLevelCurve.Evaluate(zoomTransition));

            //Zoom in centre of button
            Vector3 newDesiredPosition = desiredMenuPos * -5;
            RectTransform rectT = levelPanel.GetChild(Manager.Instance.currLevel).GetComponent<RectTransform>();
            newDesiredPosition -= rectT.anchoredPosition3D * -5;

            //Override prev position update
            menuContainer.anchoredPosition3D = Vector3.Lerp(desiredMenuPos, newDesiredPosition, zoomLevelCurve.Evaluate(zoomTransition));

            //Fade white, override
            fadeGroup.alpha = zoomTransition;

            if(zoomTransition >= 1) //If fade done, enter level
            {
                SceneManager.LoadScene("Game");
            }
        }

        levelCompleted.text = "Levels Completed: " + SaveManager.Instance.state.completedLevels.ToString();

    }

    private void InitShop() //Initialise the shop
    {
        //We assigned references
        if (colourPanel == null)
        {
            Debug.Log("Colours Not assigned"); //Error Catcher
        }

        //For each child transform, find button and add onclick event
        int i = 0;
        foreach(Transform t in colourPanel)
        {
            int currIndex = i;
            Button b = t.GetComponent<Button>();
            b.onClick.AddListener(() => OnColourSelect(currIndex));

            //Set colour of image, to show if owned or not
            Image img = t.GetComponent<Image>();
            //Check if colour is owned, if yes set the colour of the player ball
            img.color = SaveManager.Instance.IsColourOwned(i) ? Manager.Instance.playerColours[currIndex] : Color.Lerp(Manager.Instance.playerColours[currIndex], new Color(0,0,0,1),0.25f);


            i++;

        }

    }

    private void InitLevel() //Initialise the level select
    {
        //We assigned references
        if (levelPanel == null)
        {
            Debug.Log("Level Panel Not assigned"); //Error Catcher
        }

        //For each child transform, find button and add onclick event
        int i = 0;
        foreach (Transform t in levelPanel)
        {
            int currIndex = i;
            Button button = t.GetComponent<Button>();
            button.onClick.AddListener(() => OnLevelSelect(currIndex)); //When clicked go to function OnLevelSelect

            Image img = t.GetComponent<Image>(); //Allows to change colour of level select buttons
            //Is level unlocked
            if (i <= SaveManager.Instance.state.completedLevels)
            {
                if(i == SaveManager.Instance.state.completedLevels)
                {
                    img.color = Color.white; //Level not complete
                } else
                {
                    img.color = Color.green; //Level complete
                }
            } else
            {
                //Level not unlocked yet, user not allowed to select
                button.interactable = false;
                img.color = Color.grey;
            }

            i++;
        }
    }

    /* Navigation */
    private void NavigationTo(int menuIndex)
    {
        switch(menuIndex) //Switch camera to different panels
        {
            //default and 0th case is Main Menu
            default:
            case 0:
                desiredMenuPos = Vector3.zero; //If we want main menu set camera in middle
                menuCamera.ToMainMenu();
                break;
            // Case 1 is Level Menu
            case 1:
                desiredMenuPos = Vector3.right * 2960; //If we want level menu go right size of screen
                menuCamera.ToLevel();
                break;
            // Case 2 is Shop Menu
            case 2:
                desiredMenuPos = Vector3.left * 2960; //If we want shop menu go left size of screen
                menuCamera.ToShop();
                break;
        }
    }

    private void SetCameraTo(int menuIndex)
    {

        //Setting cameras position
        NavigationTo(menuIndex);
        menuContainer.anchoredPosition3D = desiredMenuPos;
    }

    /* Shop declarations */
    private void SetColour(int index)
    {

        //Set active index 
        activeColourIndex = index;
        SaveManager.Instance.state.activeColour = index;

        //Change colour of player model to selected colour
        Manager.Instance.playerMaterial.color = Manager.Instance.playerColours[index];
        

        //Change button text from buy to current
        colourBuySetText.text = "Applied";

        SaveManager.Instance.Save(); //Save each time this function is run
    }

    private void UpdateCurrencyText()
    {
        currencyText.text = SaveManager.Instance.state.currency.ToString(); //Grab gold value from SaveManager
    }

    /* #### Buttons Selection/OnClicks #### */

    /* Level Select Buttons */
    public void OnPlayCLick()
    {
        NavigationTo(1); //Go to level menu
    }

    private void OnLevelSelect(int currIndex)
    {
        Manager.Instance.currLevel = currIndex; //Check the current level to load into
        isEnteringLevel = true; //Load into level
    }

    public void OnBackClick() 
    {
        NavigationTo(0); //Go back to main menu
    }


    /* Shop Buttons */
    public void OnShopCLick()
    {
        NavigationTo(2); //Go to shop menu navigation
    }

    /* Colour Buy */
    private void OnColourSelect(int currIndex)
    { 

        //If the button user is trying to select is already selected, ignore
        if(selectedColourIndex == currIndex)
        {
            return;
        }
        //Increase size of button user is selecting
        colourPanel.GetChild(currIndex).GetComponent<RectTransform>().localScale = Vector3.one * 1.125f;
        //Resize prev selected button
        colourPanel.GetChild(selectedColourIndex).GetComponent<RectTransform>().localScale = Vector3.one;

        //Set selected colour
        selectedColourIndex = currIndex;

        // Change text of button depending on state
        if(SaveManager.Instance.IsColourOwned(currIndex))
        {
            
            if (activeColourIndex == currIndex)
            {
                //Colour owned and equipped
                colourBuySetText.text = "Applied";
            } 
            else
            {
                //Colour owned and not equipped
                colourBuySetText.text = "Purchase: " + colourCost[currIndex].ToString();
            }
            
        } else
        {
            //Colour not owned 
            colourBuySetText.text = "Purchase: " + colourCost[currIndex].ToString();
        }
    }

    public void OnColourBuy()
    {
        //Is selected colour owned = set colour
        if (SaveManager.Instance.IsColourOwned(selectedColourIndex))
        {
            SetColour(selectedColourIndex);
        }
        else
        {
            //Attempt to buy colour
            if (SaveManager.Instance.BuyColour(selectedColourIndex, colourCost[selectedColourIndex]))
            {
                //Attempt to buy colour successful
                SetColour(selectedColourIndex);
                //Changing button colour when bought
                colourPanel.GetChild(selectedColourIndex).GetComponent<Image>().color = Manager.Instance.playerColours[selectedColourIndex];
                UpdateCurrencyText();
            }
            else
            {
                //Attempt to buy colour unsuccessful
                insufficientFunds.SetActive(true);
                StartCoroutine(RemoveAfterSeconds(wait, insufficientFunds));
            }
        }
    }

    /* Controls */
    public void OnAccelerometerControl()
    {
        //Toggling accelerometer
        SaveManager.Instance.state.usingAccelerometer = !SaveManager.Instance.state.usingAccelerometer;
        SaveManager.Instance.Save(); //Saving that user has accelerometer within device, stops checking every time

        //Change colour of button
        accelerometerOnOffButton.GetComponent<Image>().color = (SaveManager.Instance.state.usingAccelerometer) ? accelerometerOn : accelerometerOff;

    }

    public void OnSettingsButton()
    {
        settingsPanel.SetActive(true); //Sets setting panel active
    }

    public void OnBackSettingsButton()
    {
        settingsPanel.SetActive(false); //sets setting panel false
        
    }

    public void OnDeleteSave()
    {
        PlayerPrefs.DeleteKey("save"); //allows users to reset their own save
        restartGame.SetActive(true);
        StartCoroutine(RemoveAfterSeconds(wait, restartGame));
    }

    IEnumerator RemoveAfterSeconds(float seconds, GameObject obj) //Timer to set active the insufficient funds for set time
    {
        yield return new WaitForSeconds(wait);
        obj.SetActive(false);
    }

  



    

}
