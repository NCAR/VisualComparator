using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OnSeekScript : MonoBehaviour,IPointerClickHandler
{
    public VideoControllerScript VCS;
    // Update is called once per frame
   

    public void OnPointerClick(PointerEventData eventData)
    {
        //Debug.Log("Clicked" + " Slider:    " + GetComponent<Slider>().value);

        //VCS.Seek(GetComponent<Slider>().value);

        Vector2 clickPosition;

        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(GetComponent<RectTransform>(), eventData.position, eventData.pressEventCamera, out clickPosition))
            return;

        //print("clicked position on the gameobject  is :" + ((clickPosition.x/ GetComponent<RectTransform>().sizeDelta.x)));
        VCS.Seek(clickPosition.x / GetComponent<RectTransform>().sizeDelta.x);
    }
}
