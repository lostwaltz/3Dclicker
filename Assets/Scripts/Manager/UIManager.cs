using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : Singleton<UIManager>
{
    private GameObject root;

    public GameObject Root
    {
        get
        {
            if (null == root)
            {
                root = GameObject.Find("ROOT_UI");

                if (null == root)
                    root = new GameObject("ROOT_UI");
            }
            return root;
        }
    }

    private readonly Dictionary<string, UI_Base> _uiList = new Dictionary<string, UI_Base>();
    
    public T CreateUI<T>(PrefabType type, Transform parent = null) where T : Object
    {
        string className = typeof(T).Name;
        string path = GetPath<T>();

        if (true == IsUIExist<T>())
            RemoveUI<T>();

        T go = ResourceManager.Instance.Instantiate(type, parent ?? Root.transform).GetComponent<T>();

        (go as UI_Base)?.Init();

        AddUI<T>(go as UI_Base);

        return go;
    }

    public void AddUI<T>(UI_Base go)
    {
        string className = typeof(T).Name;

        _uiList.Add(className, go);
    }

    public void RemoveUI<T>()
    {
        string className = typeof(T).Name;

        if (false == _uiList.ContainsKey(className))
        {
            Debug.LogError("no contain key in ui list : " + className);

            return;
        }

        _uiList[className].GetComponent<UI_Base>().Release();
        _uiList.Remove(className);
    }

    public bool IsUIExist<T>()
    {
        string className = typeof(T).Name;

        return _uiList.ContainsKey(className);
    }

    public T GetUI<T>() where T : UI_Base
    {
        var className = typeof(T).Name;

        if (false == _uiList.ContainsKey(className))
        {
            return null;
        }

        return _uiList[className].GetComponent<T>();
    }

    private string GetPath<T>() where T : Object
    {
        string defaultPath = "Prefabs/NormalPrefabs/UI/";

        return defaultPath + typeof(T).Name;
    }
}
