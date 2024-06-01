using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float damage = 10.0f;

    public void setDamage(float damage)
    {
        this.damage = damage;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            HealthScript enemy = collision.gameObject.GetComponent<HealthScript>();
            enemy.TakeDamage(damage);
            Destroy(this.gameObject);
        }
    }
}
