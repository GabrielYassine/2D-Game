using UnityEngine;

public class ExplosiveBullet : Bullet
{
    public GameObject explosionPrefab;

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        if (collision.gameObject.CompareTag("Player"))
        {
            //Instantiate(explosionPrefab, transform.position, transform.rotation);
            HealthScript enemy = collision.gameObject.GetComponent<HealthScript>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
            Destroy(gameObject);

        }
    }
}
