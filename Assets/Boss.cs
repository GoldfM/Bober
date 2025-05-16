using System.Collections;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public Transform player; // Ссылка на объект игрока
    public GameObject projectilePrefab; // Префаб снаряда
    public GameObject acidProjectilePrefab; // Префаб "кислотной" пули
    public float acidBulletSpeed = 5f; // Скорость "кислотной" пули
    public float attackRange = 10f; // Дальность атаки
    public float attackCooldown = 2f; // Время между атаками
    public Transform centerPoint; // Центр, вокруг которого будет двигаться босс
    public float movementRadius = 3f; // Радиус случайного движения
    public float moveSpeed = 3f; // Скорость движения босса
    public LayerMask damageLayer; // Слой для обнаружения, куда наносится урон
    public int damageAmount = 10; // Урон, наносимый при атаке
    public GameObject[] enemyPrefabs; // Массив префабов врагов для призыва

    public AudioClip acidAttackSound; // Звук для ядовитого удара
    public AudioClip regularAttackSound; // Звук для обычного выстрела
    private AudioSource audioSource;
    private AudioManager audioManager; // Ссылка на AudioManager

    private float nextAttackTime = 0f;
    private float nextMoveTime = 0f;
    private Vector3 targetPoint;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        // Получаем компонент AudioSource или добавляем его, если его нет
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.tag = "Sound";
        }

        // Получаем экземпляр AudioManager через GameObject.Find
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
        SetRandomTargetPoint();
    }

    void Update()
    {
        HandleMovement();

        if (Time.time >= nextAttackTime)
        {
            Attack();
        }

        // Проверка, если время для движения истекло
        if (Time.time >= nextMoveTime)
        {
            SetRandomTargetPoint(); // Устанавливаем новую случайную цель
        }
    }

    private void HandleMovement()
    {
        // Движение к случайной цели в пределах радиуса
        if (Vector3.Distance(transform.position, targetPoint) > 0.1f)
        {
            Vector3 direction = (targetPoint - transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;

            // Поворот к игроку
            FlipTowardsPlayer();
        }
    }

    private void SetRandomTargetPoint()
    {
        // Генерация случайной точки в пределах радиуса вокруг центра
        float randomX = Random.Range(-movementRadius, movementRadius);
        float randomY = Random.Range(-movementRadius, movementRadius);
        targetPoint = centerPoint.position + new Vector3(randomX, randomY, 0);

        // Устанавливаем время следующего движения
        nextMoveTime = Time.time + Random.Range(1f, 3f); // Случайный интервал от 1 до 3 секунд
    }

    private void FlipTowardsPlayer()
    {
        Vector3 direction = player.position - transform.position;
        if (direction.x > 0)
        {
            transform.localScale = new Vector3(-1, 1, 1); // Повернуть вправо
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1); // Повернуть влево
        }
    }

    private void Attack()
    {
        int attackType = Random.Range(0, 4); // Случайный выбор атаки

        switch (attackType)
        {
            case 0:
                StartCoroutine(BarrageAttack());
                break;
            case 1:
                StartCoroutine(ChargeAndSpreadAttack());
                break;
            case 2:
                StartCoroutine(SummonEnemy());
                break;
            case 3:
                StartCoroutine(AcidSlowAttack());
                break;
        }

        nextAttackTime = Time.time + attackCooldown; // Обновление времени следующей атаки
    }

    private IEnumerator BarrageAttack()
    {
        for (int i = 0; i < 5; i++)
        {
            ShootAtPlayer();
            yield return new WaitForSeconds(0.5f);
        }
    }

    private void ShootAtPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        projectile.GetComponent<Rigidbody2D>().linearVelocity = direction * 10f; // Задайте скорость снаряда
        // Воспроизводим звук обычного выстрела
        if (regularAttackSound != null && audioManager != null)
        {
            audioManager.PlayOneShotSound(audioSource, regularAttackSound);
        }
    }

    private IEnumerator ChargeAndSpreadAttack()
    {
        // Выстрелы во все стороны
        for (int i = 0; i < 8; i++)
        {
            float angle = i * 45f; // Угол между выстрелами
            Quaternion rotation = Quaternion.Euler(0, 0, angle);
            ShootAtDirection(rotation * Vector3.up);
            yield return new WaitForSeconds(0.2f);
        }
    }

    private void ShootAtDirection(Vector3 direction)
    {
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        projectile.GetComponent<Rigidbody2D>().linearVelocity = direction * 10f; // Задайте скорость снаряда
        // Воспроизводим звук обычного выстрела
        if (regularAttackSound != null && audioManager != null)
        {
            audioManager.PlayOneShotSound(audioSource, regularAttackSound);
        }
    }

    private IEnumerator SummonEnemy()
    {
        // Выбор случайного врага из массива
        GameObject enemyToSpawn = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

        // Создание нового врага
        Vector3 spawnPosition = transform.position + new Vector3(Random.Range(-2f, 2f), Random.Range(-2f, 2f), 0);
        Instantiate(enemyToSpawn, spawnPosition, Quaternion.identity);
        yield return null;
    }

    private IEnumerator AcidSlowAttack()
    {
        // Стреляем "кислотной" пулей
        GameObject acidProjectileObj = Instantiate(acidProjectilePrefab, transform.position, Quaternion.identity);
        AcidBullet acidProjectile = acidProjectileObj.GetComponent<AcidBullet>();

        // Получаем направление к игроку
        Vector3 direction = (player.position - transform.position).normalized;

        // Задаем направление и скорость пуле
        acidProjectile.SetDirection(direction, acidBulletSpeed);

        // Воспроизводим звук кислотной атаки
        if (acidAttackSound != null && audioManager != null)
        {
            audioManager.PlayOneShotSound(audioSource, acidAttackSound);
        }
        yield return null;
    }
}