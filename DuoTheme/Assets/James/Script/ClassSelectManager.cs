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
    
    private void Update()
    {
        playerOne = HostSingleton.Instance.GameManager.NetworkServer.GetUserDataByClientId(0);
        playerTwo = HostSingleton.Instance.GameManager.NetworkServer.GetUserDataByClientId(1);
    }

    public void StartGame()
    {
        NetworkManager.Singleton.SceneManager.LoadScene(GameSceneName, LoadSceneMode.Single);
    }
    
}
