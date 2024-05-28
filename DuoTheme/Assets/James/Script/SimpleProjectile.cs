using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleProjectile : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Shield"))
        {
            other.GetComponent<Shield>().TakeDamage(50);
            Destroy(gameObject);
        }
        else if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().ReceiveDamage(25);
            Destroy(gameObject);
        }
        
        Destroy(gameObject);
    }
}
