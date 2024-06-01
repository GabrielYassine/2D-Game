using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthScript : MonoBehaviour
{
    public float health = 100.0f;

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0.0f)
        {
            Destroy(gameObject);
        }
    }
}
