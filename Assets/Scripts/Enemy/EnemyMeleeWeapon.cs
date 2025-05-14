using UnityEngine;

public class EnemyMeleeWeapon : MonoBehaviour
{
    public int numberOfAttackAnimations = 3; // Количество анимаций атаки (от 1 до этого числа)
    public float fireCD = 1f;

    private float nextFireTime = 0;
    private Transform player;
    private SpriteRenderer gunSprite;
    private Vector3 initialScale;
    private Animator animator;
    private int attackNumberHash; // Hash для параметра Animator "AttackNumber"

    void Start()
    {
        gunSprite = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        initialScale = transform.localScale;
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component not found on this GameObject!");
        }

        // Получаем Hash для параметра "AttackNumber" (для оптимизации)
        attackNumberHash = Animator.StringToHash("AttackNumber");
    }

    void Update()
    {
        if (Time.time >= nextFireTime)
        {
            Attack();
        }
    }

    void Attack()
    {
        // Выбираем случайную анимацию атаки
        int attackAnimationIndex = Random.Range(1, numberOfAttackAnimations + 1);

        // Устанавливаем параметр "AttackNumber" в Animator
        animator.SetInteger(attackNumberHash, attackAnimationIndex);

        // Обновляем nextFireTime
        nextFireTime = Time.time + fireCD;
    }

    // Функция для установки параметра AttackNumber в 0
    public void ResetAttackNumber()
    {
        animator.SetInteger(attackNumberHash, 0);
    }
}