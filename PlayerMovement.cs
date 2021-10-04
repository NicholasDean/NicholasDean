using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody controller;
    private CharacterController characterController;

    private float baseSpeed = 9.0f;
    private float drag = 0.5f;
    private float roatSpeed = 20.0f;

    public Joystick joyStick;
    public GameObject gameObjectJoyStick;

    public AudioSource carHorn;

    public Transform resetBox;

    public bool accelerometerOn;

    private void Start()
    {
        if (!accelerometerOn)
        {
            controller = GetComponent<Rigidbody>(); //Gets the rigidbody of the game object
            controller.maxAngularVelocity = roatSpeed; //This is to prevent numerical instability with the roatating ball
            controller.drag = drag; //The amount of drag the ball has, used to slow the ball down
        } else if (accelerometerOn) {
            gameObjectJoyStick.SetActive(false);
            characterController = GetComponent<CharacterController>();
        }
      
    }

    private void Update()
    {

        if (!accelerometerOn)
        {
            Vector3 direction = Vector3.zero; //Makes Vector3(0,0,0)
            if (direction.magnitude > 1)
            {
                direction.Normalize(); //To stop faster speed through the ball moving at a diagonal direction, as the magnitude will be greater than 1 when moving at an angle.
            }
            if (joyStick.inputDirection != Vector3.zero) //If jotstick is moving
            {
                direction = joyStick.inputDirection; //Takes input direction from the joystick
            }
            controller.AddForce(direction * baseSpeed); //Adding force to the player
            if (transform.position.y >= -2 && transform.position.y <= 3) //If the ball drops below the road stop the resetbox from following the ball
            {
                resetBox.position = transform.position;
            }
        } else if (accelerometerOn) {
            //Player inputs
            Vector3 input = Manager.Instance.PlayerInput();

            Vector3 xRotation = input.x * transform.right * baseSpeed * Time.deltaTime;
            Vector3 direction = xRotation;

        }
    }

}
