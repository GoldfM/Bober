using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private PauseMenu pauseMenu; // Ссылка на компонент PauseMenu

    public void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
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

    private void Update()
    {
        if (pauseMenu != null && !pauseMenu.isPaused) // Проверяем, в состоянии ли паузы
        {
            ChangePlayerFacingDirection();
        }
    }

    private void ChangePlayerFacingDirection()
    {
        Vector3 playerPos = Player.Instance.GetPlayerScreenPosition();
        Vector3 mousePosition = GameInput.Instance.GetMousePos();
        if (mousePosition.x < playerPos.x)
        {
            transform.parent.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            transform.parent.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}