using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;


public class ObjectPoolManager : Singleton<ObjectPoolManager>
{

    private readonly Dictionary<PoolType, IObjectPool<PooledObject>> _poolDictionary = new Dictionary<PoolType, IObjectPool<PooledObject>>();
    private ResourceManager _resourceManager;

    protected override void Awake()
    {
        Init();
    }

    private void Init()
    {
        _resourceManager = ResourceManager.Instance;

        PooledObject[] pooledObjects = _resourceManager.PoolPrefabs;

        int idx = 0;
        foreach (PooledObject obj in pooledObjects)
        {
            PoolType index = (PoolType)idx;

            CreateNewPool(obj, index);

            for (int i = 0; i < obj.capacity; i++)
                _poolDictionary[index].Release(OnCreatePool(index));

            idx++;
        }
    }

    private void CreateNewPool(PooledObject pooledObject, PoolType poolType)
    {
        _poolDictionary[poolType] = new ObjectPool<PooledObject>(() =>
        OnCreatePool(poolType),
        OnGetFromPool,
        OnReleaseToPool,
        OnDestroyPooledObject,
        true, pooledObject.capacity, pooledObject.maxSize);
    }


    private PooledObject OnCreatePool(PoolType pooledObject)
    {
        return _resourceManager.Instantiate(pooledObject, gameObject.transform);
    }

    private void OnGetFromPool(PooledObject pooledObject)
    {
        pooledObject.gameObject.SetActive(true);
    }

    private void OnReleaseToPool(PooledObject pooledObject)
    {
        pooledObject.gameObject.SetActive(false);
    }

    private void OnDestroyPooledObject(PooledObject pooledObject)
    {
        Destroy(pooledObject.gameObject);
    }


    public PooledObject GetPooledObject(PoolType poolType, Transform parent = null)
    {
        PooledObject po = _poolDictionary[poolType].Get();
        if (parent != null)
            po.transform.SetParent(parent, false);

        return po;
    }

    public void ReleaseObject(PoolType poolType, PooledObject obj)
    {
        obj.gameObject.transform.SetParent(gameObject.transform, false);

        _poolDictionary[poolType].Release(obj);
    }
}