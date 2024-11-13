using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIShowScaleUp : IShowDisplayAnimation
{
    private GameObject displayScreen;

    public UIShowScaleUp(GameObject _displayScreen)
    {
        displayScreen = _displayScreen;
    }

    public void Execute(float duration, Action action)
    {
        CoroutineManager.Instance.StartManagedCoroutine("UIShowAnimation", ScaleUp(duration, action));
    }
    IEnumerator ScaleUp(float duration, Action action)
    {
        Vector3 initialScale = new Vector3(0f, 0f, 1f);
        Vector3 targetScale = new  Vector3(1f, 1f, 1f);

        displayScreen.transform.localScale = initialScale;

        float elapsed = 0.0f; 

        while (elapsed < duration)
        {
            displayScreen.transform.localScale = Vector3.Lerp(initialScale, targetScale, elapsed / duration);

            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        displayScreen.transform.localScale = targetScale;

        action?.Invoke();
    }
}