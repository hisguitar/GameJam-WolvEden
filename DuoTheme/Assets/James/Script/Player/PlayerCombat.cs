using System;
using System.Collections;
using System.Collections.Generic;
using EditorAttributes;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[Serializable]
public class Skill
{
    public string skillName;
    public float skillDamage;
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

public class PlayerCombat : NetworkBehaviour
{
    [Header("Combat Setting")] [SerializeField]
    private LayerMask enemyLayer;

    [SerializeField] private float skillRadius;
    [SerializeField] private Transform skillPosition;
    //[SerializeField] private Transform skillRotation;
    [SerializeField] private SkillName normalSkill;
    [SerializeField] private SkillName specialSkill;
    [SerializeField] private Shield shieldObject;
    [SerializeField] private SpecialSlash slashObject;
    

    [Header("Cooldown")] [SerializeField] private float normalSkillCooldown, specialSkillCooldown;

    [Header("Image")] [SerializeField]
    private Image normalSkillImage, normalSkillImageCD, specialSkillImage, specialSkillImageCD;

    private bool onNormalSkillActive, onSpecialSkillActive;
    private float lastNormalSkillCooldown, lastSpecialSkillCooldown;

    [Header("Player Skill")] [SerializeField]
    private List<Skill> playerSkill;

    private Class oldClass;

    [Header("Ref")] private PlayerController _playerController;
    private PlayerAnimationController _animationController;

    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
        _animationController = GetComponent<PlayerAnimationController>();
    }

    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            return;
        }

        ChangeClass();
    }

    /*private void OnEnable()
    {
        ChangeClass();
    }*/

    private void Update()
    {
        if (!IsOwner)
        {
            return;
        }

        if (_playerController.IsDead)
        {
            return;
        }

        if (oldClass != _playerController.PlayerClass)
        {
            ChangeClass();
        }

        CheckCooldown();

        if (Input.GetMouseButtonDown(0) && !onNormalSkillActive)
        {
            if (_playerController.PlayerStamina > playerSkill[(int)normalSkill].skillCost)
            {
                SoundManager.Instance.Play(SoundManager.SoundName.Slash);
                onNormalSkillActive = true;
                _playerController.DecreaseStamina(playerSkill[(int)normalSkill].skillCost);
                playerSkill[(int)normalSkill].ActiveSkill();
                normalSkillCooldown = playerSkill[(int)normalSkill].skillCooldown;
                lastNormalSkillCooldown = playerSkill[(int)normalSkill].skillCooldown;
                _animationController.AttackAnimation("NormalAttack");
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
                SoundManager.Instance.Play(SoundManager.SoundName.WhooshSlash);
                onSpecialSkillActive = true;
                _playerController.DecreaseStamina(playerSkill[(int)specialSkill].skillCost);
                playerSkill[(int)specialSkill].ActiveSkill();
                specialSkillCooldown = playerSkill[(int)specialSkill].skillCooldown;
                lastSpecialSkillCooldown = playerSkill[(int)specialSkill].skillCooldown;
                _animationController.AttackAnimation("SpecialAttack");
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
                normalSkillImageCD.sprite =
                    playerSkill[(int)normalSkill].skillIcon.GetComponent<SpriteRenderer>().sprite;
                specialSkillImage.sprite =
                    playerSkill[(int)specialSkill].skillIcon.GetComponent<SpriteRenderer>().sprite;
                specialSkillImageCD.sprite =
                    playerSkill[(int)specialSkill].skillIcon.GetComponent<SpriteRenderer>().sprite;
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
        /*GameObject shieldObject = Instantiate(slash, skillPosition.position, skillPosition.rotation, skillPosition);
        Destroy(shieldObject,playerSkill[(int)normalSkill].skillCooldown - 0.05f);*/

        Collider2D[] enemy = Physics2D.OverlapCircleAll(skillPosition.position, skillRadius, enemyLayer);
        foreach (var enemies in enemy)
        {
            if (enemies.CompareTag("Enemy"))
            {
                enemies.GetComponent<BossHealth>().TakeDamageServerRpc(playerSkill[(int)normalSkill].skillDamage);
            }
        }
    }
    
    public void SpecialSlash()
    {
        if (slashObject.OnActive == false)
        {
            slashObject.ActiveSlashClientRpc();
            slashObject.SetDamage(playerSkill[(int)specialSkill].skillDamage);
        }
    }
    

public void ShieldSlam()
    {
        Collider2D[] enemy = Physics2D.OverlapCircleAll(skillPosition.position, skillRadius, enemyLayer);
        foreach (var enemies in enemy)
        {
            if (enemies.CompareTag("Enemy"))
            {
                enemies.GetComponent<BossHealth>().TakeDamageServerRpc(playerSkill[(int)normalSkill].skillDamage);
            }
        }
    }
    
    public void RaiseShield()
    {
        if (shieldObject.OnActive == false)
        {
            shieldObject.ActiveShieldClientRpc();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(skillPosition.position,skillRadius);
    }
}
