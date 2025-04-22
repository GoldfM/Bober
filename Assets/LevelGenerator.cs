using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public GameObject[] roomPrefabs; // Массив префабов комнат
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

        // Генерация уровня с несколькими комнатами
        for (int i = 0; i < numberOfRooms; i++)
        {
            // Случайный выбор комнаты
            GameObject roomPrefab = roomPrefabs[Random.Range(0, roomPrefabs.Length)];

            // Позиционирование для каждой комнаты
            Vector3 roomPosition = new Vector3(i * (roomWidth + tunnelLength), 0, 0);
            GameObject room = Instantiate(roomPrefab, roomPosition, Quaternion.identity);

            if (i > 0)
            {
                // Определяем позицию для создания моста
                Vector2 tunnelPosition = new Vector2(roomPosition.x - roomWidth / 2 - tunnelLength / 2, roomPosition.y + GetRoomHeight(roomPrefab) / 2);
                CreateCorridor(tunnelPosition);
            }

            // Обновляем позицию выхода для следующей комнаты
            lastRoomExit = new Vector2(roomPosition.x + roomWidth, roomPosition.y + GetRoomHeight(roomPrefab) / 2);
        }
    }

    private void CreateCorridor(Vector2 tunnelPosition)
    {
        // Инстанцируем туннель в заданной позиции без вращения и масштабирования
        Instantiate(tunnelPrefab, tunnelPosition, Quaternion.identity);
    }

    // Метод для получения высоты комнаты
    private float GetRoomHeight(GameObject roomPrefab)
    {
        // Получаем размеры через 2D Collider
        Bounds bounds = roomPrefab.GetComponent<Collider2D>().bounds;
        return bounds.size.y; // Возвращаем высоту
    }
}
