using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScalerScript : MonoBehaviour
{
    public int ReferenceResolution;
    void Update()
    {
        this.GetComponent<RectTransform>().localScale = new Vector3((float)Screen.currentResolution.height/ ReferenceResolution,
            (float)Screen.currentResolution.height / ReferenceResolution,
            (float)Screen.currentResolution.height / ReferenceResolution);
            
    }
}
