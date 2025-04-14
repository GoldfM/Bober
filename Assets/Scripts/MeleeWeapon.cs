using UnityEngine;

public class MeleeWeapon : Weapon
{
    public override WeaponType Type => WeaponType.Melee;
    public int damage = 1;

    public override void Attack()
    {
        Debug.Log("Melee Attack: " + weaponName + " Damage: " + damage);
        // Здесь будет логика атаки ближнего боя (например, обнаружение врагов в радиусе)
    }
}
