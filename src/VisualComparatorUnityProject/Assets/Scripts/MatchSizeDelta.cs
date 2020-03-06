using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatchSizeDelta : MonoBehaviour
{

    public RectTransform panelRT;
    // Update is called once per frame
    void Update()
    {
        this.GetComponent<RectTransform>().sizeDelta = panelRT.sizeDelta;
    }
}
