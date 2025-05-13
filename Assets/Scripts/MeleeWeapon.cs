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

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Vector3 scale = transform.localScale;
        // Флип спрайта в зависимости от положения мыши относительно игрока
        if (mousePosition.x > transform.position.x)
        {
            if (scale.y < 0)
                scale.y = -scale.y;
            //spriteRenderer.flipY = false;
        }
        else
        {
            scale.y = -scale.y;
            if (scale.y > 0)
                scale.y = -scale.y;
            //spriteRenderer.flipY = true; // Мышь слева - флипаем
        }

        transform.localScale = scale;


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
