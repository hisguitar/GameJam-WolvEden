using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using Unity.Netcode;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ClassSelectManager : SingletonPersistent<ClassSelectManager>
{
    [Header("Lobby Setting")] 
    public TextMeshProUGUI waitingPlayerText;
    public GameObject waitingPlayerCanvas;
    public bool playerReady;
    public float selectingCounter = 6;
    public float selectingTimer = 0;
    
    private const string GameSceneName = "Game";
    public UserData playerOne;
    public UserData playerTwo;
    public int playerCount = 0;
    public Button startButton;
    public bool playerOneSelected, playerTwoSelected;
    private Lobby lobby;

    private void Start()
    {
        GetDataServerRpc();
    }

    [ServerRpc]
    private void GetDataServerRpc()
    {
        GetDataClientRpc();
    }

    [ClientRpc]
    private async void GetDataClientRpc()
    {
        lobby = await Lobbies.Instance.GetLobbyAsync(HostSingleton.Instance.GameManager.lobbyId);
        Debug.Log("Lobby Ok: " + lobby.LobbyCode);
    }
    private void Update()
    {
        Debug.Log("Lobby Player: " + lobby.Players.Count);
        if (lobby.Players.Count == lobby.MaxPlayers)
        {
            CheckPlayerInLobbyServerRpc();
        }
        OnSelectedServerRpc();
        SetupPlayerDataServerRpc();
        CheckButtonStartServerRpc();
    }

    public void StartGame()
    {
        NetworkManager.Singleton.SceneManager.LoadScene(GameSceneName, LoadSceneMode.Single);
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetupPlayerDataServerRpc()
    {
        playerOne = HostSingleton.Instance.GameManager.NetworkServer.GetUserDataByClientId(0);
        playerTwo = HostSingleton.Instance.GameManager.NetworkServer.GetUserDataByClientId(1);
        SetupPlayerDataClientRpc();
    }

    [ClientRpc(RequireOwnership = false)]
    private void SetupPlayerDataClientRpc()
    {
        playerOne = HostSingleton.Instance.GameManager.NetworkServer.GetUserDataByClientId(0);
        playerTwo = HostSingleton.Instance.GameManager.NetworkServer.GetUserDataByClientId(1);
    }

    [ServerRpc(RequireOwnership = false)]
    private void CheckButtonStartServerRpc()
    {
        if (playerOneSelected && playerTwoSelected)
        {
            startButton.interactable = true;
        }
        else
        {
            startButton.interactable = false;
        }
        CheckButtonStartClientRpc();
    }

    [ClientRpc(RequireOwnership = false)]
    private void CheckButtonStartClientRpc()
    {
        if (playerOneSelected && playerTwoSelected)
        {
            startButton.interactable = true;
        }
        else
        {
            startButton.interactable = false;
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void OnSelectedServerRpc()
    {
        if (playerCount == 1)
        {
            playerOneSelected = true;
        }
        else if (playerCount == 2)
        {
            playerTwoSelected = true;
        }
        else if (playerCount > 2)
        {
            playerOneSelected = true;
            playerTwoSelected = true;
        }
        OnSelectedClientRpc();
    }
    [ClientRpc(RequireOwnership = false)]
    private void OnSelectedClientRpc()
    {
        if (playerCount == 1)
        {
            playerOneSelected = true;
        }
        else if (playerCount == 2)
        {
            playerTwoSelected = true;
        }
        else if (playerCount > 2)
        {
            playerOneSelected = true;
            playerTwoSelected = true;
        }
    }
    
    [ServerRpc(RequireOwnership = false)]
    private void CheckPlayerInLobbyServerRpc()
    {
        waitingPlayerCanvas.SetActive(false);
        CheckPlayerInLobbyClientRpc();
    }

    [ClientRpc(RequireOwnership = false)]
    private void CheckPlayerInLobbyClientRpc()
    {
        waitingPlayerCanvas.SetActive(false);
        playerReady = true;
    }

    [ServerRpc(RequireOwnership = false)]
    private void WaitingPlayerTextServerRpc()
    {
        selectingTimer += Time.deltaTime;
        if (selectingTimer < 1.5F)
        {
            waitingPlayerText.text = "WAITING FOR OTHER PLAYER.";
        }
        else if (selectingTimer < 3)
        {
            waitingPlayerText.text = "WAITING FOR OTHER PLAYER..";
        }
        else if (selectingTimer < 4.5)
        {
            waitingPlayerText.text = "WAITING FOR OTHER PLAYER...";
        }
        else if (selectingTimer > selectingCounter)
        {
            selectingTimer = 0;
        }
        else
        {
            waitingPlayerText.text = "WAITING FOR OTHER PLAYER";
        }
        WaitingPlayerTextClientRpc();
    }

    [ClientRpc(RequireOwnership = false)]
    private void WaitingPlayerTextClientRpc()
    {
        selectingTimer += Time.deltaTime;
        if (selectingTimer < 1.5F)
        {
            waitingPlayerText.text = "WAITING FOR OTHER PLAYER.";
        }
        else if (selectingTimer < 3)
        {
            waitingPlayerText.text = "WAITING FOR OTHER PLAYER..";
        }
        else if (selectingTimer < 4.5)
        {
            waitingPlayerText.text = "WAITING FOR OTHER PLAYER...";
        }
        else if (selectingTimer > selectingCounter)
        {
            selectingTimer = 0;
        }
        else
        {
            waitingPlayerText.text = "WAITING FOR OTHER PLAYER";
        }
    }
    
    
}
