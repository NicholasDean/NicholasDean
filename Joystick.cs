using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{

    private Image backImage;
    private Image frontImage;
    public Vector3 inputDirection { set; get; } //Setters amd getters for the input direction


    private void Start()
    {
        backImage = GetComponent<Image>();
        frontImage = transform.GetChild(0).GetComponent<Image>();
        inputDirection = Vector3.zero;

    }
    public virtual void OnDrag(PointerEventData eventData)
    {
        Vector2 position = Vector2.zero;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(backImage.rectTransform, eventData.position, eventData.pressEventCamera, out position))
        {
            position.x = (position.x / backImage.rectTransform.sizeDelta.x); //Get a ratio of where the user click within the joystick
            position.y = (position.y / backImage.rectTransform.sizeDelta.y);

            float x = (backImage.rectTransform.pivot.x == 1) ? position.x * 2 + 1 : position.x * 2 - 1;
            float y = (backImage.rectTransform.pivot.y == 1) ? position.y * 2 + 1 : position.y * 2 - 1;

            inputDirection = new Vector3(x, 0, y);

            inputDirection = (inputDirection.magnitude > 1) ? inputDirection.normalized : inputDirection; //if the input direction is greater than 1 normalise the input if not leave it as intended 

            frontImage.rectTransform.anchoredPosition = new Vector3(inputDirection.x * (backImage.rectTransform.sizeDelta.x / 3), inputDirection.z * (backImage.rectTransform.sizeDelta.y/ 3));



        }
    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
        inputDirection = Vector3.zero; //Sets joystick back to zero when user releases the joystick
        frontImage.rectTransform.anchoredPosition = Vector3.zero; //Moves the image with this as well
        
        
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData); //Makes the joystick move if the user just presses the joystick without moving around
    }

}
