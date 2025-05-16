using UnityEngine;
using System.Collections.Generic;
using Unity.Cinemachine;

public class LevelGenerator : MonoBehaviour
{
    public GameObject[] roomPrefabs; // Массив префабов комнат
    public GameObject[] bossRoomPrefabs;
    public GameObject tunnelPrefab; // Префаб коридора
    public int numberOfRooms = 5; // Количество комнат в уровне
    public float roomWidth = 37f; // Фиксированная ширина комнаты
    public float tunnelLength = 8f; // Фиксированная длина моста

    private void Start()
    {
        GenerateLevel();
    }

    public void GenerateLevel()
    {
        // Начальное положение для первой комнаты
        Vector2 lastRoomExit = Vector2.zero;
        RoomClear previousRoom = null;

        // Генерация уровня с несколькими комнатами
        for (int i = 0; i < numberOfRooms; i++)
        {
            GameObject roomPrefab;
            bool isLastRoom = (i == numberOfRooms - 1); // Проверяем, является ли комната последней

            // Если это последняя комната, выбираем комнату босса
            if (isLastRoom && bossRoomPrefabs.Length > 0)
            {
                roomPrefab = bossRoomPrefabs[Random.Range(0, bossRoomPrefabs.Length)];
            }
            else
            {
                roomPrefab = roomPrefabs[Random.Range(0, roomPrefabs.Length)];
            }

            // Позиционирование для каждой комнаты
            Vector3 roomPosition = new Vector3(i * (roomWidth + tunnelLength), 0, 0);
            GameObject room = Instantiate(roomPrefab, roomPosition, Quaternion.identity);
            room.tag = "Room"; // Важно установить тег для комнаты

            // Disable enemies in the room immediately after instantiation
            DisableEnemies(room);

            // Get existing RoomClear component
            RoomClear roomClear = room.GetComponent<RoomClear>();

            // Find Entry and Exit objects - adjust the names to match your prefabs
            roomClear.Entry = FindDeepChild(room.transform, "Entry")?.gameObject;
            roomClear.Exit = FindDeepChild(room.transform, "Exit")?.gameObject;
            roomClear.cameraBorder = FindDeepChild(room.transform, "CameraBorder")?.GetComponent<Collider2D>();

            if (roomClear.Entry == null)
            {
                Debug.LogError("Entry object not found in the room!");
            }
            if (roomClear.Exit == null)
            {
                Debug.LogError("Exit object not found in the room!");
            }
            if (roomClear.cameraBorder == null)
            {
                Debug.LogError("CameraBorder object not found in the room!");
            }

            // Handle Entry for the first room
            if (i == 0)
            {
                // Set Entry to null for the first room
                roomClear.Entry = null;
                UpdateCameraConfiner(roomClear.cameraBorder);
            }
            else
            {
                // Disable the Entry GameObject for other rooms
                if (roomClear.Entry != null)
                {
                    roomClear.Entry.SetActive(false);
                }
            }

            // Set room index and level generator reference
            roomClear.roomIndex = i;
            roomClear.levelGenerator = this;

            // Создаем туннель, если это не первая комната
            if (i > 0)
            {
                // Определяем позицию для создания моста
                Vector2 tunnelPosition = new Vector2((float)0.5 + roomPosition.x - roomWidth / 2 - tunnelLength / 2, roomPosition.y + GetRoomHeight(roomPrefab) / 2);
                GameObject tunnelGameObject = CreateCorridor(tunnelPosition);
                Tunnel tunnel = tunnelGameObject.GetComponent<Tunnel>();

                // Set previous and next room references
                tunnel.previousRoom = previousRoom;
                tunnel.nextRoom = roomClear;
                tunnel.cameraBorder = FindDeepChild(tunnelGameObject.transform, "CameraBorder")?.GetComponent<Collider2D>();

                if (tunnel.cameraBorder == null)
                {
                    Debug.LogError("CameraBorder object not found in the tunnel!");
                }

                // Set the camera confiner to the tunnel's camera border on creation
                // This assumes the player might start in a tunnel
                // Consider moving this logic to the tunnel's OnTriggerEnter2D
                //UpdateCameraConfiner(tunnel.cameraBorder);
            }

            // Обновляем позицию выхода для следующей комнаты
            lastRoomExit = new Vector2(roomPosition.x + roomWidth, roomPosition.y + GetRoomHeight(roomPrefab) / 2);
            previousRoom = roomClear;
        }
    }

