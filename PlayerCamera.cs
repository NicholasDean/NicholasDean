using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
     public Transform lookAt;

     private Vector3 offset;

     public float yOffSet = 2.0f;
     public float distance = 2.5f;

     private void Start()
     {
         offset = new Vector3(0, yOffSet, -2f * distance);
     }

     private void Update()
     {
         transform.position = lookAt.position + offset;


     }

    
}
