using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHideScaleDown : IHideDisplayAnimation
{
    GameObject displayScreen;

    public UIHideScaleDown(GameObject _displayScreen)
    {
        displayScreen = _displayScreen;
    }

    public void Execute(float duration, Action action)
    {
        CoroutineManager.Instance.StartManagedCoroutine("UIHideAnimation", ScaleDown(duration, action));
    }

    IEnumerator ScaleDown(float duration, Action action)
    {
        Vector3 initialScale = new Vector3(1f, 1f);
        Vector3 targetScale = new Vector3(0f, 0f, 1f);

        displayScreen.transform.localScale = initialScale;

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            displayScreen.transform.localScale = Vector3.Lerp(initialScale, targetScale, elapsedTime / duration);
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }

        displayScreen.transform.localScale = targetScale;
        action?.Invoke();
    }
}
