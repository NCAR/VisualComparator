using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActivateVideoRawImage : MonoBehaviour
{
    public RawImage RI;


    public void OnEnable()
    {
        RI.enabled = true;
    }

    public void OnDisable()
    {
        if (RI != null)
        {
            RI.enabled = false;
        }

    }

}
