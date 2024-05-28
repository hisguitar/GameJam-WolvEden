using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public abstract class BossHealth : NetworkBehaviour
{
    [SerializeField] private float maxBossHealth;
    [ReadOnly][SerializeField] private float bossHealth;
    [SerializeField] private Image healthBar;
    [SerializeField] private Vector2 offSet;
    [SerializeField] private Vector2 areaBossRadius;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private Animator _animator;
    private bool bossActive;

    private  void Update()
    {
        if (!IsOwner)
        {
            return;
        }

        UpdateGUI();
        CheckPlayerInArea();
    }

    private void UpdateGUI()
    {
        healthBar.fillAmount = bossHealth / maxBossHealth;
    }

    private void CheckPlayerInArea()
    {
        if (bossActive)
        {
            return;
        }
        bool playerIn = Physics2D.OverlapBox(offSet, areaBossRadius, 10, playerLayer);
        if (playerIn)
        {
            ActiveBoss();
        }
        
    }

    private void ActiveBoss()
    {
        bossActive = true;
        bossHealth = maxBossHealth;
        _animator.SetBool("BossActive",true);
    }

    public void UnActiveBoss()
    {
        bossActive = false;
        _animator.SetBool("BossActive",false);
    }

    [ServerRpc]
    public void TakeDamageServerRpc(float damage)
    {
        bossHealth -= damage;
        if (bossHealth < 0)
        {
            bossHealth = 0;
        }
        TakeDamageClientRpc(damage);
    }

    [ClientRpc]
    private void TakeDamageClientRpc(float damage)
    {
        if (IsOwner)
        {
            return;
        }
        bossHealth -= damage;
        if (bossHealth < 0)
        {
            bossHealth = 0;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(offSet,areaBossRadius);
    }
}
