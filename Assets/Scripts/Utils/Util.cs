using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Util
{
    public static GameObject FindChild(GameObject go, string name = null, bool recurslve = false)
    {
        return FindChild<Transform>(go, name, recurslve).gameObject;
    }

    public static T FindChild<T>(GameObject go, string name = null, bool recurslve = false) where T : UnityEngine.Object
    {
        if(go == null)
            return null;

        if(recurslve == false)
        {
            for (int i = 0; i < go.transform.childCount; i++)
            {
                Transform transform = go.transform.GetChild(i);

                if (string.IsNullOrEmpty(name) || transform.name == name)
                {
                    T component = transform.GetComponent<T>();
                    if (component != null)
                        return component;
                }
            }

        }
        else
        {
            foreach (T component in go.GetComponentsInChildren<T>())
            {
                if (string.IsNullOrEmpty(name) || component.name == name)
                    return component;
            }
        }

        return null;
    }

}