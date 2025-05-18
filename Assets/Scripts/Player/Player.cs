using UnityEngine;
using UnityEngine.UI; // Для доступа к компонентам UI
using System.Collections;
using TMPro; // Для доступа к TextMeshPro

public class Player : MonoBehaviour
{
    public float speed = 1f;
    public int health = 100; // Начальное здоровье
    public int maxHP = 100;
    public SpriteRenderer playerSpriteRenderer;
    public AudioClip damageSound;
    private AudioSource audioSource;
    private movement playerMovement;
    public HealthBar healthBar;
    private bool isAcid;
    private Rigidbody2D rb;
    public static Player Instance { get; private set; }
    private AudioManager audioManager; // Ссылка на AudioManager
    public float invincibilityTime = 0.5f; // Длительность неуязвимости в секундах
    private bool isInvincible = false; // Флаг неуязвимости
    private float invincibilityTimer = 0f; // Таймер неуязвимости

    private float damageMultiplier; // Коэффициент урона

    public GameObject gameOverPanel; // Ссылка на панель GameOver
    public TMP_Text reachedLevelText; // TextMeshPro для показа достигнутого уровня на панели GameOver
    public TMP_Text recordLevelText; // TextMeshPro для показа рекордного уровня на панели GameOver
    public GameObject newRecordText; // GameObject для показа "НОВЫЙ РЕКОРД"

    private void Awake()
    {
        Instance = this;
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerMovement = GetComponent<movement>();

        // Загружаем коэффициент урона из PlayerPrefs при старте
        damageMultiplier = PlayerPrefs.GetFloat("DamageMultiplier", 1.0f);
        GameObject audioManagerObject = GameObject.Find("AudioManager");
        if (audioManagerObject != null)
        {
            audioManager = audioManagerObject.GetComponent<AudioManager>();
            if (audioManager == null)
            {
                Debug.LogError("AudioManager компонент не найден на объекте AudioManager!");
            }
        }
        else
        {
            Debug.LogError("Объект AudioManager не найден на сцене!");
        }

        // Убедимся, что панель GameOver деактивирована при старте
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
        else
        {
            Debug.LogError("Панель GameOver не назначена!");
        }

        // Инициализация TextMeshPro
        if (reachedLevelText == null || recordLevelText == null)
        {
            Debug.LogError("TextMeshPro компоненты не назначены!");
        }

        // Убедимся, что текст "НОВЫЙ РЕКОРД" деактивирован при старте
        if (newRecordText != null)
        {
            newRecordText.SetActive(false);
        }
        else
        {
            Debug.LogError("Текст 'НОВЫЙ РЕКОРД' не назначен!");
        }
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
                playerSpriteRenderer.color = isAcid ? Color.green : Color.white; // Возвращаем обычный цвет
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (!isInvincible) // Проверяем, неуязвим ли игрок
        {
            health -= damage;

            // Устанавливаем минимальное значение здоровья
            if (health < 0)
            {
                health = 0;
            }

            healthBar.UpdateHealthBar();
            PlayDamageSound();
            StartCoroutine(FlashRed());

            if (health <= 0)
            {
                Die(); // Вызываем метод Die() при смерти
            }

            isInvincible = true; // Включаем неуязвимость
            invincibilityTimer = invincibilityTime; // Устанавливаем время неуязвимости
        }
    }

    // Функция для восстановления здоровья
    public void Heal(int healAmount)
    {
        health += healAmount;
        // Убедитесь, что здоровье не превышает максимальное
        if (health > maxHP)
        {
            health = maxHP;
        }
        healthBar.UpdateHealthBar();
    }

    private void PlayDamageSound()
    {
        if (damageSound != null && audioSource != null)
        {
            audioManager.PlayOneShotSound(audioSource, damageSound);
        }
    }

    private IEnumerator FlashRed()
    {
        // Изменяем цвет на красный
        playerSpriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.2f); // Ждем 0.2 секунды
        // Возвращаем цвет обратно на обычный цвет, учитывая эффект кислоты
        playerSpriteRenderer.color = isAcid ? Color.green : Color.white;
    }

    public Vector3 GetPlayerScreenPosition()
    {
        return Camera.main.WorldToScreenPoint(transform.position);
    }

    public void ApplyAcid(float slowPercentage, float slowDuration, int damagePerSecond)
    {
        StartCoroutine(AcidEffect(slowPercentage, slowDuration, damagePerSecond));
    }

    private IEnumerator AcidEffect(float slowPercentage, float slowDuration, int damagePerSecond)
    {
        playerMovement.ApplySlow(slowPercentage);
        playerSpriteRenderer.color = Color.green;
        isAcid = true;
        float timePassed = 0f;
        while (timePassed < slowDuration)
        {
            TakeDamage(damagePerSecond);
            yield return new WaitForSeconds(1f);
            timePassed += 1f;
        }
        playerMovement.RemoveSlow();
        isAcid = false;
        playerSpriteRenderer.color = Color.white;
    }

    // Метод для получения коэффициента урона
    public float GetDamageMultiplier()
    {
        return damageMultiplier;
    }

    // Метод для обработки смерти игрока


    private void Die()
    {
        // Ставим игру на паузу
        Time.timeScale = 0;

        // Обновляем и сохраняем максимальный уровень
        int currentLevel = GameManager.Instance.currentLevel;
        int maxLevel = PlayerPrefs.GetInt("MaxLevel", 1); // Получаем максимальный уровень из PlayerPrefs, по умолчанию 1

        bool isNewRecord = false;
        if (currentLevel > maxLevel)
        {
            maxLevel = currentLevel;
            PlayerPrefs.SetInt("MaxLevel", maxLevel);
            PlayerPrefs.Save(); // Сохраняем изменения
            isNewRecord = true;
        }

        // Активируем панель GameOver
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);

            // Обновляем TextMeshPro текст на панели GameOver
            reachedLevelText.text = "Ты достиг: " + currentLevel;
            recordLevelText.text = "Рекорд: " + maxLevel;

            // Активируем текст "НОВЫЙ РЕКОРД", если установлен новый рекорд
            if (newRecordText != null)
            {
                newRecordText.SetActive(isNewRecord);
            }
        }
        else
        {
            Debug.LogError("Панель GameOver не назначена!");
        }

        // Отключаем скрипты игрока, чтобы он не двигался и не атаковал

        movement playerMovement = GetComponent<movement>();
        if (playerMovement != null)
        {
            playerMovement.enabled = false;
        }

        PlayerInventory playerInventory = GetComponent<PlayerInventory>();
        if (playerInventory != null)
        {
            playerInventory.enabled = false;
        }

        this.enabled = false;
        GetComponent<Collider2D>().enabled = false;
        playerSpriteRenderer.enabled = false;
        rb.linearVelocity = Vector2.zero; // Останавливаем движение
        Destroy(transform.gameObject);
    }
}