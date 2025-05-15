using UnityEngine;
using System.Collections;

public class movement : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speed = 0.5f;
    private Vector2 moveVector;
    private float originalSpeed; // Сохраняем оригинальную скорость
    private bool isSlowed = false; // Проверяем, замедлен ли игрок

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        originalSpeed = speed; // Сохраняем оригинальную скорость
    }

    void Update()
    {
        moveVector.x = Input.GetAxis("Horizontal");
        moveVector.y = Input.GetAxis("Vertical");
        rb.linearVelocity = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * speed;
    }

    public void ApplySlow(float slowPercentage)
    {
        if (!isSlowed)
        {
            speed *= (1 - slowPercentage); // Уменьшаем скорость
            isSlowed = true;
        }
    }

    public void RemoveSlow()
    {
        if (isSlowed)
        {
            speed = originalSpeed; // Возвращаем скорость
            isSlowed = false;
        }
    }
}