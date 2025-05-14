using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI; // Ссылка на панель пользовательского интерфейса меню паузы
    private bool isPaused = false; // Флаг для отслеживания того, находится ли игра в состоянии паузы

    void Update()
    {
        // Проверьте ввод для переключения паузы (например, клавиша Escape)
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            ActivateMenu();
        }
        else
        {
            DeactivateMenu();
        }
    }

    void ActivateMenu()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f; // Приостановить время
    }

    void DeactivateMenu()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f; // Возобновить нормальное время
    }

    public void RestartLevel()
    {
      Time.timeScale = 1f;
      SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ReturnToMainMenu()
    {
      Time.timeScale = 1f;
      SceneManager.LoadScene("Hub"); // Замените MainMenu именем сцены вашего главного меню
    }
}