using UnityEngine;

public class RangedWeapon : Weapon
{
    public override WeaponType Type => WeaponType.Ranged;
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float fireRate = 0.5f;
    private float nextFire = 0f;
    void Update()
     {
         if (Input.GetMouseButton(0) && Time.time > nextFire)
         {
             nextFire = Time.time + fireRate;
             Attack();
         }
     }
    public override void Attack()
    {
        Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
    }
}