    private void DisableEnemies(GameObject room)
    {
        // Get all EnemyRangeMove components in the room
        EnemyRangeMove[] enemyRangeMoves = room.GetComponentsInChildren<EnemyRangeMove>();
        foreach (EnemyRangeMove enemyRangeMove in enemyRangeMoves)
        {
            enemyRangeMove.enabled = false;
        }
        Boss[] boss = room.GetComponentsInChildren<Boss>();
        foreach (Boss enemyRangeMove in boss)
        {
            enemyRangeMove.enabled = false;
        }
        // Get all Enemy components in the room
        Enemy[] enemies = room.GetComponentsInChildren<Enemy>();
        foreach (Enemy enemy in enemies)
        {
            enemy.enabled = false;
        }

        // Get all EnemyRangeWeapon components in the room
        EnemyRangeWeapon[] enemyRangeWeapons = room.GetComponentsInChildren<EnemyRangeWeapon>();
        foreach (EnemyRangeWeapon enemyRangeWeapon in enemyRangeWeapons)
        {
            enemyRangeWeapon.enabled = false;
        }

        // Get all GameObjects with the tag "EnemyWeapon" in the room
        Transform[] enemyWeapons = room.GetComponentsInChildren<Transform>();
        foreach (Transform enemyWeapon in enemyWeapons)
        {
            if (enemyWeapon.CompareTag("EnemyWeapon"))
            {
                enemyWeapon.gameObject.SetActive(false);
            }
        }
    }

    private Transform FindDeepChild(Transform parent, string name)
    {
        var result = parent.Find(name);
        if (result != null)
            return result;
        foreach (Transform child in parent)
        {
            result = FindDeepChild(child, name);
            if (result != null)
                return result;
        }
        return null;
    }

    private GameObject CreateCorridor(Vector2 tunnelPosition)
    {
        // Инстанцируем туннель в заданной позиции без вращения и масштабирования
        return Instantiate(tunnelPrefab, tunnelPosition, Quaternion.identity);
    }

    // Метод для получения высоты комнаты
    private float GetRoomHeight(GameObject roomPrefab)
    {
        // Получаем размеры через 2D Collider
        Bounds bounds = roomPrefab.GetComponent<Collider2D>().bounds;
        return bounds.size.y; // Возвращаем высоту
    }
    private void EnableEnemies(GameObject room)
    {
        // Get all EnemyRangeMove components in the room
        EnemyRangeMove[] enemyRangeMoves = room.GetComponentsInChildren<EnemyRangeMove>();
        foreach (EnemyRangeMove enemyRangeMove in enemyRangeMoves)
        {
            enemyRangeMove.enabled = true;
        }
        // Get all Enemy components in the room
        Enemy[] enemies = room.GetComponentsInChildren<Enemy>();
        foreach (Enemy enemy in enemies)
        {
            enemy.enabled = true;
        }

        Boss[] boss = room.GetComponentsInChildren<Boss>();
        foreach (Boss enemy in boss)
        {
            enemy.enabled = true;
        }
        // Get all EnemyRangeWeapon components in the room
        EnemyRangeWeapon[] enemyRangeWeapons = room.GetComponentsInChildren<EnemyRangeWeapon>();
        foreach (EnemyRangeWeapon enemyRangeWeapon in enemyRangeWeapons)
        {
            enemyRangeWeapon.enabled = true;
        }
        // Get all GameObjects with the tag "EnemyWeapon" in the room
        Transform[] enemyWeapons = room.GetComponentsInChildren<Transform>();
        foreach (Transform enemyWeapon in enemyWeapons)
        {
            if (enemyWeapon.CompareTag("EnemyWeapon"))
            {
                enemyWeapon.gameObject.SetActive(true);
            }
        }
    }
    // Call this method when the player enters a room
    public void ActivateRoom(int roomIndex)
    {
        // Find the room by index
        GameObject[] rooms = GameObject.FindGameObjectsWithTag("Room");
        GameObject roomToActivate = null;
        foreach (GameObject room in rooms)
        {
            RoomClear roomClear = room.GetComponent<RoomClear>();
            if (roomClear != null && roomClear.roomIndex == roomIndex)
            {
                roomToActivate = room;
                break;
            }
        }

        if (roomToActivate == null)
        {
            Debug.LogError("Room with index " + roomIndex + " not found");
            return;
        }

        // Activate enemies in the room
        EnableEnemies(roomToActivate);

        // Activate Entry, if it's not null
        RoomClear roomClearComponent = roomToActivate.GetComponent<RoomClear>();
        if (roomClearComponent != null && roomClearComponent.Entry != null)
        {
            roomClearComponent.Entry.SetActive(true);
        }
        UpdateCameraConfiner(roomClearComponent.cameraBorder);
    }

    public void UpdateCameraConfiner(Collider2D cameraBorder)
    {
        CinemachineConfiner confiner = GameObject.Find("CinemachineCamera").GetComponent<CinemachineConfiner>();
        confiner.m_BoundingShape2D = cameraBorder;
        confiner.InvalidateCache();
    }
}