using UnityEngine;
using System.Collections.Generic;
using Unity.Cinemachine;

public class LevelGenerator : MonoBehaviour
{
    public GameObject[] roomPrefabs;
    public GameObject[] bossRoomPrefabs;
    public GameObject tunnelPrefab;
    public int baseNumberOfRooms = 5;
    public float roomWidth = 37f;
    public float tunnelLength = 8f;

    private List<RoomClear> rooms = new List<RoomClear>(); // Список комнат для быстрого доступа
    private CinemachineConfiner confiner;

    private void Start()
    {
        confiner = GameObject.Find("CinemachineCamera").GetComponent<CinemachineConfiner>();
        if (confiner == null)
        {
            Debug.LogError("CinemachineConfiner not found on CinemachineCamera!");
        }
        GenerateLevel();
    }

    // Вычисляем количество комнат на основе уровня
    private int CalculateNumberOfRooms()
    {
        int additionalRooms = GameManager.Instance.currentLevel / 3;
        return baseNumberOfRooms + additionalRooms;
    }

    public void GenerateLevel()
    {
        int numberOfRooms = CalculateNumberOfRooms(); // Получаем вычисленное количество комнат
        Vector2 lastRoomExit = Vector2.zero;
        RoomClear previousRoom = null;

        for (int i = 0; i < numberOfRooms; i++)
        {
            GameObject roomPrefab;
            bool isLastRoom = (i == numberOfRooms - 1);

            if (isLastRoom && bossRoomPrefabs.Length > 0)
            {
                roomPrefab = bossRoomPrefabs[Random.Range(0, bossRoomPrefabs.Length)];
            }
            else
            {
                roomPrefab = roomPrefabs[Random.Range(0, roomPrefabs.Length)];
            }

            Vector3 roomPosition = new Vector3(i * (roomWidth + tunnelLength), 0, 0);
            GameObject room = Instantiate(roomPrefab, roomPosition, Quaternion.identity);
            room.tag = "Room";

            DisableEnemies(room);

            RoomClear roomClear = room.GetComponent<RoomClear>();

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

            if (i == 0)
            {
                roomClear.Entry = null;
                UpdateCameraConfiner(roomClear.cameraBorder); // Обновляем границы камеры для первой комнаты
            }
            else
            {
                if (roomClear.Entry != null)
                {
                    roomClear.Entry.SetActive(false);
                }
            }

            roomClear.roomIndex = i;
            roomClear.levelGenerator = this;

            // Add the room to the list
            rooms.Add(roomClear); // Добавляем комнату в список

            if (i > 0)
            {
                Vector2 tunnelPosition = new Vector2((float)0.5 + roomPosition.x - roomWidth / 2 - tunnelLength / 2, roomPosition.y + GetRoomHeight(roomPrefab) / 2);
                GameObject tunnelGameObject = CreateCorridor(tunnelPosition);
                Tunnel tunnel = tunnelGameObject.GetComponent<Tunnel>();

                tunnel.previousRoom = previousRoom;
                tunnel.nextRoom = roomClear;
                tunnel.cameraBorder = FindDeepChild(tunnelGameObject.transform, "CameraBorder")?.GetComponent<Collider2D>();

                if (tunnel.cameraBorder == null)
                {
                    Debug.LogError("CameraBorder object not found in the tunnel!");
                }
            }

            lastRoomExit = new Vector2(roomPosition.x + roomWidth, roomPosition.y + GetRoomHeight(roomPrefab) / 2);
            previousRoom = roomClear;
        }
    }

