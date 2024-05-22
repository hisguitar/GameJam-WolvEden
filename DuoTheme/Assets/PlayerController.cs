using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace JamesCode
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float playerHealth;
        [SerializeField] private float playerStamina;
        [SerializeField] private float playerSpeed;

        public float PlayerSpeed
        {
            get { return playerSpeed; }
        }
    }
}

