using UnityEngine;
using System.Collections;

public class EnemyRangeMove : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float chaseDuration = 5f;
    public float wanderDuration = 5f;
    public float idleDuration = 3f;
    private Transform player;
    private Vector3 randomDirection;
    private float timer;
    private enum State { Chase, Wander, Idle }
    private State currentState = State.Chase;
    private SpriteRenderer spriteRenderer;
    public Transform weaponTransform;
    private bool isRight = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        spriteRenderer = GetComponent<SpriteRenderer>();
        timer = 0f;
    }

    void Update()
    {
        timer += Time.deltaTime;

        switch (currentState)
        {
            case State.Chase:
                ChasePlayer();
                if (timer >= chaseDuration)
                {
                    currentState = State.Wander;
                    timer = 0f;
                    SetRandomDirection();
                }
                break;
            case State.Wander:
                Wander();
                if (timer >= wanderDuration)
                {
                    currentState = State.Idle;
                    timer = 0f;
                }
                break;
            case State.Idle:
                Idle();
                if (timer >= idleDuration)
                {
                    currentState = State.Chase;
                    timer = 0f;
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
                    //weaponTransform.localScale = new Vector3(weaponTransform.localScale.x * -1, weaponTransform.localScale.y, weaponTransform.localScale.z);
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
}
