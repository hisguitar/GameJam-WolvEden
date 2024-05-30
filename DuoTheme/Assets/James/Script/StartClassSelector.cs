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
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
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
        NetworkManager.Singleton.SceneManager.LoadScene(GameSceneName, LoadSceneMode.Single);
        StartGameClientRpc();
    }

    [ClientRpc(RequireOwnership = false)]
    private void StartGameClientRpc()
    {
        NetworkManager.Singleton.SceneManager.LoadScene(GameSceneName, LoadSceneMode.Single);
    }

    [ServerRpc(RequireOwnership = false)]
    public void SelectClassSwordServerRpc()
    {
        swordSelected = true;
        UserData userData = HostSingleton.Instance.GameManager.NetworkServer.GetUserDataByClientId(OwnerClientId);
        swordText.text = userData.userName;
        userData.userClass = Class.Sword;
        Debug.Log(userData.userClass);
        SelectClassSwordClientRpc();
    }
    [ClientRpc(RequireOwnership = false)]
    private void SelectClassSwordClientRpc()
    {
        if (!IsOwner)
        {
            return;
        }
        swordSelected = true;
        UserData userData = HostSingleton.Instance.GameManager.NetworkServer.GetUserDataByClientId(OwnerClientId);
        swordText.text = userData.userName;
        userData.userClass = Class.Sword;
        Debug.Log(userData.userClass);
    }
    

    [ServerRpc(RequireOwnership = false)]
    public void SelectClassShieldServerRpc()
    {
        shieldSelected = true;
        UserData userData =
            HostSingleton.Instance.GameManager.NetworkServer.GetUserDataByClientId(OwnerClientId);
        shieldText.text = userData.userName;
        userData.userClass = Class.Shield;
        Debug.Log(userData.userClass);
    }
    [ClientRpc(RequireOwnership = false)]
    private void SelectClassShieldClientRpc()
    {
        if (!IsOwner)
        {
            return;
        }
        shieldSelected = true;
        UserData userData =
            HostSingleton.Instance.GameManager.NetworkServer.GetUserDataByClientId(OwnerClientId);
        shieldText.text = userData.userName;
        userData.userClass = Class.Shield;
        Debug.Log(userData.userClass);
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
