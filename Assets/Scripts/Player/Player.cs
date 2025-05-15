using UnityEngine;
using System.Collections;

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

    public float invincibilityTime = 0.5f; // Длительность неуязвимости в секундах
    private bool isInvincible = false; // Флаг неуязвимости
    private float invincibilityTimer = 0f; // Таймер неуязвимости

    private void Awake()
    {
        Instance = this;
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerMovement = GetComponent<movement>();
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
            healthBar.UpdateHealthBar();
            PlayDamageSound();
            StartCoroutine(FlashRed());

            if (health <= 0)
            {
                Destroy(gameObject);
            }

            isInvincible = true; // Включаем неуязвимость
            invincibilityTimer = invincibilityTime; // Устанавливаем время неуязвимости
        }
    }

    private void PlayDamageSound()
    {
        if (damageSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(damageSound);
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
}