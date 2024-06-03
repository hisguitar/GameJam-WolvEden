using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class RespawnHandler : NetworkBehaviour
{
    [SerializeField] private Player playerPrefab;

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

    private IEnumerator RespawnPlayer(ulong ownerClientId)
    {
        yield return null;

        // Spawn Player
        Player playerInstance = Instantiate(
            playerPrefab, SpawnPoint.GetRandomSpawnPos(), Quaternion.identity);
        playerInstance.NetworkObject.SpawnAsPlayerObject(ownerClientId);

        if (ownerClientId == 0)
        {
            playerInstance.playerController.PlayerClass = ClassSelectManager.Instance.playerOne.userClass;
        }
        else
        {
            playerInstance.playerController.PlayerClass = ClassSelectManager.Instance.playerTwo.userClass;
        }
        
        playerInstance.SetWhenSpawnedClientRpc();
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
}