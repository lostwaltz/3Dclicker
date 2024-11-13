using System;
using UnityEngine;

public interface IShowDisplayAnimation
{
    public void Execute(float duration, Action action);
}
public interface IHideDisplayAnimation
{
    public void Execute(float duration, Action action);
}

public interface IDisplayAnimation
{
    public void ShowAnimationExecute(float duration, Action action);
    public void HideAnimationExecute(float duration, Action action);
}

public class DisplayAnimationHandler : IDisplayAnimation
{
    private readonly IShowDisplayAnimation _showAnimation;
    private readonly IHideDisplayAnimation _hideAnimation;

    public DisplayAnimationHandler(UIShowType showType, UIHideType hideType, GameObject displayScreen)
    {
        // TODO : decide whether to apply the factory pattern

        switch (showType)
        {
            default:
            case UIShowType.NONE:
                break;
            case UIShowType.UPSCALE:
                _showAnimation = new UIShowScaleUp(displayScreen);
                break;
        }

        switch (hideType)
        {
            default:
            case UIHideType.NONE:
                break;
            case UIHideType.DOWNSCALE:
                _hideAnimation = new UIHideScaleDown(displayScreen);
                break;
        }
    }

    public void ShowAnimationExecute(float duration, Action action)
    {
        _showAnimation?.Execute(duration, action);
    }
    public void HideAnimationExecute(float duration, Action action)
    {
        _hideAnimation.Execute(duration, action);
    }
}
