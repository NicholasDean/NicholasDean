using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public static Manager Instance { set; get; }

    public Material playerMaterial; //Changes player colour
    public Color[] playerColours = new Color[8]; //Stores all buyable player colours

    public int currLevel = 0; //For changing from menu to game
    public int menuFocus = 0; //For entering new menu from game

    private Dictionary<int, Vector2> activeTouches = new Dictionary<int, Vector2>(); //Screen touch dictionary, takes number of touches and position of touch

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Instance = this;
        
    }

    public Vector3 PlayerInput()
    {
        //Accelerometer being used
        if(SaveManager.Instance.state.usingAccelerometer) {
            //Repalce Y parameters with Z
            Vector3 acceleration = Input.acceleration;
            acceleration.y = acceleration.z;
            return acceleration;
        }
        //Read touches if not using accelerometer
        Vector3 t = Vector3.zero;
        foreach(Touch touch in Input.touches)
        {
            //If user started to press screen
            if (touch.phase == TouchPhase.Began)
            {
                activeTouches.Add(touch.fingerId, touch.position); //add the touches to the touch dictionary
            }
            else if (touch.phase == TouchPhase.Ended) //If user removes finger from screen
            {
                if (activeTouches.ContainsKey(touch.fingerId)) //Remove from dictionary
                {
                    activeTouches.Remove(touch.fingerId);
                }
            }
            else //If finger is moving or still, use delta
            {
                float magnitude = 0; //Magnitude set to 0
                t = (touch.position - activeTouches[touch.fingerId]); //t is equal to where the touch position is to where it has moved to
                magnitude = t.magnitude / 300; 
                t = t.normalized * magnitude; //Normalises it to stop it being accelerated through diagonal movement of the finer.
            }

        }

        return t;

    }
}

