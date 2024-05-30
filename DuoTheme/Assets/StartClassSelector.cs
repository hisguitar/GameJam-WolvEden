using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class StartClassSelector : NetworkBehaviour
{
    [SerializeField] private Transform selectorCanvas;
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();
    }

    [ServerRpc]
    public void SelectClassServerRpc()
    {
        SelectClassClientRpc();
    }

    [ClientRpc]
    private void SelectClassClientRpc()
    {
        
    }
}
