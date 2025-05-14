using UnityEngine;
using System.Collections;

public class EnemyRangeMove : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float chaseDuration = 5f;
    public float wanderDuration = 5f;
    public float idleDuration = 3f;
    public float randomDeviation = 1f; // Добавляем параметр для случайного отклонения
    private Transform player;
    private Vector3 randomDirection;
    private float timer;
    private enum State { Chase, Wander, Idle }
    private State currentState = State.Chase;
    private SpriteRenderer spriteRenderer;
    public Transform weaponTransform;
    private bool isRight = false;
    private float currentChaseDuration;
    private float currentWanderDuration;
    private float currentIdleDuration;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        spriteRenderer = GetComponent<SpriteRenderer>();
        timer = 0f;
        SetRandomDurations();
    }

    void Update()
    {
        timer += Time.deltaTime;

        switch (currentState)
        {
            case State.Chase:
                ChasePlayer();
                if (timer >= currentChaseDuration)
                {
                    currentState = State.Wander;
                    timer = 0f;
                    SetRandomDirection();
                    SetRandomDurations(); // Обновляем длительности при смене состояния
                }
                break;
            case State.Wander:
                Wander();
                if (timer >= currentWanderDuration)
                {
                    currentState = State.Idle;
                    timer = 0f;
                    SetRandomDurations(); // Обновляем длительности при смене состояния
                }
                break;
            case State.Idle:
                Idle();
                if (timer >= currentIdleDuration)
                {
                    currentState = State.Chase;
                    timer = 0f;
                    SetRandomDurations(); // Обновляем длительности при смене состояния
                }
                break;
        }

        UpdateSpriteFlip();
    }

    void ChasePlayer()
    {
        Vector3 direction = player.position - transform.position;
        direction.Normalize();
        transform.position += direction * moveSpeed * Time.deltaTime;
    }

    void Wander()
    {
        transform.position += randomDirection * moveSpeed * Time.deltaTime;
    }

    void Idle()
    {
        // Стоим на месте
    }

    void SetRandomDirection()
    {
        randomDirection = Random.insideUnitSphere;
        randomDirection.Normalize();
    }

    void UpdateSpriteFlip()
    {
        if (player.position.x > transform.position.x)
        {
            spriteRenderer.flipX = true;
            if (!isRight)
            {
                if (weaponTransform != null)
                {
                    //weaponTransform.rotation = new Quaternion(0,180,0);
                }
                isRight = true;
            }
        }
        else
        {
            spriteRenderer.flipX = false;
            if (isRight)
            {
                if (weaponTransform != null)
                {
                    //weaponTransform.localScale = new Vector3(weaponTransform.localScale.x * -1, weaponTransform.localScale.y, weaponTransform.localScale.z);
                }
                isRight = false;
            }
        }
    }

    void SetRandomDurations()
    {
        currentChaseDuration = chaseDuration + Random.Range(-randomDeviation, randomDeviation);
        currentWanderDuration = wanderDuration + Random.Range(-randomDeviation, randomDeviation);
        currentIdleDuration = idleDuration + Random.Range(-randomDeviation, randomDeviation);

        // Гарантируем, что длительность не станет отрицательной
        currentChaseDuration = Mathf.Max(0, currentChaseDuration);
        currentWanderDuration = Mathf.Max(0, currentWanderDuration);
        currentIdleDuration = Mathf.Max(0, currentIdleDuration);
    }
}