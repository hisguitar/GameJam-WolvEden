using James.Script;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public abstract class BossHealth : NetworkBehaviour
{
    [Header("Reference")]
    [SerializeField] private Image healthBar;
    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject clearPanel;

    [Header("Boss Target & Area")]
    public LayerMask PlayerLayer;
    public Vector2 AreaBossRadius { get; private set; } = new(11.3f, 7.84f);
    [SerializeField] [Tooltip("Offset of AreaBossRadius(Private variable)")] private Vector2 offSet = new(-0.52f, 13f);

    [Header("Boss HP")]
    [SerializeField] private float maxBossHealth = 1000f;
    [ReadOnly][SerializeField] private float bossHealth = 0f;

    private BossDummy bossDummy;
    private bool bossActive = false;

    public override void OnNetworkSpawn()
    {
        RestoreBossHpServerRpc();
        bossDummy = GetComponent<BossDummy>();
    }

    private void Update()
    {
        if (!IsOwner) {  return; }

        UpdateGUI();
        CheckPlayerInArea();
    }

    #region Boss Area
    private void CheckPlayerInArea()
    {
        if (bossActive)
        {
            return;
        }
        bool playerIn = Physics2D.OverlapBox(offSet, AreaBossRadius, 10, PlayerLayer);
        if (playerIn)
        {
            ActiveBoss();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(offSet, AreaBossRadius);
    }
    #endregion

    #region Active & Inactive
    public void ActiveBoss()
    {
        gameObject.SetActive(true);
        bossActive = true;
        RestoreBossHpClientRpc();
        _animator.SetBool("BossActive", true);

        if (bossDummy != null)
        {
            bossDummy.StartShooting();
        }
    }

    public void InactiveBoss()
    {
        //bossActive = false;
        //GetComponent<SpriteRenderer>().enabled = false;
        //_animator.SetBool("BossActive", false);

        //if (bossDummy != null)
        //{
        //    bossDummy.StopShooting();
        //}

        gameObject.SetActive(false);
        clearPanel.SetActive(true);
    }
    #endregion

    #region Update Boss-hp
    // Update UI
    private void UpdateGUI()
    {
        healthBar.fillAmount = bossHealth / maxBossHealth;
    }

    // Restore boss hp to max-hp
    [ServerRpc(RequireOwnership = false)]
    private void RestoreBossHpServerRpc()
    {
        bossHealth = maxBossHealth;
        RestoreBossHpClientRpc();
    }

    [ClientRpc]
    private void RestoreBossHpClientRpc()
    {
        bossHealth = maxBossHealth;
    }

    // TakeDamage to boss
    [ServerRpc(RequireOwnership = false)]
    public void TakeDamageServerRpc(float damage)
    {
        bossHealth -= damage;
        if (bossHealth < 0)
        {
            InactiveBoss();
            bossHealth = 0;
        }
        UpdateGUI();
        TakeDamageClientRpc(damage);
    }

    [ClientRpc]
    private void TakeDamageClientRpc(float damage)
    {
        if (IsOwner)
        {
            return;
        }
        bossHealth -= damage;
        if (bossHealth < 0)
        {
            InactiveBoss();
            bossHealth = 0;
        }
        UpdateGUI();
    }
    #endregion
}