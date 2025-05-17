using UnityEngine;

public class Coin : MonoBehaviour
{
    public int coinValue = 1; // Количество монет, добавляемое при подборе
    private ScoreManager scoreManager; // Ссылка на ScoreManager
    public AudioClip coinSound; // Звук при подборе монеты
    private AudioManager audioManager; // Ссылка на AudioManager

    void Start()
    {
        // Получаем ссылку на ScoreManager
        scoreManager = FindObjectOfType<ScoreManager>();

        if (scoreManager == null)
        {
            Debug.LogError("ScoreManager не найден на сцене!");
        }

        // Получаем ссылку на AudioManager
        audioManager = FindObjectOfType<AudioManager>();

        if (audioManager == null)
        {
            Debug.LogError("AudioManager не найден на сцене!");
        }
    }

    // Метод, вызываемый при столкновении с другим объектом
    void OnTriggerEnter2D(Collider2D other)
    {
        // Проверяем, является ли другой объект игроком
        if (other.gameObject.CompareTag("Player"))
        {
            // Воспроизводим звук
            if (coinSound != null && audioManager != null)
            {
                // Создаем временный AudioSource
                GameObject tempGO = new GameObject("TempAudio"); // create the temp object
                AudioSource tempAS = tempGO.AddComponent<AudioSource>(); // add an audio source
                tempAS.volume = 1f;
                //Воспроизводим звук
                audioManager.PlayOneShotSound(tempAS, coinSound);
                Destroy(tempGO, coinSound.length); // Destroy temporary object after playing sound
            }

            // Увеличиваем счетчик монет
            scoreManager.AddScore(coinValue);

            // Уничтожаем объект монетки
            Destroy(gameObject);
        }
    }
}