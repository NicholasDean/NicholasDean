using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCamera : MonoBehaviour
{
    private Vector3 startPos;
    private Quaternion startRotation;

    private Vector3 desiredPos;
    private Quaternion desiredRotation; 

    public Transform shopWaypoint;
    public Transform levelWaypoint;

    private void Start()
    {
        startPos = desiredPos = transform.localPosition;
        startRotation = desiredRotation = transform.rotation;

    }

    private void Update()
    {
        float x = Manager.Instance.PlayerInput().x;

        transform.localPosition = Vector3.Lerp(transform.localPosition, desiredPos + new Vector3(0,x,0) * 0.01f, 0.1f);
        transform.localRotation = Quaternion.Lerp(transform.localRotation, desiredRotation, 0.1f);
    }

    public void ToMainMenu()
    {
        desiredPos = startPos;
        desiredRotation = startRotation;
    }

    public void ToShop()
    {
        desiredPos = shopWaypoint.localPosition;
        desiredRotation = shopWaypoint.localRotation;
    }

    public void ToLevel()
    {
        desiredPos = levelWaypoint.localPosition;
        desiredRotation = levelWaypoint.localRotation;
    }

}
