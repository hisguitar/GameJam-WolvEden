using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartClassSelector : NetworkBehaviour
{
    private static StartClassSelector instance;
    [SerializeField] private GameObject selectorCanvas;
    [SerializeField] private Button startButton;
    [Space(2)]
    [Header("Sword Class")] 
    [SerializeField] private TextMeshProUGUI swordText;
    [SerializeField] private Button swordButton;
    [SerializeField] private GameObject acSwordUi;
    [SerializeField] private GameObject unSwordUi;

    [Space(2)] 
    [Header("Shield Class")] 
    [SerializeField] private TextMeshProUGUI shieldText;
    [SerializeField] private Button shieldButton;
    [SerializeField] private GameObject acShieldUi;
    [SerializeField] private GameObject unShieldUi;
    public bool selectedSuccess;
    private const string GameSceneName = "Game";
    private bool swordSelected, shieldSelected;

    public UserData _hostData = new UserData();
    private bool hostSelected;
    public UserData _clientData = new UserData();

    public static StartClassSelector Instance 
    {
        get
        {
            if (instance != null) { return instance; }
            instance = FindFirstObjectByType<StartClassSelector>();

            if (instance == null)
            {
                // Debug.LogError("No HostSingleton in the scene!");
                return null;
            }
            return instance;
        }
    }

    public override void OnNetworkSpawn()
    {
        SetDataServerRpc();
        DontDestroyOnLoad(gameObject);
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();
    }

    private void Update()
    {
        if (swordSelected && shieldSelected)
        {
            StartInteractableServerRpc();
        }
        else
        {
            UnStartInteractableServerRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetDataServerRpc()
    {
        _hostData.userName = "P1";
        _clientData.userName = "P2";
        SetDataClientRpc();
    }
    
    [ClientRpc(RequireOwnership = false)]
    private void SetDataClientRpc()
    {
        _hostData.userName = "P1";
        _clientData.userName = "P2";
    }
    

    [ServerRpc(RequireOwnership = false)]
    private void StartInteractableServerRpc()
    {
        startButton.interactable = true;
        StartInteractableClientRpc();
    }

    [ClientRpc(RequireOwnership = false)]
    private void StartInteractableClientRpc()
    {
        startButton.interactable = true;
    }
    
    [ServerRpc(RequireOwnership = false)]
    private void UnStartInteractableServerRpc()
    {
        startButton.interactable = false;
        UnStartInteractableClientRpc();
    }

    [ClientRpc(RequireOwnership = false)]
    private void UnStartInteractableClientRpc()
    {
        startButton.interactable = false;
    }

    [ServerRpc(RequireOwnership = false)]
    public void StartGameServerRpc()
    {
        selectorCanvas.SetActive(false);
        PlayerOne.Instance.playerController.ResetStatsServerRpc();
        PlayerTwo.Instance.playerController.ResetStatsServerRpc();
        StartGameClientRpc();
        if (IsServer)
        {
            NetworkManager.Singleton.SceneManager.LoadScene(GameSceneName, LoadSceneMode.Single);
        }
    }

    [ClientRpc(RequireOwnership = false)]
    private void StartGameClientRpc()
    {
        selectorCanvas.SetActive(false);
        PlayerOne.Instance.playerController.ResetStatsServerRpc();
        PlayerTwo.Instance.playerController.ResetStatsServerRpc();
        StartGameClientRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    public void SelectClassSwordServerRpc()
    {
        swordSelected = true;
        if (!hostSelected)
        {
            string playerName  = _hostData.userName;
            swordText.text = playerName;
            _hostData.userClass = Class.Sword;
            hostSelected = true;
            SelectClassSwordOwnerClientRpc(playerName);
        }
        else if (hostSelected)
        {
            string playerName  = _clientData.userName;
            swordText.text = playerName;
            _clientData.userClass = Class.Sword;
            SelectClassSwordNotOwnerClientRpc(playerName);
        }
    }
    [ClientRpc(RequireOwnership = false)]
    private void SelectClassSwordOwnerClientRpc(string namePlayer)
    {
        swordSelected = true;
        swordText.text = namePlayer;
        _hostData.userClass = Class.Sword;
        hostSelected = true;
    }
    [ClientRpc(RequireOwnership = false)]
    private void SelectClassSwordNotOwnerClientRpc(string namePlayer)
    {
        swordSelected = true;
        swordText.text = namePlayer;
        _clientData.userClass = Class.Sword;
    }
    

    [ServerRpc(RequireOwnership = false)]
    public void SelectClassShieldServerRpc()
    {
        shieldSelected = true;
        if (!hostSelected)
        {
            string playerName  = _hostData.userName;
            shieldText.text = playerName;
            _hostData.userClass = Class.Shield;
            hostSelected = true;
            SelectClassShieldOwnerClientRpc(playerName);
        }
        else if (hostSelected)
        {
            string playerName  = _clientData.userName;
            shieldText.text = playerName;
            _clientData.userClass = Class.Shield;
            SelectClassShieldNotOwnerClientRpc(playerName);
        }
    }
    [ClientRpc(RequireOwnership = false)]
    private void SelectClassShieldOwnerClientRpc(string namePlayer)
    {
        shieldSelected = true;
        shieldText.text = namePlayer;
        _hostData.userClass = Class.Shield;
        hostSelected = true;
    }
    [ClientRpc(RequireOwnership = false)]
    private void SelectClassShieldNotOwnerClientRpc(string namePlayer)
    {
        shieldSelected = true;
        shieldText.text = namePlayer;
        _clientData.userClass = Class.Shield;
    }
    
    
    [ServerRpc(RequireOwnership = false)]
    public void SetActiveSwordUIServerRpc()
    {
        unSwordUi.SetActive(true);
        SetActiveSwordUIClientRpc();
    }
    [ClientRpc(RequireOwnership = false)]
    private void SetActiveSwordUIClientRpc()
    {
        unSwordUi.SetActive(true);
    }
    
    [ServerRpc(RequireOwnership = false)]
    public void SetActiveShieldUIServerRpc()
    {
        unShieldUi.SetActive(true);
        SetActiveShieldUIClientRpc();
    }
    [ClientRpc(RequireOwnership = false)]
    private void SetActiveShieldUIClientRpc()
    {
        unShieldUi.SetActive(true);
    }
    [ServerRpc(RequireOwnership = false)]
    public void DeActiveSwordUIServerRpc()
    {
        acSwordUi.SetActive(false);
        DeActiveSwordUIClientRpc();
    }
    [ClientRpc(RequireOwnership = false)]
    private void DeActiveSwordUIClientRpc()
    {
        acSwordUi.SetActive(false);
    }
    
    [ServerRpc(RequireOwnership = false)]
    public void DeActiveShieldUIServerRpc()
    {
        acShieldUi.SetActive(false);
        DeActiveShieldUIClientRpc();
    }
    [ClientRpc(RequireOwnership = false)]
    private void DeActiveShieldUIClientRpc()
    {
        acShieldUi.SetActive(false);
    }
    
}
