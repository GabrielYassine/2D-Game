using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage;

    public void setDamage(float damage)
    {
        this.damage = damage;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            HealthScript enemy = collision.gameObject.GetComponent<HealthScript>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);

            }
            Destroy(gameObject);
        }
    }
}
