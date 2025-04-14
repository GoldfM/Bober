using UnityEngine;

public class RangedWeapon : Weapon
{
    public override WeaponType Type => WeaponType.Ranged;
    public GameObject projectilePrefab;
    public Transform firePoint;

    public override void Attack()
    {
        Debug.Log("Ranged Attack: " + weaponName);
        // Здесь будет логика атаки дальнего боя (например, выстрел пулей)
        Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
    }
}
