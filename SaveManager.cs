using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { set; get; } //Setter and Getters of the save manager to access 
    public SaveState state;


    /* Awake state, Save state and load state */

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Instance = this; //Creates the instance 
        Load(); //Load the save file

        //Accelerometer being used and can the device support it?
        if(state.usingAccelerometer && !SystemInfo.supportsAccelerometer)
        {
            //If not, save 
            state.usingAccelerometer = false;
            Save();
        }

    }

    /* Ecryption and Decryption */

    //This is used to make the save data for each user more secure than no encryption

    //Save state of SaveState script to PlayerPrefrences
    public void Save()
    {
        PlayerPrefs.SetString("save",Helper.Encrypt(Helper.Serialize<SaveState>(state))); //Encrypts the save data  

    }

    //Load state of SaveState script to PlayerPrefrences
    public void Load()
    {
        //Is there already a save set
        if (PlayerPrefs.HasKey("save"))
        {
            state = Helper.Deserialize<SaveState>(Helper.Decrypt(PlayerPrefs.GetString("save"))); //Decrypts the save data on load,

        }
        else //If no save set, set state to save
        {
            state = new SaveState(); //Create new save 
            Save();
            Debug.Log("No save found, creating new save");
        }
    }


    /* Colours Owned */

    //Is the colour owned
    public bool IsColourOwned(int index)
    {
        //Check through bits set to see which colours owned
        return (state.colourOwned & (1 << index)) != 0;
    }

    //Unlock new colour
    public void UnlockColour(int index)
    {
        //Set the bit for the new colour owned 
        state.colourOwned |= 1 << index; //To toggle off/delete a colour owned ^= 1
    }

    //Does user have enough coins to buy colour
    public bool BuyColour(int index, int cost)
    {
        if (state.currency >= cost) //Check cost vs user currency count
        {
            state.currency -= cost; //Enough currency, remove amount from account
            UnlockColour(index); //Unlock newly bought colour
            Save(); //Save the cost

            return true;

        } else
        {
            return false; //Not enough money for user, return false
        }
    }


    //Complete level
    public void CompleteLevel(int index)
    {
        //Is complete?
        if(state.completedLevels == index)
        {
            state.completedLevels++; //Increase completed levels if the index and number of completed levels are the same
            Save();
        }
    }

    /* Reset */
    //Reset a save to beginning 
    public void ResetSave()
    {
        PlayerPrefs.DeleteKey("save"); //Resets the save for the user.
    }
}
