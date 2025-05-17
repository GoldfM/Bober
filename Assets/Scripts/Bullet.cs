using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public float lifeTime = 2f;
    public int damage = 1; // Урон пули
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = transform.right * speed;
        Destroy(gameObject, lifeTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null)
        {
            // Получаем коэффициент урона игрока
            float damageMultiplier = Player.Instance.GetDamageMultiplier();
            // Рассчитываем итоговый урон
            int totalDamage = Mathf.RoundToInt(damage * damageMultiplier);
            // Наносим урон врагу
            enemy.TakeDamage(totalDamage);
            Destroy(gameObject);
        }

        if (collision.tag == "Walls")
        {
            Destroy(gameObject);
        }
    }
}