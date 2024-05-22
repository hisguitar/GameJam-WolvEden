using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EditorAttributes;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private float playerHealth;
    [SerializeField] private float playerStamina;
    [SerializeField] private float playerSpeed;

    public float PlayerSpeed
    {
        get { return playerSpeed; }
    }

    [Button("Reset To Default")]
    public void ResetStats()
    {
        playerHealth = playerStats.playerHealth;
        playerStamina = playerStats.playerStamina;
        playerSpeed = playerStats.playerSpeed;
    }
}

