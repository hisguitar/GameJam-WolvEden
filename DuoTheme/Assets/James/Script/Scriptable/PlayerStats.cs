using System.Collections;
using System.Collections.Generic;
using EditorAttributesSamples;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "New Stats", menuName = "Player/Stats")]
public class PlayerStats : ScriptableObject
{
       public Material playerMaterial;
       public Class playerClass;
       public float playerMaxHealth;
       public float playerMaxStamina;
       public float playerSpeed;
       public SkillName normalSkill;
       public SkillName specialSkill;
       public RuntimeAnimatorController animation;
}

