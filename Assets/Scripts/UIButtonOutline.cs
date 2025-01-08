using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIButtonOutline : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Outline outline;
    void Start()
    {
        outline=gameObject.AddComponent<Outline>();
        outline.effectDistance = Vector2.one * 4;
        outline.effectColor = Color.black;
        outline.enabled = false;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (outline != null)
        {
            outline.enabled = true;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (outline != null)
        {
            outline.enabled = false;
        }
    }
}
