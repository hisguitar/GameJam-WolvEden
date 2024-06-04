using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using Unity.Netcode;
using Unity.Services.Authentication;
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
    
    private NetworkServer _networkServer;
    private const string GameSceneName = "Game";
    public UserData playerOne, playerTwo;
    public int playerCount = 0;
    public Button startButton;
    public bool playerOneSelected, playerTwoSelected;
    
    private void Update()
    {
        if (!playerReady)
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

    [ServerRpc]
    private void SetupPlayerDataServerRpc()
    {
        playerOne = HostSingleton.Instance.GameManager.NetworkServer.GetUserDataByClientId(0);
        playerTwo = HostSingleton.Instance.GameManager.NetworkServer.GetUserDataByClientId(1);
        SetupPlayerDataClientRpc();
    }

    [ClientRpc]
    private void SetupPlayerDataClientRpc()
    {
        playerOne = HostSingleton.Instance.GameManager.NetworkServer.GetUserDataByClientId(0);
        playerTwo = HostSingleton.Instance.GameManager.NetworkServer.GetUserDataByClientId(1);
    }

    [ServerRpc]
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

    [ClientRpc]
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

    [ServerRpc]
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
        else
        {
            playerOneSelected = true;
            playerTwoSelected = true;
        }
        OnSelectedClientRpc();
    }
    [ClientRpc]
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
        else
        {
            playerOneSelected = true;
            playerTwoSelected = true;
        }
    }
    
    [ServerRpc]
    private void CheckPlayerInLobbyServerRpc()
    {
        WaitingPlayerTextServerRpc();
        if (playerOne.userClass == Class.Nobody && playerTwo.userClass == Class.Nobody)
        {
            waitingPlayerCanvas.SetActive(false);
            playerReady = true;
            CheckPlayerInLobbyClientRpc();
        }
    }

    [ClientRpc]
    private void CheckPlayerInLobbyClientRpc()
    {
        waitingPlayerCanvas.SetActive(false);
        playerReady = true;
    }

    [ServerRpc]
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

    [ClientRpc]
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
