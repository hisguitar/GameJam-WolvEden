using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerNumber : NetworkBehaviour
{
    public PlayerOne playerOne;
    public PlayerTwo playerTwo;
    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            DestroyPlayerTwoServerRpc();
        }
        else if (!IsOwner)
        {
            DestroyPlayerOneServerRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void DestroyPlayerOneServerRpc()
    {
        if (IsOwner)
        {
            return;
        }
        Destroy(playerOne);
        //DestroyPlayerOneClientRpc();
    }

    [ClientRpc(RequireOwnership = false)]
    private void DestroyPlayerOneClientRpc()
    {
        if (!IsOwner)
        {
            return;
        }
        Destroy(playerOne);
    }
    
    [ServerRpc(RequireOwnership = false)]
    private void DestroyPlayerTwoServerRpc()
    {
        if (!IsOwner)
        {
            return;
        }
        Destroy(playerTwo);
        //DestroyPlayerTwoClientRpc();
    }

    [ClientRpc(RequireOwnership = false)]
    private void DestroyPlayerTwoClientRpc()
    {
        if (IsOwner)
        {
            return;
        }
        Destroy(playerTwo);
    }
}
