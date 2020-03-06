using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FullScreenButtonScript : MonoBehaviour,IPointerDownHandler
{
    public Crosstales.FB.Demo.CanvasComponentsScript CCS;
    public void OnPointerDown(PointerEventData eventData)
    {

        CCS.OnFullScreenButtonPress();
    }

}
