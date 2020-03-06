using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaintainIndependentScale : MonoBehaviour
{
    public RectTransform ReferenceRT;
    public int Tag;
    static float[] panelScales = new float[2];
    static ushort NoOfPanelsReady = 0;
    private ushort numberOfVideos;
    private bool isSliderHorizontal;
    // Update is called once per frame
    void OnEnable()
    {
        numberOfVideos = (ushort)GameObject.FindWithTag("VideoManager").GetComponent<VideoControllerScript>().numberOfVideos;
        isSliderHorizontal=!GameObject.FindWithTag("VideoManager").GetComponent<VideoControllerScript>().VideoSliders[Tag].GetComponent<SliderScript>().isVertical;


        GetSizeAndContinue();


    }

    private void OnDisable()
    {

        NoOfPanelsReady = 0;
        this.GetComponent<Text>().enabled = false;
    }

    private void GetSizeAndContinue()
    {

        if(Tag==2 || Tag==1)
        {
            Debug.Log(this.transform.parent.GetComponent<RectTransform>().sizeDelta);

            if(Tag==2)
            {
               
                panelScales[1]= this.transform.parent.transform.parent.GetComponent<RectTransform>().localScale.y;

                NoOfPanelsReady++;

            }
            else if(Tag==1)
            {
                panelScales[0]= this.transform.parent.transform.parent.GetComponent<RectTransform>().localScale.y;

                NoOfPanelsReady++;
            }

        }

        StartCoroutine(WaitForPanelSize());

    }
    private IEnumerator WaitForPanelSize()
    {
        while(NoOfPanelsReady<numberOfVideos-1)
        {
            yield return null;
        }

        SetPanelSize();
    }

    private void SetPanelSize()
    {
        Vector2 tempVec2 = this.transform.parent.transform.parent.transform.parent.GetComponent<RectTransform>().sizeDelta;

        float scale = tempVec2.y;

        if (scale<=0.01)
        {
            scale = 0.01f;
        }

        if (Tag == 1)
        {
            if(panelScales[0]<=0.01f)
            {
                SetPanelRToffset(new Vector2(this.GetComponent<RectTransform>().offsetMax.x, -(panelScales[1] / 0.01f) * scale));
            }
            else
            {
                SetPanelRToffset(new Vector2(this.GetComponent<RectTransform>().offsetMax.x, -(panelScales[1] / panelScales[0]) * scale));
            }

        }

        else if (Tag == 0)
        {
            if(panelScales[0]<panelScales[1])
            {

                SetPanelRToffset(new Vector2(this.GetComponent<RectTransform>().offsetMax.x, -(panelScales[1] / 1f) * scale));


            }
            else
            {

                SetPanelRToffset(new Vector2(this.GetComponent<RectTransform>().offsetMax.x, -(panelScales[0] / 1f) * scale));

            }

        }

        if (ReferenceRT.localScale.x >= 0.001 && ReferenceRT.localScale.y >= 0.001 && ReferenceRT.localScale.z > 0.001)
        {
            this.GetComponent<RectTransform>().localScale = new Vector3(1f / ReferenceRT.localScale.x, 1f / ReferenceRT.localScale.y, 1f / ReferenceRT.localScale.z);
        }

        Debug.Log(tempVec2);

        this.GetComponent<Text>().enabled = true;
    }

    private void SetPanelRToffset(Vector2 sizeIn)
    {
        this.GetComponent<RectTransform>().offsetMax = sizeIn;
    }
}
