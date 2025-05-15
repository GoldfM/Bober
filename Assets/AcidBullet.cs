using UnityEngine;

public class AcidBullet : MonoBehaviour
{
    public float lifeTime = 5f; // Время жизни пули
    public int initialDamage = 5; // Начальный урон
    public int damagePerSecond = 2; // Урон в секунду
    public float slowPercentage = 0.5f; // Процент замедления (0.5 = 50%)
    public float slowDuration = 3f; // Продолжительность замедления

    private Vector3 direction; // Направление движения пули
    private float speed; // Скорость пули
    private float timer; // Таймер для отсчета времени жизни пули

    public void SetDirection(Vector3 direction, float speed)
    {
        this.direction = direction;
        this.speed = speed;
    }

    void OnEnable()
    {
        timer = 0f;
    }

    void Update()
    {
        // Движение пули в направлении
        transform.position += direction * speed * Time.deltaTime;

        // Уничтожение пули по истечении времени жизни
        timer += Time.deltaTime;
        if (timer >= lifeTime)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Проверка на столкновение с игроком
        Player playerScript = other.GetComponent<Player>();
        if (playerScript != null)
        {
            movement playerMovement = other.GetComponent<movement>();
            if (playerMovement != null)
            {
                ApplyAcidEffect(playerScript, playerMovement); // Применяем эффект кислоты на игроке
                Destroy(gameObject); // Уничтожаем пулю после столкновения
            }
        }
        // Уничтожение пули при столкновении со стеной
        if (other.tag == "Walls")
        {
            Destroy(gameObject);
        }
    }

    private void ApplyAcidEffect(Player playerScript, movement playerMovement)
    {
        // Наносим начальный урон
        playerScript.TakeDamage(initialDamage);

        // Применяем эффект кислоты через Player
        playerScript.ApplyAcid(slowPercentage, slowDuration, damagePerSecond);
    }
}