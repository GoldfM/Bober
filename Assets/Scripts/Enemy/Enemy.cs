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

    private Transform levelParent; // Ссылка на объект Level, теперь приватная
    void Awake() // Use Awake to ensure levelParent is set before other Start() methods
    {
        levelParent = transform.parent; // Assign the parent of the enemy to levelParent
    }


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
        GameObject drop1 = null; // Объявляем переменную drop
        if (randomValue < weaponDropChance)
        {
            if (weaponPrefabs != null && weaponPrefabs.Length > 0)
            {
                GameObject weaponToDrop = weaponPrefabs[Random.Range(0, weaponPrefabs.Length)];
                drop1 = Instantiate(weaponToDrop, transform.position, Quaternion.identity);
                
            }
        }
        else if (randomValue < healthDropChance)
        {
            drop1 = Instantiate(healthPickupPrefab, transform.position, Quaternion.identity);
        }
        if (randomValue < coinDropChance)
        {
            drop1 = Instantiate(coinPrefab, transform.position, Quaternion.identity);
        }
        if (drop1 != null && levelParent != null)
    {
        Debug.Log("Drop Position before parenting: " + drop1.transform.position);
        Debug.Log("Level Parent Position: " + levelParent.position);
        drop1.transform.SetParent(levelParent, worldPositionStays: true);
        Debug.Log("Drop Position after parenting: " + drop1.transform.position);
    }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Player>().TakeDamage(damage);
        }
    }
}