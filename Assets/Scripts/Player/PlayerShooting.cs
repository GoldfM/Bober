 using UnityEngine;

 public class PlayerShooting : MonoBehaviour
 {
     public GameObject bulletPrefab;
     public Transform firePoint;
     public float fireRate = 0.5f;
     private float nextFire = 0f;

     void Update()
     {
         if (Input.GetMouseButton(0) && Time.time > nextFire)
         {
             nextFire = Time.time + fireRate;
             Shoot();
         }
     }

     void Shoot()
     {
         Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
     }
 }
