using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
    {
        [Header("Dash Setting")] 
        [SerializeField] private float dashDuration;
        [SerializeField] private float dashDistance;
        private Vector3 moveDirection;
        
        [Header("Condition Check")] 
        private bool isDashing;

        
        [Header("Ref")] 
        private Rigidbody2D rb;
        private PlayerController _playerController;
        
        public Vector3 MoveDirection { get { return moveDirection; } }
        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            _playerController = GetComponent<PlayerController>();
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
            if (isDashing)
            {
                return;
            }

            PlayerMove();
            PlayerDash();
        }
        
        private void PlayerMove()
        {
            moveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            transform.position += moveDirection * _playerController.PlayerSpeed * Time.deltaTime;
        }

        private void PlayerDash()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartCoroutine(Dash());
            }
        }

        IEnumerator Dash()
        {
            isDashing = true;
            float dashTimer = dashDuration;
            Vector2 dashDirection = moveDirection.normalized;
            rb.velocity = dashDirection * dashDistance / dashDuration;

            while (dashTimer > 0)
            {
                dashTimer -= Time.deltaTime;
                yield return null;
            }

            rb.velocity = Vector2.zero;
            isDashing = false;
        }
    }
