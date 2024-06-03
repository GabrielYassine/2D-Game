using System;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public Dictionary<string, WeaponData> weapons = new Dictionary<string, WeaponData>();
    private WeaponData currentWeapon;
    public Transform bulletSpawn;

    private float nextFire = 0.0f;

    void Awake()
    {
        LoadWeapons();
    }
    void Start()
    {
        EquipWeapon("NormalWeapon");
    }

    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time > nextFire)
        {
            nextFire = Time.time + 1.0f / currentWeapon.fireRate;
            Shoot();
        }
    }


    private void Shoot()
    {
        GameObject bullet = Instantiate(currentWeapon.bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        bulletScript.setDamage(currentWeapon.playerDamage);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = bulletSpawn.up * currentWeapon.bulletSpeed;

        Destroy(bullet, currentWeapon.bulletRange / currentWeapon.bulletSpeed);
    }

    public void EquipWeapon(String weapon)
    {
        currentWeapon = weapons[weapon];
    }
    private void LoadWeapons()
    {
        string directory = "ScriptableObjects/Weapons";
        WeaponData[] weaponDataArray = Resources.LoadAll<WeaponData>(directory);
        foreach (WeaponData weaponData in weaponDataArray)
        {
            if (!weapons.ContainsKey(weaponData.name))
            {
                weapons.Add(weaponData.name, weaponData);
                Debug.Log($"Loaded weapon: {weaponData.name}");
            }
            else
            {
                Debug.LogWarning($"Duplicate weapon name found: {weaponData.name}. Skipping this entry.");
            }
        }
    }
}
