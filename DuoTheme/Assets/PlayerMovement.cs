using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JamesCode
{
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Movement Setting")] 
        private float playerSpeed;
        
        [Header("Dash Setting")] 
        [SerializeField] private float dashDuration;
        [SerializeField] private float dashDistance;
        private Vector3 moveDirection;
        
        [Header("Condition Check")] 
        private bool isDashing;

        
        [Header("Ref")] 
        private Rigidbody2D rb;
        private PlayerController _playerController;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            _playerController = GetComponent<PlayerController>();
            playerSpeed = _playerController.PlayerSpeed;
        }

        private void Update()
        {
            PlayerDash();
        }

        private void FixedUpdate()
        {
            if (isDashing)
            {
                return;
            }

            PlayerMove();
        }

        private void PlayerMove()
        {
            moveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            transform.position += moveDirection * playerSpeed * Time.deltaTime;
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
}
