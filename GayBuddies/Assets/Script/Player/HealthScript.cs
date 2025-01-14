using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using FishNet.Connection;
using UnityEngine;

public class HealthScript : NetworkBehaviour
{
    public float health = 100.0f;

    public void TakeDamage(float damage)
    {
        if (IsServer)
        {
            health -= damage;
            if (health <= 0.0f)
            {
                Die();
            }
            UpdateHealth(health);
        }
    }

    private void Die()
    {
        Debug.Log("Player has died.");
        Destroy(gameObject);
    }

    [ObserversRpc] // This method will be called on all clients
    private void UpdateHealth(float newHealth)
    {
        health = newHealth;
        Debug.Log("Updated health: " + health);
    }
}
