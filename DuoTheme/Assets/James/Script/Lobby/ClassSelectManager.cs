using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.Netcode;
using Unity.Services.Authentication;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ClassSelectManager : SingletonPersistent<ClassSelectManager>
{
    private NetworkServer _networkServer;
    private const string GameSceneName = "Game";
    public UserData playerOne, playerTwo;
    public int playerCount = 0;
    public Button startButton;
    public bool playerOneSelected, playerTwoSelected;
    
    private void Update()
    {
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
    
}
