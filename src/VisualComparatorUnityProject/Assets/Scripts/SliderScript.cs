using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SliderScript : MonoBehaviour,IPointerUpHandler,IPointerDownHandler
{
    public GameObject videoRawImageGO;
    //public GameObject videoBottomGO;
    private float xScale;
    public bool isVertical { get; set; }
    private Slider slider;
    private int videoIndex = 0;
    public static float[] sliderValues;
    //private static bool ColorRed=true ;

    private void Update()
    {
        if(sliderValues.Length>1 && videoIndex==0)
        {
            if((sliderValues[0]-sliderValues[1])<=0)
            {

                    this.GetComponent<Slider>().image.color = new Color(1f, 0f, 0f, this.GetComponent<Slider>().image.color.a);
                    

            }
            else
            {
             
                    this.GetComponent<Slider>().image.color = new Color(1f, 1f, 1f, this.GetComponent<Slider>().image.color.a);
                   
            }
        }
    }

    void OnEnable()
    {
        
        //Debug.Log(sliderValues.Length);
        if(videoRawImageGO.transform.GetSiblingIndex()==1)
        {
            //Debug.Log("is second video");
            videoIndex = 0;
        }
        else
        {
            //Debug.Log("is Third video");
            videoIndex = 1;
        }

        slider = GetComponent<Slider>();
        //videoRawImageGO.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, Screen.height);

        RectTransform rt = this.transform.parent.GetComponent<RectTransform>();
        videoRawImageGO.GetComponent<RectTransform>().sizeDelta = rt.sizeDelta;

        if (slider.direction== Slider.Direction.LeftToRight || slider.direction==Slider.Direction.RightToLeft)
        {
            isVertical = false;
            videoRawImageGO.GetComponent<RectTransform>().anchorMax =new Vector2(0f,0.5f);
            videoRawImageGO.GetComponent<RectTransform>().anchorMin =new Vector2(0f, 0.5f);
            videoRawImageGO.GetComponent<RectTransform>().pivot = new Vector2(0f, 0.5f);

            //this.GetComponent<RectTransform>().localPosition = new Vector3(this.GetComponent<RectTransform>().localPosition.x,
                //0f,
                //this.GetComponent<RectTransform>().localPosition.z);

        }
        else
        {
            isVertical = true;
            videoRawImageGO.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 1f);
            videoRawImageGO.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 1f);
            videoRawImageGO.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 1f);
            //this.GetComponent<RectTransform>().localPosition = new Vector3(0f,
                //this.GetComponent<RectTransform>().localPosition.y,
                //this.GetComponent<RectTransform>().localPosition.z);

        }

        slider.value = 0.5f;

        //Debug.Log("width:  "+Screen.width+"   Height:  "+Screen.height);


    }

    void OnDisable()
    {

        slider.value = 1f;
        Vector3 tempPos = this.GetComponent<RectTransform>().localPosition;
        if (isVertical)
        {
            this.GetComponent<RectTransform>().localPosition = new Vector3(0f, tempPos.y, tempPos.z);
        }
        else
        {
            this.GetComponent<RectTransform>().localPosition = new Vector3(tempPos.x, -(videoRawImageGO.GetComponent<RectTransform>().sizeDelta.y / 2), tempPos.z);
        }

        this.GetComponent<Slider>().image.color = new Color(1f, 1f, 1f, this.GetComponent<Slider>().image.color.a);

    }

    public void ValueChange()
    {

        sliderValues[videoIndex] = slider.value;

       

        float tempSliderVal = slider.value;

        if (tempSliderVal <= 0)
        {
            tempSliderVal = 0;
        }

        Vector3 tempScale = videoRawImageGO.GetComponent<RectTransform>().localScale;

        if(isVertical)
        {
            tempScale.y = tempSliderVal;
            videoRawImageGO.GetComponent<RawImage>().uvRect = new Rect(0f, 1-tempSliderVal, 1f, tempSliderVal);
        }
        else
        {
            tempScale.x = tempSliderVal;
            videoRawImageGO.GetComponent<RawImage>().uvRect = new Rect(0f, 0f, tempSliderVal, 1f);
        }

        videoRawImageGO.GetComponent<RectTransform>().localScale = tempScale;
            
    }



    Color tempCol;
    public void OnPointerUp(PointerEventData eventData)
    {

        //Make Slider Appear
        this.GetComponent<Slider>().image.color = new Color(tempCol.r, tempCol.g, tempCol.b, 1f);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        tempCol = this.GetComponent<Slider>().image.color;
        // Make the image disappear/lighten
        this.GetComponent<Slider>().image.color = new Color(tempCol.r, tempCol.g, tempCol.b, 0.3f);
    }
}
