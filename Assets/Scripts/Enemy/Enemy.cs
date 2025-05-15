using UnityEngine;
using System.Collections;
public class Enemy : MonoBehaviour
{
    public int maxHealth = 1; // Максимальное здоровье врага
    public int currentHealth; // Текущее здоровье врага
    public int damage = 1; // Урон, наносимый врагом

    [Header("Настройки дропа")]
    public GameObject coinPrefab; // Префаб монеты
    public GameObject healthPickupPrefab; // Префаб хилки
    public GameObject[] weaponPrefabs; // Массив префабов оружия

    [Range(0, 1)] public float coinDropChance = 0.3f; // Вероятность выпадения монеты
    [Range(0, 1)] public float healthDropChance = 0.2f; // Вероятность выпадения хилки
    [Range(0, 1)] public float weaponDropChance = 0.1f; // Вероятность выпадения оружия

    private Transform levelParent; // Ссылка на объект Level
    private Transform healthBar; // Полоса здоровья
    private float initialHealthBarWidth; // Начальная ширина полоски здоровья

    public float invincibilityTime = 0.5f; // Длительность неуязвимости в секундах
    private bool isInvincible = false; // Флаг неуязвимости
    private float invincibilityTimer = 0f; // Таймер неуязвимости

    private SpriteRenderer enemySpriteRenderer; // Ссылка на SpriteRenderer врага

    void Awake()
    {
        levelParent = transform.parent; // Присваиваем родителя врага levelParent
        healthBar = transform.Find("HealthBar"); // Получаем ссылку на объект полосы здоровья
        enemySpriteRenderer = GetComponent<SpriteRenderer>(); // Получаем ссылку на SpriteRenderer врага

        // Инициализация текущего здоровья
        currentHealth = maxHealth;
        initialHealthBarWidth = healthBar.localScale.x;
    }

    void Update()
    {
        // Обновляем таймер неуязвимости
        if (isInvincible)
        {
            invincibilityTimer -= Time.deltaTime;
            if (invincibilityTimer <= 0)
            {
                isInvincible = false;
                enemySpriteRenderer.color = Color.white; // Возвращаем обычный цвет
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (!isInvincible) // Проверяем, неуязвим ли враг
        {
            currentHealth -= damage;
            UpdateHealthBar(); // Обновляем полоску здоровья

            if (currentHealth <= 0)
            {
                DropItem();
                Destroy(gameObject);
            }

            isInvincible = true; // Включаем неуязвимость
            invincibilityTimer = invincibilityTime; // Устанавливаем время неуязвимости
            StartCoroutine(FlashWhite()); // Запускаем корутину для эффекта окраски
        }
    }

    private IEnumerator FlashWhite()
    {
        // Изменяем цвет на белый (или другой цвет для эффекта)
        enemySpriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.2f); // Ждем 0.2 секунды
        // Возвращаем обычный цвет
        enemySpriteRenderer.color = Color.white;
    }

    void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            // Вычисляем долю оставшегося здоровья
            float healthPercentage = (float)currentHealth / maxHealth;
            healthBar.localScale = new Vector3(initialHealthBarWidth * healthPercentage, healthBar.localScale.y, healthBar.localScale.z);
        }
    }

    void DropItem()
    {
        float randomValue = Random.value;
        GameObject drop1 = null; // Объявляем переменную drop
        if (randomValue < weaponDropChance)
        {
            if (weaponPrefabs != null && weaponPrefabs.Length > 0)
            {
                GameObject weaponToDrop = weaponPrefabs[Random.Range(0, weaponPrefabs.Length)];
                drop1 = Instantiate(weaponToDrop, transform.position, Quaternion.identity);
            }
        }
        else if (randomValue < healthDropChance)
        {
            drop1 = Instantiate(healthPickupPrefab, transform.position, Quaternion.identity);
        }
        if (randomValue < coinDropChance)
        {
            drop1 = Instantiate(coinPrefab, transform.position, Quaternion.identity);
        }
        if (drop1 != null && levelParent != null)
        {
            drop1.transform.SetParent(levelParent, worldPositionStays: true);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Player>().TakeDamage(damage);
        }
    }
}