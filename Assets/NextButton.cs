using UnityEngine;
using UnityEngine.UI;

public class NextButton : MonoBehaviour
{
    private Button button;
    private GameManager gameManager;

    void Start()
    {
        // Получаем компонент Button
        button = GetComponent<Button>();

        if (button == null)
        {
            Debug.LogError("Кнопка 'NextButton' не имеет компонента Button. Пожалуйста, добавьте компонент Button к этому объекту.");
            return;
        }

        // Ищем GameManager на сцене по имени
        GameObject gameManagerObject = GameObject.Find("GameManager");

        if (gameManagerObject == null)
        {
            Debug.LogError("Объект 'GameManager' не найден на сцене. Пожалуйста, убедитесь, что он существует.");
            return;
        }

        // Получаем компонент GameManager
        gameManager = gameManagerObject.GetComponent<GameManager>();

        if (gameManager == null)
        {
            Debug.LogError("Объект 'GameManager' не имеет компонента GameManager. Пожалуйста, добавьте компонент GameManager к этому объекту.");
            return;
        }

        // Добавляем слушатель на нажатие кнопки
        button.onClick.AddListener(CallLevelComplete);
    }

    void CallLevelComplete()
    {
        // Вызываем функцию CompleteLevel из GameManager
        if (gameManager != null)
        {
            gameManager.CompleteLevel();
        }
    }
}