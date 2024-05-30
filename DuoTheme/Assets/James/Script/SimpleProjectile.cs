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
        else if (other.CompareTag("Shield"))
        {
            Destroy(gameObject);
        }
        // Other cases, don't destroy the bullets.
        else
        {
            return;
        }
    }
}
