using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using Unity.Services.Lobbies;
using UnityEngine;

public class CodeHUD : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI joinCodeText;
    public string joinCode;

    private void Start()
    {
       SetLobbyCodeServerRpc();
    }

    private void Update()
    {
        UpdateLobbyCodeServerRpc();
    }

    [ServerRpc]
    private void SetLobbyCodeServerRpc()
    {
        this.joinCode = HostSingleton.Instance.GameManager.joinCode;
        SetLobbyCodeClientRpc();
    }
    [ClientRpc]
    private void SetLobbyCodeClientRpc()
    {
        this.joinCode = HostSingleton.Instance.GameManager.joinCode;
    }
    
    [ServerRpc]
    private void UpdateLobbyCodeServerRpc()
    {
        joinCodeText.text = "Code\n" + joinCode;
        UpdateLobbyCodeClientRpc();
    }
    [ClientRpc]
    private void UpdateLobbyCodeClientRpc()
    {
        joinCodeText.text = "Code\n" + joinCode;
    }
}
