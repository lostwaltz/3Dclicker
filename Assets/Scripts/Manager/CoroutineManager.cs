using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: refactor coroutine manager to minimize garbage generation
public class CoroutineManager : Singleton<CoroutineManager>
{
    private readonly Dictionary<string, Coroutine> _coroutineDictionary = new Dictionary<string, Coroutine>();

    public void StartManagedCoroutine(string key, IEnumerator coroutine)
    {
        if (_coroutineDictionary.TryGetValue(key, out var value))
            StopCoroutine(value);
        
        var newCoroutine = StartCoroutine(WrapCoroutine(key, coroutine));
        _coroutineDictionary.Add(key, newCoroutine);
    }

    public void StopManagedCoroutine(string key)
    {
        if (!_coroutineDictionary.TryGetValue(key, out var value))
            return;
        
        StopCoroutine(value);
        
        _coroutineDictionary.Remove(key);
    }

    public void StopAllManagedCoroutines()
    {
        foreach (var coroutine in _coroutineDictionary.Values)
        {
            StopCoroutine(coroutine);
        }
        _coroutineDictionary.Clear();
    }

    private IEnumerator WrapCoroutine(string key, IEnumerator coroutine)
    {
        yield return StartCoroutine(coroutine);
        _coroutineDictionary.Remove(key);
    }
}