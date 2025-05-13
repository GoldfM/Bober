using UnityEngine;

public class MeleeWeapon : Weapon
{
    public override WeaponType Type => WeaponType.Melee;
    public float fireRate = 0.5f;
    public Animator animator;
    public Collider2D swordCollider;
    public int damage = 20;
    private float nextFire = 0f;

    void Update()
    {
        // Поворот меча к мыши
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = (mousePosition - transform.position).normalized;
        transform.up = direction;

        if (Input.GetMouseButton(0) && Time.time > nextFire && animator.GetInteger("IsAttack") == 0)
        {
            Attack();
        }
    }

    public override void Attack()
    {
        animator.SetInteger("IsAttack", 1);
        swordCollider.enabled = true;
    }

    public void DisableCollider()
    {
        animator.SetInteger("IsAttack", 0);
        swordCollider.enabled = false;
        nextFire = Time.time + fireRate;
    }

    void OnTriggerEnter2D(Collider2D hit)
    {
        Enemy enemy = hit.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }
    }
}
