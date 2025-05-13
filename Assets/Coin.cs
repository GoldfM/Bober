using UnityEngine;

public class Coin : MonoBehaviour
{
    public int coinValue = 1; // Количество монет, добавляемое при подборе
    private ScoreManager scoreManager; // Ссылка на ScoreManager

    void Start()
    {
        // Получаем ссылку на ScoreManager
        scoreManager = FindObjectOfType<ScoreManager>();

        if (scoreManager == null)
        {
            Debug.LogError("ScoreManager не найден на сцене!");
        }
    }

    // Метод, вызываемый при столкновении с другим объектом
    void OnTriggerEnter2D(Collider2D other)
    {
        // Проверяем, является ли другой объект игроком
        if (other.gameObject.CompareTag("Player"))
        {
            // Увеличиваем счетчик монет
            scoreManager.AddScore(coinValue);

            // Уничтожаем объект монетки
            Destroy(gameObject);
        }
    }
}