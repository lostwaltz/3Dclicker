using System;
using DG.Tweening;
using UnityEngine;

public class SpawnEffects : MonoBehaviour
{
    [SerializeField] private GameObject spawnVFX;
    [SerializeField] private float animationDuration = 1f;
    [SerializeField] private Ease type;

    private void Start()
    {
        transform.localScale = Vector3.zero;
        transform.DOScale(Vector3.one, animationDuration).SetEase(type);

        if (spawnVFX != null)
            Instantiate(spawnVFX, transform.position, Quaternion.identity);

        if(true == TryGetComponent<AudioSource>(out AudioSource audioSource))
            audioSource.Play();
    }
}