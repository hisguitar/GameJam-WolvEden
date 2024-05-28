using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Damageable: MonoBehaviour
{
    [SerializeField] private string tagName;
    public float damage;

    public void SetDamage(float damage)
    {
        this.damage = damage;
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(tagName))
        {
            other.GetComponent<BossHealth>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
