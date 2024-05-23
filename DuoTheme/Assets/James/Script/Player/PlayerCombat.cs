using System;
using System.Collections;
using System.Collections.Generic;
using EditorAttributes;
using UnityEngine;
using UnityEngine.Events;

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
    [SerializeField] private float skillCooldown;
    private bool onSkillActive;
    
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
        
        if (Input.GetMouseButtonDown(0) && !onSkillActive)
        {
            if (_playerController.PlayerStamina > playerSkill[(int)normalSkill].skillCost)
            {
                _playerController.DecreaseStamina(playerSkill[(int)normalSkill].skillCost);
                playerSkill[(int)normalSkill].ActiveSkill();
            }
            else
            {
                Debug.Log("no Stamina");
            }
            
        }
        else if (Input.GetMouseButtonDown(1) && !onSkillActive)
        {
            if (_playerController.PlayerStamina > playerSkill[(int)specialSkill].skillCost)
            {
                _playerController.DecreaseStamina(playerSkill[(int)specialSkill].skillCost);
                playerSkill[(int)specialSkill].ActiveSkill();
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
                normalSkill = SkillName.NormalSlash;
                specialSkill = SkillName.SpecialSlash;
                break;
            case Class.Shield:
                normalSkill = SkillName.ShieldSlab;
                specialSkill = SkillName.RaiseShield;
                break;
        }
        oldClass = oldClass = _playerController.PlayerClass;
    }
    

    
    public void NormalSlash()
    {
        onSkillActive = true;
        StartCoroutine(NormalSlashCoroutine());

    }
    IEnumerator NormalSlashCoroutine()
    {
        Debug.Log("Slash");
        yield return new WaitForSeconds(5);
        onSkillActive = false;
    }
    public void SpecialSlash()
    {
        onSkillActive = true;
        StartCoroutine(SpecialSlashCoroutine());
    }
    IEnumerator SpecialSlashCoroutine()
    {
        Debug.Log("Special Slash");
        yield return new WaitForSeconds(5);
        onSkillActive = false;
    }
    
    public void ShieldSlam()
    {
        onSkillActive = true;
        StartCoroutine(ShieldSlamCoroutine());
    }
    IEnumerator ShieldSlamCoroutine()
    {
        Debug.Log("ShieldSlam");
        yield return new WaitForSeconds(5);
        onSkillActive = false;
    }
    
    public void RaiseShield()
    {
        onSkillActive = true;
        StartCoroutine(RaiseShieldCoroutine());
    }
    IEnumerator RaiseShieldCoroutine()
    {
        Debug.Log("RaiseShield");
        yield return new WaitForSeconds(5);
        onSkillActive = false;
    }
}
