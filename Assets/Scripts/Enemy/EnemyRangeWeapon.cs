using UnityEngine;

public class EnemyRangeWeapon : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 1f;
    public int bulletDamage; // Урон пули

    private float nextFireTime = 0;
    private Transform player;
    private SpriteRenderer gunSprite;
    private Vector3 initialScale;

    void Start()
    {
        gunSprite = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        initialScale = transform.localScale;
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
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        if (player.position.x < transform.position.x)
        {
            transform.localScale = new Vector3(-Mathf.Abs(initialScale.x), -Mathf.Abs(initialScale.y), initialScale.z);
        }
        else
        {
            transform.localScale = new Vector3(-Mathf.Abs(initialScale.x), Mathf.Abs(initialScale.y), initialScale.z);
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
            bullet.damage = bulletDamage; // Передаем урон пуле
        }
    }
}