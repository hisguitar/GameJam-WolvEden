using EditorAttributes;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class Shield : MonoBehaviour
{
    [Header("Shield Setting")]
    [SerializeField] private float maxShieldHealth;
    [ReadOnly][SerializeField] private float shieldHealth;
    [SerializeField] private float shieldRegenTime;
    [SerializeField] private float shieldTime;
    
    private Collider2D _collider2D;
    private SpriteRenderer shieldSprite;
    private float shieldTimeCounter;
    private bool onActive;

    [Header("UI")] 
    [SerializeField] private GameObject shieldCanvas;
    [SerializeField] private Image shieldBar;

    [Header("Ref")] 
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private PlayerAnimationController _animationController;
    private Animator _animator;
    
    public bool OnActive { get { return onActive; } }
    private void OnEnable()
    {
        shieldHealth = maxShieldHealth;
        _collider2D = GetComponent<Collider2D>();
        shieldSprite = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        shieldBar.fillAmount = shieldHealth / maxShieldHealth;
        shieldSprite.material.SetFloat("_CustomFadeAlpha",shieldBar.fillAmount);
        

        if (_playerController.PlayerClass == Class.Shield)
        {
            shieldCanvas.SetActive(true);
        }
        else
        {
            shieldCanvas.SetActive(false);
        }

        if (shieldHealth < maxShieldHealth && !onActive)
        {
            shieldHealth += shieldRegenTime * Time.deltaTime;
            if (shieldHealth >= maxShieldHealth)
            {
                shieldHealth = maxShieldHealth;
            }
        }
        
        if (onActive)
        {
            shieldTimeCounter += Time.deltaTime;
            if (shieldTimeCounter >= shieldTime)
            {
                if (Input.GetMouseButton(1))
                {
                    return;
                }
                DeActiveShieldClientRpc();
            }
             
        }
       

    }

    [ClientRpc]
    public void ActiveShieldClientRpc()
    {
        _animator.SetBool("ShieldActive",true);
        _playerController.ReduceSpeed(_playerController.PlayerSpeed);
        _animationController.OnHoldAnimation();
        onActive = true;
        shieldSprite.enabled = true;
        _collider2D.enabled = true;
    }

    [ClientRpc]
    private void DeActiveShieldClientRpc()
    {
        _animator.SetBool("ShieldActive",false);
        _playerController.SetSpeedDefault();
        _animationController.UnHoldAnimation();
        onActive = false;
        shieldSprite.enabled = false;
        _collider2D.enabled = false;
        shieldTimeCounter = 0;
    }

    public void TakeDamage(float damageToShield)
    {
        shieldHealth -= damageToShield;
        if (shieldHealth <= 0)
        {
            DeActiveShieldClientRpc();
            shieldHealth = 0;
        }
    }

    
}
