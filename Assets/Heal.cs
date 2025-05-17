using UnityEngine;

public class Heal : MonoBehaviour
{
    public int healValue = 10;
    public AudioClip healSound; // Звук при подборе хилки
    private AudioManager audioManager; // Ссылка на AudioManager

    void Start()
    {
        // Получаем ссылку на AudioManager
        audioManager = FindObjectOfType<AudioManager>();

        if (audioManager == null)
        {
            Debug.LogError("AudioManager не найден на сцене!");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Проверяем, является ли другой объект игроком
        if (other.gameObject.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            // Увеличиваем счетчик хилки
            player.Heal(healValue);

            // Воспроизводим звук
            if (healSound != null && audioManager != null)
            {
                // Создаем временный AudioSource
                GameObject tempGO = new GameObject("TempAudio"); // create the temp object
                AudioSource tempAS = tempGO.AddComponent<AudioSource>(); // add an audio source
                tempAS.volume = 1f;
                //Воспроизводим звук
                audioManager.PlayOneShotSound(tempAS, healSound);
                Destroy(tempGO, healSound.length); // Destroy temporary object after playing sound
            }

            // Уничтожаем объект хилки
            Destroy(gameObject);
        }
    }
}