using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_EventHandler : MonoBehaviour, IPointerClickHandler, IDragHandler
{
    private event Action<PointerEventData> onClickEvent = null;
    private event Action<PointerEventData> onDragEvent = null;

    public Action<PointerEventData> OnClickEvent { get => onClickEvent; set => onClickEvent = value; }
    public Action<PointerEventData> OnDragEvent { get => onDragEvent; set => onDragEvent = value; }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClickEvent?.Invoke(eventData);
    }
    public void OnDrag(PointerEventData eventData)
    {
        OnDragEvent?.Invoke(eventData);
    }
}
