using UnityEngine;

public class Weapon : MonoBehaviour
{
    public WeaponData weaponData;
    public Transform bulletSpawn;

    private float nextFire = 0.0f;

    private void Update()
    {
        if (Input.GetButton("Fire1") && Time.time > nextFire)
        {
            nextFire = Time.time + 1.0f / weaponData.fireRate;
            Shoot();
        }
    }

    private void Shoot()
    {
        GameObject bullet = Instantiate(weaponData.bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        bulletScript.setDamage(weaponData.playerDamage);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = bulletSpawn.up * weaponData.bulletSpeed;

        Destroy(bullet, weaponData.bulletRange / weaponData.bulletSpeed);
    }
}
