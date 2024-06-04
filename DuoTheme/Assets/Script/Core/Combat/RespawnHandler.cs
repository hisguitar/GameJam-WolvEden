using System;
using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class RespawnHandler : SingletonNetwork<RespawnHandler>
{
    [SerializeField] private Player playerPrefab;
    [SerializeField] private GameObject deadPanel;
    public int dieCount;
    public int dieLimit = 2;

    public override void OnNetworkSpawn()
    {
        if (!IsServer) { return; }
        Player[] players = FindObjectsByType<Player>(FindObjectsSortMode.None);
        foreach (Player player in players)
        {
            HandlePlayerSpawned(player);
        }
        StartCoroutine(RespawnPlayer(0));
        StartCoroutine(RespawnPlayer(1));
        Player.OnPlayerSpawned += HandlePlayerSpawned;
        Player.OnPlayerDespawned += HandlePlayerDespawned;
    }

    public override void OnNetworkDespawn()
    {
        if (!IsServer) { return; }
        Player.OnPlayerSpawned -= HandlePlayerSpawned;
        Player.OnPlayerDespawned -= HandlePlayerDespawned;
    }

    private void HandlePlayerSpawned(Player player)
    {
        player.playerController.OnDie += (health) => HandlePlayerDie(player);
    }

    private void HandlePlayerDespawned(Player player)
    {
        player.playerController.OnDie -= (health) => HandlePlayerDie(player);
    }

    private void HandlePlayerDie(Player player)
    {
        Destroy(player.gameObject);

        StartCoroutine(RespawnPlayer(player.OwnerClientId));
    }

    private void Update()
    {
        CheckDeadServerRpc();
    }

    private IEnumerator RespawnPlayer(ulong ownerClientId)
    {
        yield return null;

        // Spawn Player
        Player playerInstance = Instantiate(
            playerPrefab, SpawnPoint.GetRandomSpawnPos(), Quaternion.identity);
        playerInstance.NetworkObject.SpawnAsPlayerObject(ownerClientId);
        playerInstance.SetWhenSpawnedServerRpc();
    }

    public void MoveAllPlayersToSpawnPoints()
    {
        if (!IsServer) { return; }

        Player[] players = FindObjectsByType<Player>(FindObjectsSortMode.None);
        foreach (Player player in players)
        {
            Vector3 spawnPosition = SpawnPoint.GetRandomSpawnPos();
            player.transform.position = spawnPosition;
            // Optionally reset rotation or other parameters
            player.transform.rotation = Quaternion.identity;
        }
    }
    [ServerRpc]
    public void CheckDeadServerRpc()
    {
        if (dieCount >= dieLimit)
        {
            deadPanel.SetActive(true);
        }
        CheckDeadClientRpc();
    }
    [ClientRpc]
    public void CheckDeadClientRpc()
    {
        if (dieCount >= dieLimit)
        {
            deadPanel.SetActive(true);
        }
    }

    [ServerRpc]
    public void AddDieCountServerRpc()
    {
        dieCount += 1;
        AddDieCountClientRpc();
    }
    [ClientRpc]
    public void AddDieCountClientRpc()
    {
        if (IsOwner)
        {
            return;
        }

        dieCount += 1;
    }
}