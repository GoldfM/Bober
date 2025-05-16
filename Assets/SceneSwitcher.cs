using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneSwitcher : MonoBehaviour
{
    // Метод для переключения на указанную сцену по имени
    public void SwitchScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // Метод для выхода из приложения
    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}