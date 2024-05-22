using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EditorAttributes;
using UnityEngine.Serialization;

public enum Class
{
    Sword,
    Shield,
}
public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private Class playerClass;
    [SerializeField] private float playerMaxHealth;
    private float playerHealth;
    [SerializeField] private float playerMaxStamina;
    private float playerStamina;
    [SerializeField] private float playerSpeed;
    
    public Class PlayerClass { get { return playerClass; } }
    public float PlayerHealth { get { return playerHealth; } }
    public float PlayerStamina { get { return playerStamina; } }
    public float PlayerSpeed { get { return playerSpeed; } }

    private void Update()
    {
        ReganStamina();
    }

    private void Awake()
    {
        ResetStats();
    }
    private void ReganStamina()
    {
        if (playerStamina < playerMaxStamina)
        {
            playerStamina += Time.deltaTime * 3.5f;
        }
        else
        {
            playerStamina = playerMaxStamina;
        }
    }
    public void ReceiveHealth(float health)
    {
        playerHealth += health;
    }
    public void ReceiveDamage(float damage)
    {
        playerHealth -= damage;
        if (playerHealth < 0)
        {
            playerHealth = 0;
        }
    }

    public void DecreaseStamina(float cost)
    {
        playerStamina -= cost;
        if (playerStamina < 0)
        {
            playerStamina = 0;
        }
    }

    [Button("Reset Stats")]
    public void ResetStats()
    {
        playerClass = playerStats.playerClass;
        playerMaxHealth = playerStats.playerMaxHealth;
        playerMaxStamina = playerStats.playerMaxStamina;
        playerSpeed = playerStats.playerSpeed;
        
        playerHealth = playerMaxHealth;
        playerStamina = playerMaxStamina;
    }

    [Button("Upgrade Max Health")]
    public void UpgradeMaxHealth()
    {
        playerMaxHealth += 100;
    }
    
    [Button("Upgrade Stamina")]
    public void UpgradeStamina()
    {
        playerMaxStamina += 30;
    }
    [Button("Upgrade Speed")]
    public void UpgradeSpeed()
    {
        playerSpeed += 0.25f;
    }
}

