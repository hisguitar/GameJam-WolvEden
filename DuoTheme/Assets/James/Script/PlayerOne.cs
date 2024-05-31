using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerOne : NetworkBehaviour
{
    public PlayerController playerController;
    private static PlayerOne instance;
    
    public static PlayerOne Instance {
        get
        {
            if (instance != null) { return instance; }
            instance = FindFirstObjectByType<PlayerOne>();

            if (instance == null)
            {
                // Debug.LogError("No HostSingleton in the scene!");
                return null;
            }
            return instance;
        }
    }
    
}