    private void DisableEnemies(GameObject room)
    {
        EnemyRangeMove[] enemyRangeMoves = room.GetComponentsInChildren<EnemyRangeMove>();
        foreach (EnemyRangeMove enemyRangeMove in enemyRangeMoves)
        {
            enemyRangeMove.enabled = false;
        }
        EnemyMeleeMove[] enemyMeleeMoves = room.GetComponentsInChildren<EnemyMeleeMove>();
        foreach (EnemyMeleeMove enemyMeleeMove in enemyMeleeMoves)
        {
            enemyMeleeMove.enabled = false;
        }
        Boss[] boss = room.GetComponentsInChildren<Boss>();
        foreach (Boss enemyRangeMove in boss)
        {
            enemyRangeMove.enabled = false;
        }
        Enemy[] enemies = room.GetComponentsInChildren<Enemy>();
        foreach (Enemy enemy in enemies)
        {
            enemy.enabled = false;
        }

        EnemyRangeWeapon[] enemyRangeWeapons = room.GetComponentsInChildren<EnemyRangeWeapon>();
        foreach (EnemyRangeWeapon enemyRangeWeapon in enemyRangeWeapons)
        {
            enemyRangeWeapon.enabled = false;
        }
        EnemyMeleeWeapon[] enemyMeleeWeapons = room.GetComponentsInChildren<EnemyMeleeWeapon>();
        foreach (EnemyMeleeWeapon enemyMeleeWeapon in enemyMeleeWeapons)
        {
            enemyMeleeWeapon.enabled = false;
        }

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
        return Instantiate(tunnelPrefab, tunnelPosition, Quaternion.identity);
    }

    private float GetRoomHeight(GameObject roomPrefab)
    {
        Bounds bounds = roomPrefab.GetComponent<Collider2D>().bounds;
        return bounds.size.y;
    }
    private void EnableEnemies(GameObject room)
    {
        EnemyRangeMove[] enemyRangeMoves = room.GetComponentsInChildren<EnemyRangeMove>();
        foreach (EnemyRangeMove enemyRangeMove in enemyRangeMoves)
        {
            enemyRangeMove.enabled = true;
        }
        EnemyMeleeMove[] enemyMeleeMoves = room.GetComponentsInChildren<EnemyMeleeMove>();
        foreach (EnemyMeleeMove enemyMeleeMove in enemyMeleeMoves)
        {
            enemyMeleeMove.enabled = true;
        }
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
        EnemyRangeWeapon[] enemyRangeWeapons = room.GetComponentsInChildren<EnemyRangeWeapon>();
        foreach (EnemyRangeWeapon enemyRangeWeapon in enemyRangeWeapons)
        {
            enemyRangeWeapon.enabled = true;
        }
        EnemyMeleeWeapon[] enemyMeleeWeapons = room.GetComponentsInChildren<EnemyMeleeWeapon>();
        foreach (EnemyMeleeWeapon enemyMeleeWeapon in enemyMeleeWeapons)
        {
            enemyMeleeWeapon.enabled = true;
        }
        Transform[] enemyWeapons = room.GetComponentsInChildren<Transform>();
        foreach (Transform enemyWeapon in enemyWeapons)
        {
            if (enemyWeapon.CompareTag("EnemyWeapon"))
            {
                enemyWeapon.gameObject.SetActive(true);
            }
        }
    }

    // Вместо поиска комнаты по тегу, используем список rooms
    public void ActivateRoom(int roomIndex)
    {
        RoomClear roomToActivate = rooms.Find(room => room.roomIndex == roomIndex);

        if (roomToActivate == null)
        {
            Debug.LogError("Room with index " + roomIndex + " not found");
            return;
        }

        EnableEnemies(roomToActivate.gameObject);

        RoomClear roomClearComponent = roomToActivate;
        if (roomClearComponent != null && roomClearComponent.Entry != null)
        {
            roomClearComponent.Entry.SetActive(true);
        }
        UpdateCameraConfiner(roomClearComponent.cameraBorder);
    }

    public void UpdateCameraConfiner(Collider2D cameraBorder)
    {
        if (confiner != null)
        {
            confiner.m_BoundingShape2D = cameraBorder;
            confiner.InvalidateCache();
        }
        else
        {
            Debug.LogError("CinemachineConfiner is null. Make sure it's assigned in the Start method.");
        }
    }
}