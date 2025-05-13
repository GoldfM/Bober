using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 1;
    public int damage = 1;

    [Header("Настройки дропа")]
    public GameObject coinPrefab; // Префаб монеты
    public GameObject healthPickupPrefab; // Префаб хилки
    public GameObject[] weaponPrefabs; // Массив префабов оружия

    [Range(0, 1)] public float coinDropChance = 0.3f; // Вероятность выпадения монеты
    [Range(0, 1)] public float healthDropChance = 0.2f; // Вероятность выпадения хилки
    [Range(0, 1)] public float weaponDropChance = 0.1f; // Вероятность выпадения оружия

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            DropItem();
            Destroy(gameObject);
        }
    }

    void DropItem()
    {
        float randomValue = Random.value;

        if (randomValue < coinDropChance)
        {
            Instantiate(coinPrefab, transform.position, Quaternion.identity);
        }
        else if (randomValue < coinDropChance + healthDropChance)
        {
            Instantiate(healthPickupPrefab, transform.position, Quaternion.identity);
        }
        else if (randomValue < coinDropChance + healthDropChance + weaponDropChance)
        {
            if (weaponPrefabs != null && weaponPrefabs.Length > 0)
            {
                // Выбираем случайное оружие из списка
                GameObject weaponToDrop = weaponPrefabs[Random.Range(0, weaponPrefabs.Length)];
                Instantiate(weaponToDrop, transform.position, Quaternion.identity);
            }
        }
        // Если ничего не выпало, ничего не делаем
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Player>().TakeDamage(damage);
        }
    }
}