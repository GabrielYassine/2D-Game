using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "ScriptableObjects/WeaponData", order = 1)]
public class WeaponData : ScriptableObject
{
    public string weaponName;
    public GameObject bulletPrefab;
    public float bulletSpeed;
    public float fireRate;
    public float bulletRange;
    public float playerDamage;
}
