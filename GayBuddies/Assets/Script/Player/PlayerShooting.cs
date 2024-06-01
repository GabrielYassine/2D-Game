using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float bulletSpeed = 15.0f;
    public float fireRate = 1.0f; // Number of times player can shoot per second
    public float bulletRange = 10.0f; // Range of the bullet
    private float nextFire = 0.0f; // Time when player can shoot again
    public float playerDamage = 10.0f;

    public bulletTypes bulletType = bulletTypes.normal;


    public enum bulletTypes
    {
        normal,
        explosive
    }

    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time > nextFire && bulletType == bulletTypes.normal)
        {
            nextFire = Time.time + 1.0f / fireRate; // Calculate the next time the player can fire
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
            bullet.GetComponent<BulletScript>().setDamage(playerDamage);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.velocity = bulletSpawn.up * bulletSpeed;

            // Destroy the bullet after it has traveled its range
            Destroy(bullet, bulletRange / bulletSpeed);
        }
    }
}