using UnityEngine;

public class EnemyRangeWeapon : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 1f;
    private float nextFireTime;
    private Transform player;
    private SpriteRenderer gunSprite;

    void Start()
    {
        gunSprite = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        Vector3 direction = player.position - firePoint.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        GunRotate(angle);
        
        if (Time.time >= nextFireTime)
        {
            Shoot(angle, direction);
            nextFireTime = Time.time + 1f / fireRate;
        }
    }

 
    
    void GunRotate(float angle)
    {
        Vector3 scale = transform.localScale;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        if (player.position.x < transform.position.x && scale.y > 0)
        {
            scale.y = -scale.y;
            if (scale.x < 0)
            {
                scale.x = -scale.x;
            }
            
            transform.localScale = scale;
        }
        else if(player.position.x > transform.position.x && scale.y < 0)
        {
            scale.y = -scale.y;
            if (scale.x > 0)
            {
                scale.x = -scale.x;
            }
            transform.localScale = scale;
        }

    }

    void Shoot(float angle, Vector3 direction)
    {
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        GameObject bulletObj = Instantiate(bulletPrefab, firePoint.position, rotation);
        EnemyBullet bullet = bulletObj.GetComponent<EnemyBullet>();
        if (bullet != null)
        {
            bullet.SetDirection(direction);
        }
    }
}
