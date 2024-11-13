using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UIShowType
{
    NONE,
    UPSCALE
}
public enum UIHideType
{
    NONE,
    DOWNSCALE
}

// TODO : repectoring - 
public abstract class UI_View : UI_Base
{
    public UIShowType showType;
    public UIHideType hideType;

    private float _showTime;
    private float _hideTime;

    private IDisplayAnimation _displayAnimation;

    public override void Init()
    {
    }

    public override void Release()
    {
    }

    protected void ShowExecute(Action action = null)
    {
        _displayAnimation.ShowAnimationExecute(_showTime, action);
    }
    protected void HideExecute(Action action = null)
    {
        _displayAnimation.HideAnimationExecute(_hideTime, action);
    }

    protected void InitDisplayHandler(float showTime, float hideTime, IDisplayAnimation displayAnimation)
    {
        _showTime = showTime;
        _hideTime = hideTime;

        _displayAnimation = displayAnimation;
    }

}
