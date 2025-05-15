using UnityEngine;

public class RotateTowardsMouse : MonoBehaviour
{
    public SpriteRenderer spriteRenderer; // Ссылка на SpriteRenderer оружия
    private PauseMenu pauseMenu; // Ссылка на компонент PauseMenu

    void Start()
    {
        // Получаем компонент SpriteRenderer при старте
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer не найден на этом объекте!");
        }

        // Поиск объекта LevelManager и получение компонента PauseMenu
        GameObject levelManager = GameObject.Find("LevelManager");
        if (levelManager != null)
        {
            pauseMenu = levelManager.GetComponent<PauseMenu>();
        }
        else
        {
            Debug.LogError("Объект LevelManager не найден на сцене!");
        }
    }

    void Update()
    {
        if (pauseMenu != null && !pauseMenu.isPaused) // Проверяем, в состоянии ли паузы
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = mousePosition - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            Vector3 scale = transform.localScale;

            // Флип спрайта в зависимости от положения мыши относительно игрока
            if (mousePosition.x > transform.position.x)
            {
                if (scale.y < 0)
                    scale.y = -scale.y;
            }
            else
            {
                scale.y = -scale.y;
                if (scale.y > 0)
                    scale.y = -scale.y;
            }

            transform.localScale = scale;
        }
    }
}