using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    [Header("Player Name Slot")] 
    public TextMeshProUGUI slotOneLoadingText;
    public TextMeshProUGUI slotTwoLoadingText;
    private float selectingCounter = 6;
    private float selectingTimer = 0;

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
        SetPlayerSelectingServerRpc();
        CheckAllSlotServerRpc();
    }

    [ServerRpc]
    private void CheckAllSlotServerRpc()
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
        
        CheckAllSlotClientRpc();
    }

    [ClientRpc]
    private void CheckAllSlotClientRpc()
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

    [ServerRpc]
    private void SetPlayerSelectingServerRpc()
    {
        if (ClassSelectManager.Instance.playerCount == 0)
        {
            selectingTimer += Time.deltaTime;
            if (selectingTimer > 0)
            {
                slotOneLoadingText.text = "PLAYER 1 SELECTING";
            }
            else if (selectingTimer > 1.5F)
            {
                slotOneLoadingText.text = "PLAYER 1 SELECTING.";
            }
            else if (selectingTimer > 3)
            {
                slotOneLoadingText.text = "PLAYER 1 SELECTING..";
            }
            else if (selectingCounter > 4.5)
            {
                slotOneLoadingText.text = "PLAYER 1 SELECTING...";
            }
            else if (selectingTimer > selectingCounter)
            {
                selectingTimer = 0;
            }
        }
        else if (ClassSelectManager.Instance.playerCount == 1)
        {
            slotOneLoadingText.gameObject.SetActive(false);
            selectingTimer += Time.deltaTime;
            if (selectingTimer > 0)
            {
                slotTwoLoadingText.text = "PLAYER 2 SELECTING";
            }
            else if (selectingTimer > 1.5F)
            {
                slotTwoLoadingText.text = "PLAYER 2 SELECTING.";
            }
            else if (selectingTimer > 3)
            {
                slotTwoLoadingText.text = "PLAYER 2 SELECTING..";
            }
            else if (selectingCounter > 4.5)
            {
                slotTwoLoadingText.text = "PLAYER 2 SELECTING...";
            }
            else if (selectingTimer > selectingCounter)
            {
                selectingTimer = 0;
            }
        }
        else
        {
            slotOneLoadingText.gameObject.SetActive(false);
            slotTwoLoadingText.gameObject.SetActive(false);
        }
        SetPlayerSelectingClientRpc();
    }

    [ClientRpc]
    private void SetPlayerSelectingClientRpc()
    {
        if (ClassSelectManager.Instance.playerCount == 0)
        {
            slotOneLoadingText.gameObject.SetActive(true);
            slotTwoLoadingText.gameObject.SetActive(false);
            selectingTimer += Time.deltaTime;
            if (selectingTimer > 0)
            {
                slotOneLoadingText.text = "PLAYER 1 SELECTING";
            }
            else if (selectingTimer > 1.5F)
            {
                slotOneLoadingText.text = "PLAYER 1 SELECTING.";
            }
            else if (selectingTimer > 3)
            {
                slotOneLoadingText.text = "PLAYER 1 SELECTING..";
            }
            else if (selectingCounter > 4.5)
            {
                slotOneLoadingText.text = "PLAYER 1 SELECTING...";
            }
            else if (selectingTimer > selectingCounter)
            {
                selectingTimer = 0;
            }
        }
        else if (ClassSelectManager.Instance.playerCount == 1)
        {
            slotTwoLoadingText.gameObject.SetActive(true);
            slotOneLoadingText.gameObject.SetActive(false);
            selectingTimer += Time.deltaTime;
            if (selectingTimer > 0)
            {
                slotTwoLoadingText.text = "PLAYER 2 SELECTING";
            }
            else if (selectingTimer > 1.5F)
            {
                slotTwoLoadingText.text = "PLAYER 2 SELECTING.";
            }
            else if (selectingTimer > 3)
            {
                slotTwoLoadingText.text = "PLAYER 2 SELECTING..";
            }
            else if (selectingCounter > 4.5)
            {
                slotTwoLoadingText.text = "PLAYER 2 SELECTING...";
            }
            else if (selectingTimer > selectingCounter)
            {
                selectingTimer = 0;
            }
        }
        else
        {
            slotOneLoadingText.gameObject.SetActive(false);
            slotTwoLoadingText.gameObject.SetActive(false);
        }
    }
}
