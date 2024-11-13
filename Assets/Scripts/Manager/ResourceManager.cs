using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ResourceManager : Singleton<ResourceManager>
{
    public Object[] NormalPrefabs { get; private set; }
    public PooledObject[] PoolPrefabs { get; private set; }

    private Dictionary<string, AudioClip> audioClipsList = new Dictionary<string, AudioClip>();

    protected override void Awake()
    {
        Init();
    }

    private void Init()
    {
        NormalPrefabs = LoadAll<GameObject>("Prefabs/Prefabs_Normal");
        PoolPrefabs = LoadAll<PooledObject>("Prefabs/Prefabs_Pooled");
    }

    #region SOUND_RESOURCE
    public AudioClip GetOrLoadAudioClip(string path)
    {
        if (true == audioClipsList.TryGetValue(path, out AudioClip clip))
            return clip;

        clip = Load<AudioClip>(path);

        if (clip != null)
            audioClipsList.Add(path, clip);
        else
            Debug.LogWarning($"AudioClip at path {path} not found.");

        return clip;
    }

    #endregion

    #region LOAD_RESOURCE
    public T Load<T>(string path) where T : Object
    {
        return Resources.Load<T>(path); 
    }

    public T[] LoadAll<T>(string path) where T : Object
    {
        return Resources.LoadAll<T>(path);
    }
    #endregion

    #region INSTANTIATE_PREFABS
    public GameObject Instantiate(PrefabType type, Transform parent = null)
    {
        int index = (int)type;

        if (NormalPrefabs[index] == null)
        {
            Debug.Log($"Failed to load prefab : {NormalPrefabs[index]}");
            return null;
        }

        return Instantiate(NormalPrefabs[index], parent) as GameObject;
    }
    public GameObject Instantiate(PrefabType type, Vector3 postion, Transform parent = null)
    {
        int index = (int)type;

        if (NormalPrefabs[index] == null)
        {
            Debug.Log($"Failed to load prefab : {NormalPrefabs[index]}");
            return null;
        }

        GameObject go = (Instantiate(NormalPrefabs[index], parent) as GameObject);
        go.transform.position = postion;

        return go;
    }
    public PooledObject Instantiate(PoolType type, Transform parent = null)
    {
        var index = (int)type;

        if (PoolPrefabs[index] == null)
        {
            Debug.Log($"Failed to load prefab : {PoolPrefabs[index]}");
            return null;
        }

        return Instantiate(PoolPrefabs[index], parent);
    }
    #endregion

    public void Destroy(GameObject go, float time = 0)
    {
        if (go == null)
        {
            Debug.Log($"Failed to destroy object : object is null");
            return;
        }
        Destroy(go, time);
    }
}
