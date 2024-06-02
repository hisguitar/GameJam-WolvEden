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

    public void StartGame()
    {
        NetworkManager.Singleton.SceneManager.LoadScene(GameSceneName, LoadSceneMode.Single);
        //SetVisibleMouseClientRpc();
    }
    
}
