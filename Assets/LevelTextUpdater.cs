using UnityEngine;
using TMPro; // Добавляем пространство имен TextMeshPro

public class LevelTextUpdater : MonoBehaviour
{
    public TMP_Text levelText; // Ссылка на текстовый компонент TextMeshPro

    private void Start()
    {
        // Проверяем, что текстовое поле установлено
        if (levelText == null)
        {
            Debug.LogError("LevelText не присвоен! Пожалуйста, присвойте текстовое поле TextMeshPro в инспекторе.");
            return;
        }

        // Подписываемся на событие OnLevelChanged в GameManager
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnLevelChanged += UpdateLevelText;
        }
        else
        {
            Debug.LogError("GameManager не найден!");
        }

        // Обновляем текст уровня при старте
        UpdateLevelText();
    }

    // Метод для обновления текста уровня
    public void UpdateLevelText(int newLevel = 0) // int newLevel = 0 чтобы можно было вызвать без параметров при старте
    {
        // Проверяем, что GameManager существует
        if (GameManager.Instance != null)
        {
            levelText.text = "Уровень " + GameManager.Instance.currentLevel;
        }
        else
        {
            Debug.LogError("GameManager не найден!");
        }
    }

    private void OnDestroy()
    {
        // Отписываемся от события при уничтожении объекта
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnLevelChanged -= UpdateLevelText;
        }
    }
}