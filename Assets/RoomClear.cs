using UnityEngine;

public class RoomClear : MonoBehaviour
{
    public GameObject Entry;
    public GameObject Exit;
    public Collider2D cameraBorder;
    public int roomIndex;
    public LevelGenerator levelGenerator;
    public bool isBossRoom = false; // New bool field to indicate if the room is a boss room
    public GameObject hatch; // Field for the hatch object

    private bool roomCleared = false;
    private bool roomActivated = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // Activate the room when the player enters
            if (!roomActivated)
            {
                levelGenerator.ActivateRoom(roomIndex);
                roomActivated = true;
            }
            else
            {
                levelGenerator.UpdateCameraConfiner(cameraBorder);
            }
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

            if (isBossRoom && hatch != null)
            {
                hatch.SetActive(true); // Activate the hatch if it's a boss room
            }

            roomCleared = true;
        }
    }

    private bool AreAllEnemiesDead()
    {
        Enemy[] enemies = GetComponentsInChildren<Enemy>();
        if (enemies.Length > 0)
        {
            return false;
        }

        Transform[] enemyObjects = GetComponentsInChildren<Transform>();
        foreach (Transform enemyObject in enemyObjects)
        {
            if (enemyObject != transform && enemyObject.CompareTag("Enemy"))
            {
                return false;
            }
        }

        return true;
    }
}