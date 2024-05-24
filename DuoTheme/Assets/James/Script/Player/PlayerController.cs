using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EditorAttributes;
using UnityEngine.Serialization;
using UnityEngine.UI;

public enum Class
{
    Sword,
    Shield,
}
public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private Class playerClass;

    [Header("Health")] 
    [SerializeField] private Image healthBar;
    [SerializeField] private float playerMaxHealth;
    [ReadOnly][SerializeField]private float playerHealth;

    [Header("Stamina")] 
    [SerializeField] private Image staminaBar;
    [SerializeField] private float reganSpeed;
    [SerializeField] private float playerMaxStamina;
    [ReadOnly][SerializeField]private float playerStamina;
    
    [Header("Movement")]
    [SerializeField] private float playerSpeed;
    
    public PlayerStats PlayerStats { get { return playerStats; } }
    public Class PlayerClass { get { return playerClass; } }
    public float PlayerHealth { get { return playerHealth; } }
    public float PlayerStamina { get { return playerStamina; } }
    public float PlayerSpeed { get { return playerSpeed; } }

    private void Awake()
    {
        ResetStats();
    }
    private void Update()
    {
        ReganStamina();
        UpdateStatsGUI();
    }
    private void ReganStamina()
    {
        if (playerStamina < playerMaxStamina)
        {
            playerStamina += Time.deltaTime * reganSpeed;
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

    public void UpdateStatsGUI()
    {
        healthBar.fillAmount = playerHealth / playerMaxHealth;
        staminaBar.fillAmount = playerStamina / playerMaxStamina;
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
    
    [Button("Change Class")]
    public void ChangeClassPlayer()
    {
        if (playerClass == Class.Sword)
        {
            playerClass = Class.Shield;
        }
        else
        {
            playerClass = Class.Sword;
        }
    }
}

