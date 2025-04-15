using UnityEngine;

public class RotateTowardsMouse : MonoBehaviour
{
    public SpriteRenderer spriteRenderer; // Ссылка на SpriteRenderer оружия

    void Start()
    {
        // Получаем компонент SpriteRenderer при старте
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer не найден на этом объекте!");
        }
    }

    void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        // Флип спрайта в зависимости от положения мыши относительно игрока
        if (mousePosition.x > transform.position.x)
        {
            spriteRenderer.flipY = false; // Мышь справа - не флипаем
        }
        else
        {
            spriteRenderer.flipY = true; // Мышь слева - флипаем
        }
    }
}
