using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerAnimationController : NetworkBehaviour
{
    [SerializeField] private LayerMask cursorLayer;
    [SerializeField] private SpriteRenderer playerSprite;
    [SerializeField] private Animator animator;
    private Vector3 mouseDirection;
    
    private PlayerMovement _playerMovement;
    private PlayerLookAtMouse _lookAtMouse;
    private PlayerController _playerController;
    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
        _playerMovement = GetComponent<PlayerMovement>();
        _lookAtMouse = GetComponent<PlayerLookAtMouse>();
    }

    private void Start()
    {
        ChangeAnimationClassServerRpc();
    }

    private void Update()
    {
        if (!IsOwner)
        {
            return;
        }
        if (_playerController.IsDead)
        {
            return;
        }
        MovementAnimation();
        SetMouseDirection();
    }

    private void MovementAnimation()
    {
        if (_playerMovement.MoveDirection != Vector3.zero)
        {
            animator.SetBool("OnMove", true);
        }
        else
        {
            animator.SetBool("OnMove", false);
        }
        animator.SetFloat("MoveX",_playerMovement.MoveDirection.x);
        animator.SetFloat("MoveY",_playerMovement.MoveDirection.y);
    }
    
    public void OnHoldAnimation()
    {
        animator.SetBool("OnHold",true);
    }

    public void UnHoldAnimation()
    {
        animator.SetBool("OnHold",false);
    }
    

    private void ChangeAnimationClass()
    {
        if (_playerController.PlayerClass == Class.Sword)
        {
            animator.SetLayerWeight(1,0);
        }
        else if (_playerController.PlayerClass == Class.Shield)
        {
            animator.SetLayerWeight(1,1);
        }
        
    }
    public void SetMouseDirection()
    {
        bool leftDirection = Physics2D.OverlapCircle(new Vector3(transform.position.x - 1, transform.position.y), 0.7f, cursorLayer);
        bool rightDirection = Physics2D.OverlapCircle(new Vector3(transform.position.x + 1, transform.position.y), 0.7f, cursorLayer);
        bool topDirection = Physics2D.OverlapCircle(new Vector3(transform.position.x, transform.position.y + 1), 0.7f, cursorLayer);
        bool downDirection = Physics2D.OverlapCircle(new Vector3(transform.position.x, transform.position.y - 1), 0.7f, cursorLayer);
        if (leftDirection)
        {
            ResetMousePosition();
            mouseDirection.x = -1;
        }
        else if (rightDirection)
        {
            ResetMousePosition();
            mouseDirection.x = 1;
        }
        else if (topDirection)
        {
            ResetMousePosition();
            mouseDirection.y = 1;
        }
        else if (downDirection)
        {
            ResetMousePosition();
            mouseDirection.y = -1;
        }
        
        animator.SetFloat("MouseX",mouseDirection.x);
        animator.SetFloat("MouseY",mouseDirection.y);
    }

    private void ResetMousePosition()
    {
        mouseDirection = Vector3.zero;
    }

    public void AttackAnimation(string parameterName)
    {
        animator.SetTrigger(parameterName);
    }

    public void HurtAnimation()
    {
        animator.SetTrigger("OnHurt");
    }

    public void DeadAnimation(bool isDead)
    {
        animator.SetBool("OnDie",isDead);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(new Vector3(transform.position.x,transform.position.y + 1),0.7f);
        Gizmos.DrawWireSphere(new Vector3(transform.position.x,transform.position.y - 1),0.7f);
        Gizmos.DrawWireSphere(new Vector3(transform.position.x + 1,transform.position.y),0.7f);
        Gizmos.DrawWireSphere(new Vector3(transform.position.x - 1,transform.position.y),0.7f);
    }

    [ServerRpc(RequireOwnership = false)]
    public void ChangeAnimationClassServerRpc()
    {
        if (_playerController.PlayerClass == Class.Sword)
        {
            animator.SetLayerWeight(1,0);
        }
        else if (_playerController.PlayerClass == Class.Shield)
        {
            animator.SetLayerWeight(1,1);
        }
        ChangeAnimationClassClientRpc();
    }

    [ClientRpc(RequireOwnership = false)]
    public void ChangeAnimationClassClientRpc()
    {
        ChangeAnimationClass();
    }
}
