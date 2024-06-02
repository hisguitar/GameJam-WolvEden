using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class ClassSelector : NetworkBehaviour
{
    [Header("Class")]
    [SerializeField] private Class classToSelect;
    [Space(2)]
    [Header("GUI")]
    [SerializeField] private Sprite playOneIcon;
    [SerializeField] private Sprite playTwoIcon;
    [SerializeField] private Image selectorIcon;
    [SerializeField] private Button selectorButton;
    [SerializeField] private TextMeshProUGUI classNameText;

    public override void OnNetworkSpawn()
    {
        classNameText.text = classToSelect.ToString();
    }

    [ServerRpc(RequireOwnership = false)]
    public void OnSelectServerRpc()
    {
        OnSelectClientRpc();
    }
    [ClientRpc(RequireOwnership = false)]
    public void OnSelectClientRpc()
    {
        int playerNumber = 1;
        if (IsOwner)
        {
            playerNumber = 1;
        }
        else if (!IsOwner)
        {
            playerNumber = 2;
        }
        
        selectorIcon.sprite = playerNumber == 1 ? playOneIcon : playTwoIcon;

        selectorIcon.gameObject.SetActive(true);
        selectorButton.interactable = false;
    }
    
    [ServerRpc(RequireOwnership = false)]
    public void SelectClassServerRpc()
    {
        HostSingleton.Instance.GameManager.NetworkServer.GetUserDataByClientId(OwnerClientId).userClass = classToSelect;
        
        SelectClassClientRpc();
    }
    
    [ClientRpc]
    private void SelectClassClientRpc()
    {
        HostSingleton.Instance.GameManager.NetworkServer.GetUserDataByClientId(OwnerClientId).userClass = classToSelect;
    }
}
