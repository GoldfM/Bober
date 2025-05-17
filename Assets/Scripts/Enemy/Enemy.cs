using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public int baseMaxHealth = 1; // Базовое максимальное здоровье врага
    public int currentHealth; // Текущее здоровье врага
    public int baseDamage = 1; // Базовый урон, наносимый врагом

    [Header("Настройки дропа")]
    public GameObject coinPrefab; // Префаб монеты
    public GameObject healthPickupPrefab; // Префаб хилки
    public GameObject[] weaponPrefabs; // Массив префабов оружия

    [Range(0, 1)] public float coinDropChance = 0.3f; // Вероятность выпадения монеты
    [Range(0, 1)] public float healthDropChance = 0.2f; // Вероятность выпадения хилки
    [Range(0, 1)] public float weaponDropChance = 0.1f; // Вероятность выпадения оружия

    [Header("Настройки монет")]
    public int guaranteedCoins = 5; // Гарантированное количество монет за убийство
    public int additionalCoinMin = 3; // Минимальное количество дополнительных монет
    public int additionalCoinMax = 5; // Максимальное количество дополнительных монет

    [Header("Scaling Settings")]
    public float healthIncreasePerLevel = 1.1f; // Множитель увеличения здоровья с каждым уровнем
    public float damageIncreasePerLevel = 1.05f; // Множитель увеличения урона с каждым уровнем

    private Transform levelParent; // Ссылка на объект Level
    private Transform healthBar; // Полоса здоровья
    private float initialHealthBarWidth; // Начальная ширина полоски здоровья

    public float invincibilityTime = 0.5f; // Длительность неуязвимости в секундах
    private bool isInvincible = false; // Флаг неуязвимости
    private float invincibilityTimer = 0f; // Таймер неуязвимости

    private SpriteRenderer enemySpriteRenderer; // Ссылка на SpriteRenderer врага
    private int maxHealth;
    public int damage;
    private ScoreManager scoreManager;

    void Awake()
    {
        levelParent = transform.parent; // Присваиваем родителя врага levelParent
        healthBar = transform.Find("HealthBar"); // Получаем ссылку на объект полосы здоровья
        enemySpriteRenderer = GetComponent<SpriteRenderer>(); // Получаем ссылку на SpriteRenderer врага

        // Инициализация текущего здоровья
        SetStatsBasedOnLevel();
        currentHealth = maxHealth;
        initialHealthBarWidth = healthBar.localScale.x;
    }

    void Start()
    {
        // Получаем ссылку на ScoreManager
        scoreManager = FindObjectOfType<ScoreManager>();
        if (scoreManager == null)
        {
            Debug.LogError("ScoreManager не найден на сцене!");
        }
    }

    void SetStatsBasedOnLevel()
    {
        // Получаем текущий уровень из GameManager
        int currentLevel = GameManager.Instance.currentLevel;

        // Вычисляем здоровье и урон на основе уровня
        maxHealth = Mathf.RoundToInt(baseMaxHealth * Mathf.Pow(healthIncreasePerLevel, currentLevel - 1));

        damage = Mathf.RoundToInt(baseDamage * Mathf.Pow(damageIncreasePerLevel, currentLevel - 1));

        // Обновляем текущее здоровье до максимального
        currentHealth = maxHealth;
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
                DropCoins(); // Вызываем метод для гарантированного получения монет
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

        // Генерируем случайный офсет
        Vector2 dropOffset = Random.insideUnitCircle * 0.5f; // Случайный вектор в круге радиусом 0.5
        GameObject drop1 = null; // Объявляем переменную drop
        if (randomValue < weaponDropChance)
        {
            if (weaponPrefabs != null && weaponPrefabs.Length > 0)
            {
                GameObject weaponToDrop = weaponPrefabs[Random.Range(0, weaponPrefabs.Length)];
                drop1 = Instantiate(weaponToDrop, (Vector2)transform.position + dropOffset, Quaternion.identity);
                if (drop1 != null && levelParent != null)
                {
                    drop1.transform.SetParent(levelParent, worldPositionStays: true);
                }
            }
        }
        else if (randomValue < healthDropChance)
        {
            drop1 = Instantiate(healthPickupPrefab, (Vector2)transform.position + dropOffset, Quaternion.identity);
            if (drop1 != null && levelParent != null)
            {
                drop1.transform.SetParent(levelParent, worldPositionStays: true);
            }
        }

        // Дополнительные монеты
        if (Random.value < coinDropChance)
        {
            int additionalCoins = Random.Range(additionalCoinMin, additionalCoinMax + 1);
            for (int i = 0; i < additionalCoins; i++)
            {
                // Генерируем случайный офсет для каждой монеты
                Vector2 coinDropOffset = Random.insideUnitCircle * 0.5f;
                drop1 = Instantiate(coinPrefab, (Vector2)transform.position + coinDropOffset, Quaternion.identity);
                if (drop1 != null && levelParent != null)
                {
                    drop1.transform.SetParent(levelParent, worldPositionStays: true);
                }
            }
        }
    }

    // Метод для гарантированного получения монет
    void DropCoins()
    {
        if (scoreManager != null)
        {
            scoreManager.AddScore(guaranteedCoins);
        }
        else
        {
            Debug.LogError("ScoreManager не установлен!");
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