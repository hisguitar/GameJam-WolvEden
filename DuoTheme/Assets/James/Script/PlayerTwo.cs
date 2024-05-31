using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerTwo : NetworkBehaviour
{
    public PlayerController playerController;
    private static PlayerTwo instance;
    
    public static PlayerTwo Instance {
        get
        {
            if (instance != null) { return instance; }
            instance = FindFirstObjectByType<PlayerTwo>();

            if (instance == null)
            {
                // Debug.LogError("No HostSingleton in the scene!");
                return null;
            }
            return instance;
        }
    }
}
