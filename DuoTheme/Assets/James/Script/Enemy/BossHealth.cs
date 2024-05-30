using James.Script;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public abstract class BossHealth : NetworkBehaviour
{
    [SerializeField] private float maxBossHealth = 1000f;
    [ReadOnly][SerializeField] private float bossHealth = 0f;
    [SerializeField] private Image healthBar;
    [SerializeField] private Vector2 offSet = new(-0.52f, 13f);
    public Vector2 areaBossRadius = new(11.3f, 7.84f);
    public LayerMask playerLayer;
    [SerializeField] private Animator _animator;

    private BossDummy bossDummy;
    private bool bossActive = false;

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
