using System;
using System.Collections;
using System.Collections.Generic;
using EditorAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[Serializable]
public class Skill
{
    public string skillName;
    public float skillCost;
    public float skillCooldown;
    public SpriteRenderer skillIcon;
    public UnityEvent skillEvent;

    public void ActiveSkill()
    {
        skillEvent?.Invoke();
    }
}

public enum SkillName
{
    NormalSlash = 0,
    SpecialSlash = 1,
    ShieldSlab = 2,
    RaiseShield = 3,
}
public class PlayerCombat : MonoBehaviour
{
    [Header("Combat Setting")] 
    [SerializeField] private Transform skillPosition;
    [SerializeField] private SkillName normalSkill;
    [SerializeField] private SkillName specialSkill;
    
    [Header("Cooldown")]
    [SerializeField] private float normalSkillCooldown,specialSkillCooldown;
    
    [Header("Image")]
    [SerializeField] private Image normalSkillImage,normalSkillImageCD, specialSkillImage,specialSkillImageCD;
    private bool onNormalSkillActive,onSpecialSkillActive;
    private float lastNormalSkillCooldown, lastSpecialSkillCooldown;
    
    [Header("Player Skill")] 
    [SerializeField] private List<Skill> playerSkill;
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
        
        CheckCooldown();
        
        if (Input.GetMouseButtonDown(0) && !onNormalSkillActive)
        {
            if (_playerController.PlayerStamina > playerSkill[(int)normalSkill].skillCost)
            {
                _playerController.DecreaseStamina(playerSkill[(int)normalSkill].skillCost);
                playerSkill[(int)normalSkill].ActiveSkill();
                normalSkillCooldown = playerSkill[(int)normalSkill].skillCooldown;
                lastNormalSkillCooldown = playerSkill[(int)normalSkill].skillCooldown;
            }
            else
            {
                Debug.Log("no Stamina");
            }
            
        }
        else if (Input.GetMouseButtonDown(1) && !onSpecialSkillActive)
        {
            if (_playerController.PlayerStamina > playerSkill[(int)specialSkill].skillCost)
            {
                _playerController.DecreaseStamina(playerSkill[(int)specialSkill].skillCost);
                playerSkill[(int)specialSkill].ActiveSkill();
                specialSkillCooldown = playerSkill[(int)specialSkill].skillCooldown;
                lastSpecialSkillCooldown = playerSkill[(int)specialSkill].skillCooldown;
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
                normalSkill = _playerController.PlayerStats.normalSkill;
                specialSkill = _playerController.PlayerStats.specialSkill;
                normalSkillImage.sprite = playerSkill[(int)normalSkill].skillIcon.GetComponent<SpriteRenderer>().sprite;
                normalSkillImageCD.sprite = playerSkill[(int)normalSkill].skillIcon.GetComponent<SpriteRenderer>().sprite;
                specialSkillImage.sprite = playerSkill[(int)specialSkill].skillIcon.GetComponent<SpriteRenderer>().sprite;
                specialSkillImageCD.sprite = playerSkill[(int)specialSkill].skillIcon.GetComponent<SpriteRenderer>().sprite;
                break;
            case Class.Shield:
                normalSkill = SkillName.ShieldSlab;
                specialSkill = SkillName.RaiseShield;
                normalSkillImage.sprite = playerSkill[(int)normalSkill].skillIcon.sprite;
                normalSkillImageCD.sprite = playerSkill[(int)normalSkill].skillIcon.sprite;
                specialSkillImage.sprite = playerSkill[(int)specialSkill].skillIcon.sprite;
                specialSkillImageCD.sprite = playerSkill[(int)specialSkill].skillIcon.sprite;
                break;
        }
        oldClass = oldClass = _playerController.PlayerClass;
    }

    private void CheckCooldown()
    {
        if (normalSkillCooldown > 0)
        {
            normalSkillImageCD.fillAmount = normalSkillCooldown / lastNormalSkillCooldown;
            normalSkillCooldown -= Time.deltaTime;
            if (normalSkillCooldown < 0)
            {
                normalSkillCooldown = 0;
                onNormalSkillActive = false;
            }
        }

        if (specialSkillCooldown > 0)
        {
            specialSkillImageCD.fillAmount = specialSkillCooldown / lastSpecialSkillCooldown;
            specialSkillCooldown -= Time.deltaTime;
            if (specialSkillCooldown < 0)
            {
                specialSkillCooldown = 0;
                onSpecialSkillActive = false;
            }
        }
    }

    
    public void NormalSlash()
    {
        
    }
    public void SpecialSlash()
    {
        
    }
    
    public void ShieldSlam()
    {
        
    }
    
    public void RaiseShield()
    {
        
    }
}
