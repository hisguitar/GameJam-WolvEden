using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer playerSprite;
    [SerializeField] private Animator animator;
    private Vector3 mouseDirection;
    
    private PlayerMovement _playerMovement;
    private PlayerLookAtMouse _lookAtMouse;
    private void Awake()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _lookAtMouse = GetComponent<PlayerLookAtMouse>();
    }

    private void Update()
    {
        RotationController();
        MovementAnimation();
        SetMouseDirection();
    }

    private void MovementAnimation()
    {
        animator.SetFloat("MoveX",_playerMovement.MoveDirection.x);
        animator.SetFloat("MoveY",_playerMovement.MoveDirection.y);
    }

    private void RotationController()
    {
        /*if (_playerMovement.MoveDirection != Vector3.zero)
        {
            if (_playerMovement.MoveDirection.x < 0)
            {
                playerSprite.flipX = true;
            }
            else if (_playerMovement.MoveDirection.x > 0)
            {
                playerSprite.flipX = false;
            }
        }
        else
        {
            if (transform.position.x > _lookAtMouse.MousePosition.x)
            {
                playerSprite.flipX = true;
            }
            else if(transform.position.x < _lookAtMouse.MousePosition.x)
            {
                playerSprite.flipX = false;
            }
        }*/
    }
    public void SetMouseDirection()
    {
        if (_lookAtMouse.MousePosition.x < transform.position.x)
        {
            mouseDirection.x = -1;
        }
        else if (_lookAtMouse.MousePosition.x > transform.position.x)
        {
            mouseDirection.x = 1;
        }

        if (_lookAtMouse.MousePosition.y < transform.position.y)
        {
            mouseDirection.y = -1;
        }
        else if (_lookAtMouse.MousePosition.y > transform.position.y)
        {
            mouseDirection.y = 1;
        }
        
        animator.SetFloat("MouseX",mouseDirection.x);
        animator.SetFloat("MouseY",mouseDirection.y);
    }

    public void AttackAnimation(string parameterName)
    {
        animator.SetTrigger(parameterName);
    }
}
