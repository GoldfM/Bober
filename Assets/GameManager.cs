using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public int currentLevel = 1;

    // Копии префабов оружия
    private GameObject StoredMeleeWeaponPrefabCopy;
    private GameObject StoredRangedWeaponPrefabCopy;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Подписываемся на событие загрузки сцены
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Этот метод вызывается при загрузке каждой сцены
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SpawnPlayer();
    }

    void SpawnPlayer()
    {
        GameObject playerStart = GameObject.FindGameObjectWithTag("PlayerStart");
        if (playerStart == null)
        {
            Debug.LogError("На сцене нет объекта с тегом 'PlayerStart'!");
            return;
        }

        // Ищем игрока по тегу "Player"
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("На сцене нет объекта с тегом 'Player'!");
            return;
        }

        // Перемещаем игрока к PlayerStart
        player.transform.position = playerStart.transform.position;
        player.transform.rotation = playerStart.transform.rotation;

        // Получаем PlayerInventory
        PlayerInventory inventory = player.GetComponent<PlayerInventory>();

        // Восстанавливаем HP игрока, если это первый уровень или первый запуск игры
        Player playerScript = player.GetComponent<Player>();
        if (currentLevel == 1)
        {
            playerScript.health = playerScript.maxHP;
        }
        else
        {
            playerScript.health = PlayerPrefs.GetInt("CurrentHealth", playerScript.maxHP);
        }
        playerScript.healthBar.UpdateHealthBar();

        if (currentLevel == 1)
            return;

        // Уничтожаем старое оружие, если оно есть
        if (inventory.meleeWeaponSlot != null)
        {
            Destroy(inventory.meleeWeaponSlot.gameObject);
        }
        if (inventory.rangedWeaponSlot != null)
        {
            Destroy(inventory.rangedWeaponSlot.gameObject);
        }

        // Создаем и добавляем новое оружие из сохраненных копий префабов
        if (StoredMeleeWeaponPrefabCopy != null)
        {
            GameObject meleeWeapon = Instantiate(StoredMeleeWeaponPrefabCopy);
            meleeWeapon.transform.SetParent(player.transform);
            meleeWeapon.transform.localPosition = Vector3.zero;
            inventory.meleeWeaponSlot = meleeWeapon.GetComponent<Weapon>();
        }
        else
        {
            Debug.Log("Melee weapon prefab copy is null");
        }
        if (StoredRangedWeaponPrefabCopy != null)
        {
            GameObject rangedWeapon = Instantiate(StoredRangedWeaponPrefabCopy);
            rangedWeapon.transform.SetParent(player.transform);
            rangedWeapon.transform.localPosition = Vector3.zero;
            inventory.rangedWeaponSlot = rangedWeapon.GetComponent<Weapon>();
        }
        else
        {
            Debug.Log("Ranged weapon prefab copy is null");
        }

        // Устанавливаем текущее оружие
        inventory.EquipWeapon(inventory.meleeWeaponSlot);
    }

    public void LoadLevelAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void CompleteLevel()
    {
        // Ищем игрока по тегу "Player"
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("На сцене нет объекта с тегом 'Player'!");
            return;
        }

        // Получаем PlayerInventory
        PlayerInventory inventory = player.GetComponent<PlayerInventory>();

        // Сохраняем копии префабов оружия
        SetMeleeWeaponPrefabCopy(inventory.meleeWeaponSlot.GetComponent<DefaultPrefab>().prefab);
        SetRangedWeaponPrefabCopy(inventory.rangedWeaponSlot.GetComponent<DefaultPrefab>().prefab);

        // Сохраняем HP игрока
        Player playerScript = player.GetComponent<Player>();
        playerScript.Heal(30);
        PlayerPrefs.SetInt("CurrentHealth", playerScript.health);
        PlayerPrefs.Save(); // Обязательно вызываем Save, чтобы данные сохранились на диск

        currentLevel++;
        LoadLevelAgain();
    }

    private void SetMeleeWeaponPrefabCopy(GameObject prefab)
    {
        if (StoredMeleeWeaponPrefabCopy != null)
        {
            Destroy(StoredMeleeWeaponPrefabCopy); // Уничтожаем старую копию
        }
        StoredMeleeWeaponPrefabCopy = Instantiate(prefab); // Создаем новую копию
        DontDestroyOnLoad(StoredMeleeWeaponPrefabCopy); // Помечаем как DontDestroyOnLoad
        StoredMeleeWeaponPrefabCopy.SetActive(false); // Отключаем, чтобы не отображалась на сцене
    }

    private void SetRangedWeaponPrefabCopy(GameObject prefab)
    {
        if (StoredRangedWeaponPrefabCopy != null)
        {
            Destroy(StoredRangedWeaponPrefabCopy); // Уничтожаем старую копию
        }
        StoredRangedWeaponPrefabCopy = Instantiate(prefab); // Создаем новую копию
        DontDestroyOnLoad(StoredRangedWeaponPrefabCopy); // Помечаем как DontDestroyOnLoad
        StoredRangedWeaponPrefabCopy.SetActive(false); // Отключаем, чтобы не отображалась на сцене
    }

    // Отписываемся от события при уничтожении объекта
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}