using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private LayerMask cursorLayer;
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
        bool leftDirection = Physics2D.OverlapCircle(new Vector3(transform.position.x - 2, transform.position.y), 1.4f, cursorLayer);
        bool rightDirection = Physics2D.OverlapCircle(new Vector3(transform.position.x + 2, transform.position.y), 1.4f, cursorLayer);
        bool topDirection = Physics2D.OverlapCircle(new Vector3(transform.position.x, transform.position.y + 2), 1.4f, cursorLayer);
        bool downDirection = Physics2D.OverlapCircle(new Vector3(transform.position.x, transform.position.y - 2), 1.4f, cursorLayer);
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(new Vector3(transform.position.x,transform.position.y + 2),1.4f);
        Gizmos.DrawWireSphere(new Vector3(transform.position.x,transform.position.y - 2),1.4f);
        Gizmos.DrawWireSphere(new Vector3(transform.position.x + 2,transform.position.y),1.4f);
        Gizmos.DrawWireSphere(new Vector3(transform.position.x - 2,transform.position.y),1.4f);
    }
}
