using UnityEngine;

public class RoomClear : MonoBehaviour
{
    public GameObject Entry;
    public GameObject Exit;
    public int roomIndex; // Индекс комнаты
    public LevelGenerator levelGenerator; // Ссылка на LevelGenerator

    private bool roomCleared = false;
    private bool roomActivated = false; // Флаг, чтобы комната активировалась только один раз

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!roomActivated && other.gameObject.CompareTag("Player"))
        {
            // Activate the room when the player enters
            levelGenerator.ActivateRoom(roomIndex);
            roomActivated = true; // Устанавливаем флаг, чтобы комната не активировалась снова

            // Disable the trigger after activation
            GetComponent<Collider2D>().enabled = false;
        }
    }

    void Update()
    {
        if (roomCleared) return;

        if (AreAllEnemiesDead())
        {
            if (Entry != null)
            {
                Entry.SetActive(false);
            }
            if (Exit != null)
            {
                Exit.SetActive(false);
            }
            roomCleared = true;
        }
    }

    private bool AreAllEnemiesDead()
    {
        // Check for Enemy components
        Enemy[] enemies = GetComponentsInChildren<Enemy>();
        if (enemies.Length > 0)
        {
            return false; // Enemies with the Enemy component still exist
        }

        // Check for GameObjects with the tag "Enemy"
        Transform[] enemyObjects = GetComponentsInChildren<Transform>();
        foreach (Transform enemyObject in enemyObjects)
        {
            if (enemyObject != transform && enemyObject.CompareTag("Enemy"))
            {
                return false; // Found an enemy with the "Enemy" tag
            }
        }

        return true; // No enemies found by either method
    }
}
