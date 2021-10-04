using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetBox : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {

            
            SaveManager.Instance.state.currency = SaveManager.Instance.state.currency - SaveManager.Instance.state.collectedCurrency; //Removes collected currencies to total
            SaveManager.Instance.state.collectedCurrency = 0; //Resets collected currencies
            SaveManager.Instance.state.cpCollected = 0; //Resets checkpoints collected
            SceneManager.LoadScene("Game"); //Reloads the game scene
        

    }
}
