using System;
using System.Collections;
using System.Collections.Generic;
using EditorAttributes;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class PlayerClassSkill
{
    public Class playerClass;
    public List<Skill> skills;
}
[Serializable]
public class Skill
{
    public string skillName;
    public float skillCost;
    public UnityEvent skillEvent;

    public void ActiveSkill()
    {
        skillEvent?.Invoke();
    }
}
public class PlayerCombat : MonoBehaviour
{
    [Header("Combat Setting")] 
    [SerializeField] private float skillCooldown;
    private bool onSkillActive;
    
    [Header("Player Skill")] 
    [SerializeField] private int classNumber;
    [SerializeField] private List<PlayerClassSkill> playerSkill;
    private Class oldClass;
    
    [Header("Ref")] 
    private PlayerController _playerController;

    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
    }

    private void OnEnable()
    {
        ChangeClass();
    }
    
    private void Update()
    {
        if (oldClass != _playerController.PlayerClass)
        {
            ChangeClass();
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            if (_playerController.PlayerStamina > playerSkill[classNumber].skills[0].skillCost)
            {
                _playerController.DecreaseStamina(playerSkill[classNumber].skills[0].skillCost);
                playerSkill[classNumber].skills[0].ActiveSkill();
            }
            else
            {
                Debug.Log("no Stamina");
            }
            
        }
        else if (Input.GetMouseButtonDown(1))
        {
            if (_playerController.PlayerStamina > playerSkill[classNumber].skills[1].skillCost)
            {
                _playerController.DecreaseStamina(playerSkill[classNumber].skills[1].skillCost);
                playerSkill[classNumber].skills[1].ActiveSkill();
            }
            else
            {
                Debug.Log("no Stamina");
            }
        }
    }
    private void ChangeClass()
    {
        switch (_playerController.PlayerClass)
        {
            case Class.Sword:
                classNumber = 0;
                break;
            case Class.Shield:
                classNumber = 1;
                break;
        }
        oldClass = oldClass = _playerController.PlayerClass;
    }
    

    public void NormalSlash()
    {
        onSkillActive = true;
        Debug.Log("Slash");
    }
    public void SpecialSlash()
    {
        if (_playerController)
        {
            
        }
        Debug.Log("Special Slash");
    }
    
    public void ShieldSlam()
    {
        Debug.Log("Shield Slam");
    }
    
    public void RaiseShield()
    {
        Debug.Log("Raise Shield");
    }
}
