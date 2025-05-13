using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public int healAmount = -1; // Отрицательный урон для хилки

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Player>().TakeDamage(healAmount);
            Destroy(gameObject); // Удаление хилки после использования
        }
    }
}
