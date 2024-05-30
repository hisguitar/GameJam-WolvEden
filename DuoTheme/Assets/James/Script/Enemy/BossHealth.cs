using James.Script;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public abstract class BossHealth : NetworkBehaviour
{
    [SerializeField] private float maxBossHealth;
    [ReadOnly][SerializeField] private float bossHealth;
    [SerializeField] private Image healthBar;
    [SerializeField] private Vector2 offSet;
    public Vector2 areaBossRadius;
    public LayerMask playerLayer;
    [SerializeField] private Animator _animator;
    [SerializeField] private bool bossActive;

    private BossDummy bossDummy;

    public override void OnNetworkSpawn()
    {
        RestoreBossHpServerRpc();
        bossDummy = GetComponent<BossDummy>();
    }

    private void Update()
    {
        if (!IsOwner)
        {
            return;
        }

        UpdateGUI();
        CheckPlayerInArea();
    }

    private void UpdateGUI()
    {
        healthBar.fillAmount = bossHealth / maxBossHealth;
    }

    private void CheckPlayerInArea()
    {
        if (bossActive)
        {
            return;
        }
        bool playerIn = Physics2D.OverlapBox(offSet, areaBossRadius, 10, playerLayer);
        if (playerIn)
        {
            ActiveBoss();
        }
    }

    private void ActiveBoss()
    {
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
    }

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

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(offSet, areaBossRadius);
    }
}
