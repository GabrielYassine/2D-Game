using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float damage = 10.0f;

    public void setDamage(float damage)
    {
        this.damage = damage;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            HealthScript enemy = collision.gameObject.GetComponent<HealthScript>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                Destroy(this.gameObject);
            }
        }
    }
}
