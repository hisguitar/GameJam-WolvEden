using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;

public abstract class BossHealth : MonoBehaviour
{
    [SerializeField] private float maxBossHealth;
    [ReadOnly][SerializeField] private float bossHealth;
    [SerializeField] private Image healthBar;

    private void Update()
    {
        healthBar.fillAmount = bossHealth / maxBossHealth;
    }

    public void TakeDamage(float damage)
    {
        bossHealth -= damage;
        if (bossHealth < 0)
        {
            bossHealth = 0;
        }
    }
}
