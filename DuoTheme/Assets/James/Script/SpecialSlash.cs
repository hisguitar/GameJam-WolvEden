using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SpecialSlash : NetworkBehaviour
{
    private SpriteRenderer slashSprite;
    private bool onActive;
    [SerializeField] private string tagName;
    public float damage;
    
    [Header("Ref")] 
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private PlayerAnimationController _animationController;
    private Animator _animator;
    private Collider2D _collider2D;

    private void OnEnable()
    {
        slashSprite = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _collider2D = GetComponent<Collider2D>();
    }

    public bool OnActive
    {
        get { return onActive; }
    }
    public void SetDamage(float damage)
    {
        this.damage = damage;
    }

    [ServerRpc]
    public void ActiveSlashServerRpc()
    {
        slashSprite.enabled = true;
        _collider2D.enabled = true;
        onActive = true;
        _animator.SetTrigger("SlashOn");
        ActiveSlashServerRpc();
    }

    [ServerRpc]
    public void UnActiveSlashServerRpc()
    {
        onActive = false;
        slashSprite.enabled = false;
        _collider2D.enabled = false;
        UnActiveSlashServerRpc();
    }
    [ClientRpc]
    public void ActiveSlashClientRpc()
    {
        if (IsOwner)
        {
            return;
        }
        slashSprite.enabled = true;
        _collider2D.enabled = true;
        onActive = true;
        _animator.SetTrigger("SlashOn");
    }
    [ClientRpc]
    public void UnActiveSlashClientRpc()
    {
        if (IsOwner)
        {
            return;
        }
        onActive = false;
        slashSprite.enabled = false;
        _collider2D.enabled = false;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(tagName))
        {
            other.GetComponent<BossHealth>().TakeDamageServerRpc(damage);
        }
    }
}
