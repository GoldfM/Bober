using UnityEngine;

public class HatchTrigger : MonoBehaviour
{
    public GameObject startButton; // Ссылка на кнопку "Начать"
    private bool playerIsNear = false;

    void Start()
    {
        // Если поле startButton не заполнено, ищем кнопку на сцене среди всех объектов, включая неактивные
        if (startButton == null)
        {
            // Получаем все объекты типа GameObject на сцене, включая неактивные
            GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();

            // Ищем кнопку "Next" среди всех объектов
            foreach (GameObject obj in allObjects)
            {
                if (obj.name == "Next")
                {
                    startButton = obj;
                    break;
                }
            }

            if (startButton == null)
            {
                Debug.LogWarning("Кнопка 'Next' не найдена на сцене. Пожалуйста, убедитесь, что она существует.");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerIsNear = true;
            UpdateStartButtonVisibility();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerIsNear = false;
            UpdateStartButtonVisibility();
        }
    }

    private void UpdateStartButtonVisibility()
    {
        // Проверяем, что кнопка существует, прежде чем пытаться ее активировать/деактивировать
        if (startButton != null)
        {
            startButton.SetActive(playerIsNear);
        }
    }
}