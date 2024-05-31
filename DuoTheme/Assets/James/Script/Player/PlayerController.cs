using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EditorAttributes;
using Unity.Netcode;
using UnityEditor;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum Class
{
    Sword = 0,
    Shield = 1,
}
public class PlayerController : NetworkBehaviour
{
    [SerializeField] private List<PlayerStats> playerStats;
    [SerializeField] private Class playerClass;
    private Class previousClass;

    [Header("Health")] 
    [SerializeField] private bool isDead;
    [SerializeField] private Image healthBar;
    [SerializeField] private float playerMaxHealth;
    [ReadOnly][SerializeField]private float playerHealth;

    [Header("Stamina")] 
    [SerializeField] private Image staminaBar;
    [SerializeField] private float reganSpeed;
    [SerializeField] private float playerMaxStamina;
    [ReadOnly][SerializeField]private float playerStamina;

    [Header("Movement")] 
    [SerializeField] private float playerMaxSpeed;
    [ReadOnly][SerializeField] private float playerSpeed;

    [Header("GUI")] 
    [SerializeField] private GameObject playerHUD;

    [Header("Ref")] 
    public PlayerAnimationController _playerAnimationController;
    private UserData userData;
    
    public PlayerStats PlayerStats { get { return playerStats[(int)playerClass]; } }

    public Class PlayerClass
    {
        get { return playerClass; }
        set { playerClass = value; }
    }

    public float PlayerHealth { get { return playerHealth; } }
    public float PlayerStamina { get { return playerStamina; } }
    public float PlayerSpeed { get { return playerSpeed; } }
    public bool IsDead { get { return isDead; } }

    public static event Action<PlayerController> OnPlayerSpawned; 
    public static event Action<PlayerController> OnPlayerDespawned; 
    
    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            OnPlayerSpawned?.Invoke(this);
        }

        if (!IsOwner)
        {
            playerHUD.SetActive(false);
            return;
            
        }

        if (IsOwner)
        { 
            SetDataServerRpc();
            Debug.Log("Start ResetStats");
            ResetStatsServerRpc();
            //StartClassSelector.Instance.PlayerList.Add(this);
        }
    }
    public override void OnNetworkDespawn()
    {
        if (IsServer)
        {
            OnPlayerDespawned?.Invoke(this);
        }
    }
    
    private void Update()
    {
        if (!IsOwner)
        {
            return;
        }
        SetDataServerRpc();
        ReganStamina();
        UpdateStatsGUI();
        CheckClassChange();
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
        if (playerHealth <= 0)
        {
            PlayerDead();
            playerHealth = 0;
        }
        else
        {
            _playerAnimationController.HurtAnimation();
        }
    }

    private void CheckClassChange()
    {
        if (previousClass != playerClass)
        {
            ResetStatsServerRpc();
            previousClass = playerClass;
        }
    }

    private void PlayerDead()
    {
        GetComponent<Collider2D>().enabled = false;
        _playerAnimationController.DeadAnimation(true);
        isDead = true;
    }

    public void DecreaseStamina(float cost)
    {
        playerStamina -= cost;
        if (playerStamina < 0)
        {
            playerStamina = 0;
        }
    }

    public void ReduceSpeed(float reduceCount)
    {
        playerSpeed -= reduceCount;
    }

    public void SetSpeedDefault()
    {
        playerSpeed = playerMaxSpeed;
    }

    public void UpdateStatsGUI()
    {
        healthBar.fillAmount = playerHealth / playerMaxHealth;
        staminaBar.fillAmount = playerStamina / playerMaxStamina;
    }

    [ContextMenu("Reset Stats")]
    public void ResetStats()
    {
        playerClass = playerStats[(int)playerClass].playerClass;
        playerMaxHealth = playerStats[(int)playerClass].playerMaxHealth;
        playerMaxStamina = playerStats[(int)playerClass].playerMaxStamina;
        playerMaxSpeed = playerStats[(int)playerClass].playerSpeed;
        
        playerSpeed = playerMaxSpeed;
        playerHealth = playerMaxHealth;
        playerStamina = playerMaxStamina;
        _playerAnimationController.ChangeAnimationClassServerRpc();
        UpdateStatsGUI();
    }

    [ContextMenu("Upgrade Max Health")]
    public void UpgradeMaxHealth()
    {
        playerMaxHealth += 100;
    }
    
    [ContextMenu("Upgrade Stamina")]
    public void UpgradeStamina()
    {
        playerMaxStamina += 30;
    }
    [ContextMenu("Upgrade Speed")]
    public void UpgradeSpeed()
    {
        playerSpeed += 0.25f;
    }
    
    [ContextMenu("Change Class")]
    public void ChangeClassPlayer()
    {
        if (playerClass == Class.Sword)
        {
            playerClass = Class.Shield;
            ResetStatsServerRpc();
        }
        else
        {
            playerClass = Class.Sword;
            ResetStatsServerRpc();
        }
    }

    [ContextMenu("Test Damage")]
    public void TestDamage()
    {
        ReceiveDamage(50);
    }

    [ServerRpc(RequireOwnership = false)]
    public void ResetStatsServerRpc()
    {
        playerClass = playerStats[(int)playerClass].playerClass;
        playerMaxHealth = playerStats[(int)playerClass].playerMaxHealth;
        playerMaxStamina = playerStats[(int)playerClass].playerMaxStamina;
        playerMaxSpeed = playerStats[(int)playerClass].playerSpeed;
        
        playerSpeed = playerMaxSpeed;
        playerHealth = playerMaxHealth;
        playerStamina = playerMaxStamina;
        _playerAnimationController.ChangeAnimationClassServerRpc();
        UpdateStatsGUI();
        ResetStatsClientRpc();
    }

    [ClientRpc]
    private void ResetStatsClientRpc()
    {
        Debug.Log("Class Change Client RPC");
        ResetStats();
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetDataServerRpc()
    {
        if (IsOwner)
        {
            playerClass = StartClassSelector.Instance._hostData.userClass;
            previousClass = playerClass;
        }
        else if (!IsOwner)
        {
            playerClass = StartClassSelector.Instance._clientData.userClass;
            previousClass = playerClass;
        }
        SetDataClientRpc();
    }

    [ClientRpc(RequireOwnership = false)]
    private void SetDataClientRpc()
    {
        if (IsOwner)
        {
            playerClass = StartClassSelector.Instance._hostData.userClass;
            previousClass = playerClass;
        }
        else if (!IsOwner)
        {
            playerClass = StartClassSelector.Instance._clientData.userClass;
            previousClass = playerClass;
        }
    }
    

    
}

