using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Stats", menuName = "Player/Stats")]
public class PlayerStats : ScriptableObject
{
       public float playerHealth;
       public float playerStamina;
       public float playerSpeed;
}

