using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class UI_Base : MonoBehaviour
{
    public abstract void Init();
    public abstract void Release();

    public static void BindEvent(GameObject go, Action<PointerEventData> action, Define.UIEvent type = Define.UIEvent.CLICK)
    {
        UI_EventHandler evt = go.GetOrAddComponent<UI_EventHandler>();

        switch (type)
        {
            case Define.UIEvent.CLICK:
                evt.OnClickEvent -= action;
                evt.OnClickEvent += action;
                break;
            case Define.UIEvent.DRAG:
                evt.OnDragEvent -= action;
                evt.OnDragEvent += action;
                break;
        }
    }
}
