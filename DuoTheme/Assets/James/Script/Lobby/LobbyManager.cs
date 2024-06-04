using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    [Header("Player Name Slot")] 
    public TextMeshProUGUI slotOneLoadingText;
    public TextMeshProUGUI slotTwoLoadingText;
    public float selectingCounter = 6;
    public float selectingTimer = 0;

    [Space(2)] 
    [Header("Slot one setting")]
    public GameObject swordSpriteOne;
    public GameObject shieldSpriteOne;
    
    [Space(2)] 
    [Header("Slot Two setting")]
    public GameObject swordSpriteTwo;
    public GameObject shieldSpriteTwo;
    

    private void Update()
    {
        /*if (ClassSelectManager.Instance.playerReady == false)
        {
            return;
        }*/
        SetPlayerSelectingServerRpc();
        CheckAllSlotServerRpc();
    }
    private void CheckAllSlot()
    {
        if (ClassSelectManager.Instance.playerOne.userClass == Class.Sword)
        {
            swordSpriteOne.SetActive(true);
            shieldSpriteOne.SetActive(false);
        }
        else if (ClassSelectManager.Instance.playerOne.userClass == Class.Shield)
        {
            swordSpriteOne.SetActive(false);
            shieldSpriteOne.SetActive(true);
        }
        else
        {
            swordSpriteOne.SetActive(false);
            shieldSpriteOne.SetActive(false);
        }
        
        if (ClassSelectManager.Instance.playerTwo.userClass == Class.Sword)
        {
            swordSpriteTwo.SetActive(true);
            shieldSpriteTwo.SetActive(false);
        }
        else if (ClassSelectManager.Instance.playerTwo.userClass == Class.Shield)
        {
            swordSpriteTwo.SetActive(false);
            shieldSpriteTwo.SetActive(true);
        }
        else
        {
            swordSpriteTwo.SetActive(false);
            shieldSpriteTwo.SetActive(false);
        }
    }
    [ServerRpc(RequireOwnership = false)]
    private void CheckAllSlotServerRpc()
    {
        CheckAllSlot();
        CheckAllSlotClientRpc();
    }

    [ClientRpc(RequireOwnership = false)]
    private void CheckAllSlotClientRpc()
    {
        CheckAllSlot();
    }

    private void SetPlayerSelecting()
    {
        if (ClassSelectManager.Instance.playerCount == 0)
        {
            slotOneLoadingText.gameObject.SetActive(true);
            slotTwoLoadingText.gameObject.SetActive(false);
            selectingTimer += Time.deltaTime;
            if (selectingTimer < 1.5F)
            {
                slotOneLoadingText.text = "PLAYER 1 SELECTING.";
            }
            else if (selectingTimer < 3)
            {
                slotOneLoadingText.text = "PLAYER 1 SELECTING..";
            }
            else if (selectingTimer < 4.5f)
            {
                slotOneLoadingText.text = "PLAYER 1 SELECTING...";
            }
            else if (selectingTimer > selectingCounter)
            {
                selectingTimer = 0;
            }
            else
            {
                slotOneLoadingText.text = "PLAYER 1 SELECTING";
            }
        }
        else if (ClassSelectManager.Instance.playerCount == 1)
        {
            slotTwoLoadingText.gameObject.SetActive(true);
            slotOneLoadingText.gameObject.SetActive(false);
            selectingTimer += Time.deltaTime;
            if (selectingTimer < 1.5F)
            {
                slotTwoLoadingText.text = "PLAYER 2 SELECTING.";
            }
            else if (selectingTimer < 3)
            {
                slotTwoLoadingText.text = "PLAYER 2 SELECTING..";
            }
            else if (selectingTimer < 4.5f)
            {
                slotTwoLoadingText.text = "PLAYER 2 SELECTING...";
            }
            else if (selectingTimer > selectingCounter)
            {
                selectingTimer = 0;
            }
            else
            {
                slotTwoLoadingText.text = "PLAYER 2 SELECTING";
            }
        }
        else
        {
            slotOneLoadingText.gameObject.SetActive(false);
            slotTwoLoadingText.gameObject.SetActive(false);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetPlayerSelectingServerRpc()
    {
        SetPlayerSelecting();
        SetPlayerSelectingClientRpc();
    }

    [ClientRpc(RequireOwnership = false)]
    private void SetPlayerSelectingClientRpc()
    {
        SetPlayerSelecting();
    }

    
    
    
}
