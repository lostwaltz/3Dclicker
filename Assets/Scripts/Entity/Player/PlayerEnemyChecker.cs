using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnemyChecker : MonoBehaviour
{
    public bool IsHit { get; private set; }
    [SerializeField] private LayerMask mask;
    [SerializeField] private Transform rayPoint;

    private float _time;
    private const float Rate = 0.01f;

    private void Update()
    {
        _time += Time.deltaTime;
        if(_time <= Rate)
            return;
        
        CheckEnemyOnFront();
    }

    private void CheckEnemyOnFront()
    {
        Ray ray = new Ray(rayPoint.position, rayPoint.forward);
        IsHit = Physics.Raycast(ray, out RaycastHit hit, 1f, mask);

        if (false == IsHit) return;
            
        hit.transform.gameObject.TryGetComponent(out IDamageable damageable);
        damageable?.TakeDamage(-1f);
    }
}
