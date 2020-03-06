
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System;

[Serializable]
public class OnHoverDown : UnityEvent { }

[Serializable]
public class OnHoverUp : UnityEvent { }

public class HoverButton : MonoBehaviour,IPointerUpHandler,IPointerDownHandler
{

    public OnHoverDown OnHoverEnter;
    public OnHoverUp OnHoverExit;

    public void OnPointerDown(PointerEventData eventData)
    {
        OnHoverEnter.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        OnHoverExit.Invoke();
    }
}
