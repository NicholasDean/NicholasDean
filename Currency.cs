using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Currency : MonoBehaviour
{
    private void Update()
    {
        transform.Rotate(Vector3.left * 75 * Time.deltaTime); //Rotates the currency within the level
    }

   private void OnTriggerEnter(Collider other)
    {
        SaveManager.Instance.state.currency++; //Adds currency to the users amount eachtime one is picked up
        SaveManager.Instance.state.collectedCurrency++; //Adds to the total collected currency for each level
        SaveManager.Instance.Save(); //Saves 
        Destroy(gameObject); //Removes currency once one is collected
    }

}
