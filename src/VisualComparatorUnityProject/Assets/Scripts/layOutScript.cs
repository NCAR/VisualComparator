using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class layOutScript : MonoBehaviour
{
    public RectTransform SliderRT;

    public float sliderSizePercent;
    public float panelSizePercent;
    // Update is called once per frame
    void Update()
    {

        this.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width * panelSizePercent, this.GetComponent<RectTransform>().sizeDelta.y);
        SliderRT.sizeDelta = new Vector2(this.GetComponent<RectTransform>().sizeDelta.x * sliderSizePercent, SliderRT.sizeDelta.y);

    }
}
