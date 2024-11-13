using System;
using DG.Tweening;
using UnityEngine;

public interface IDamageable
{
    void TakeDamage(float damage);
}

public class Enemy : Entity, IDamageable
{
    private float _health = 2f;
    private float _timer = 0f;
    private const float TimeRate = 1f;
    
    private void Update()
    {
        _timer += Time.deltaTime;
    }

    public void TakeDamage(float damage)
    {
        if(_timer <= TimeRate)
            return;
        _timer = 0f;
        
        _health += damage;

        if(_health <= 0)
            EnemyManager.Instance.ReleaseEnemy(this);
    }
    
    public override void Release()
    {
        transform.DOScale(0f, 1.5f).SetEase(Ease.InBounce).OnComplete(() => 
        {
            Destroy(gameObject);
        });
    }
}
