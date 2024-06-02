using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.Netcode;
using Unity.Services.Authentication;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ClassSelect : NetworkBehaviour
{
    private NetworkServer _networkServer;
    private const string GameSceneName = "Game";
    public Button swordButton,shieldButton;
    private ulong classCount = 0;
    
    public void StartGame()
    {
        NetworkManager.Singleton.SceneManager.LoadScene(GameSceneName, LoadSceneMode.Single);
        SetVisibleMouseClientRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    public void SelectSwordClassServerRpc()
    {
        HostSingleton.Instance.GameManager.NetworkServer.GetUserDataByClientId(classCount).userClass = Class.Sword;
        classCount += 1;
        SelectSwordClassClientRpc();
    }
    
    [ClientRpc(RequireOwnership = false)]
    private void SelectSwordClassClientRpc()
    {
        swordButton.interactable = false;
        /*if (!IsOwner)
        {
            Cursor.visible = false;
        }*/
    }
    
    [ServerRpc(RequireOwnership = false)]
    public void SelectShieldClassServerRpc()
    {
        HostSingleton.Instance.GameManager.NetworkServer.GetUserDataByClientId(classCount).userClass = Class.Shield;
        classCount += 1;
        SelectShieldClassClientRpc();
    }
    
    [ClientRpc(RequireOwnership = false)]
    private void SelectShieldClassClientRpc()
    {
        shieldButton.interactable = false;
        /*if (!IsOwner)
        {
            Cursor.visible = false;
        }*/
    }

    [ClientRpc(RequireOwnership = false)]
    private void SetVisibleMouseClientRpc()
    {
        Cursor.visible = true;
    }
}
