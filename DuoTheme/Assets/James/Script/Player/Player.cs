using Cinemachine;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class Player : NetworkBehaviour
{
    // public NetworkVariable<FixedString32Bytes> PlayerName = new();

    [Header("References")]
    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    [Header("Settings")]
    [SerializeField] private int ownerPriority = 15;

    public override void OnNetworkSpawn()
    {
        //if (IsServer)
        //{
        //    UserData userData =
        //        HostSingleton.Instance.GameManager.NetworkServer.GetUserDataByClientId(OwnerClientId);

        //    PlayerName.Value = userData.userName;
        //}

        if (IsOwner)
        {
            virtualCamera.Priority = ownerPriority;
        }
    }
}