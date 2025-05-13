using UnityEngine;

public class Heal : MonoBehaviour
{
    public int healValue = 10;

    void OnTriggerEnter2D(Collider2D other)
    {
        // Проверяем, является ли другой объект игроком
        if (other.gameObject.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            // Увеличиваем счетчик хилки
            player.TakeDamage(-healValue);

            // Уничтожаем объект хилки
            Destroy(gameObject);
        }
    }
}
