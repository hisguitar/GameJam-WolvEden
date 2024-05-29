using Cinemachine;
using System;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class Player : NetworkBehaviour
{
    [Header("Player")]
    public NetworkVariable<FixedString32Bytes> PlayerName = new();
    [field: SerializeField] public PlayerController Health { get; private set; }

    [Header("Camera")]
    [SerializeField] private int ownerPriority = 15;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    public static event Action<Player> OnPlayerSpawned;
    public static event Action<Player> OnPlayerDespawned;

    // OnNetworkSpawn is used when an object begins network connection.
    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            UserData userData =
                HostSingleton.Instance.GameManager.NetworkServer.GetUserDataByClientId(OwnerClientId);

            /// If userData is null,
            /// it is possible that ApprovalCheck() in NetworkServer Not activated,
            /// the solution is to go to NetBootstrap scene
            /// and tick Connection Approval of NetworkManager to True.
            PlayerName.Value = userData.userName;

            OnPlayerSpawned?.Invoke(this);
        }

        if (IsOwner)
        {
            virtualCamera.Priority = ownerPriority;
        }
    }

    public override void OnNetworkDespawn()
    {
        if (IsServer)
        {
            OnPlayerDespawned?.Invoke(this);
        }
    }
}