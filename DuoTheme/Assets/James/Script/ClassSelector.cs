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
        if (ClassSelectManager.instance.playerOne.userClass == classToSelect)
        {
            OnSelectClientRpc(1);
        }
        else if (ClassSelectManager.instance.playerTwo.userClass == classToSelect)
        {
            OnSelectClientRpc(2);
        }
        
    }
    [ClientRpc(RequireOwnership = false)]
    public void OnSelectClientRpc(int playerNumber)
    {
        if (playerNumber == 1)
        {
            selectorIcon.sprite = playOneIcon;
        }
        else
        {
            selectorIcon.sprite = playTwoIcon;
        }
        selectorIcon.gameObject.SetActive(true);
        selectorButton.interactable = false;
    }
    
    [ServerRpc(RequireOwnership = false)]
    public void SelectClassServerRpc()
    {
        if (IsOwnedByServer)
        {
            ClassSelectManager.instance.playerOne.userClass = classToSelect;
        }
        else
        {
            ClassSelectManager.instance.playerTwo.userClass = classToSelect;
        }
        
        SelectClassClientRpc();
        Debug.Log("Player Select: " + classToSelect);
    }
    
    [ClientRpc(RequireOwnership = false)]
    private void SelectClassClientRpc()
    {
        if (IsOwnedByServer)
        {
            ClassSelectManager.instance.playerOne.userClass = classToSelect;
            HostSingleton.Instance.GameManager.NetworkServer.GetUserDataByClientId(0).userClass = classToSelect;
        }
        else
        {
            ClassSelectManager.instance.playerTwo.userClass = classToSelect;
            HostSingleton.Instance.GameManager.NetworkServer.GetUserDataByClientId(1).userClass = classToSelect;
        }
    }
}
