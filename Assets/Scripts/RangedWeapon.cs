using UnityEngine;

public class RangedWeapon : Weapon
{
    public override WeaponType Type => WeaponType.Ranged;
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float fireRate = 0.5f;
    private float nextFire = 0f;
    public int damage = 10; // Базовый урон оружия
    public AudioClip attackSound; // Звук атаки
    private AudioSource audioSource;
    private AudioManager audioManager; // Ссылка на AudioManager

    void Start()
    {
        // Получаем компонент AudioSource или добавляем его, если его нет
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.tag = "Sound";
        }

        // Получаем экземпляр AudioManager через GameObject.Find
        GameObject audioManagerObject = GameObject.Find("AudioManager");
        if (audioManagerObject != null)
        {
            audioManager = audioManagerObject.GetComponent<AudioManager>();
            if (audioManager == null)
            {
                Debug.LogError("AudioManager компонент не найден на объекте AudioManager!");
            }
        }
        else
        {
            Debug.LogError("Объект AudioManager не найден на сцене!");
        }
    }

    void Update()
    {
        if (Input.GetMouseButton(0) && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            Attack();
        }
    }

    public override void Attack()
    {
        // Создаем пулю
        GameObject bullet = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

        // Получаем компонент Bullet и устанавливаем базовый урон
        Bullet bulletComponent = bullet.GetComponent<Bullet>();
        bulletComponent.damage = damage; // Устанавливаем урон пули

        // Воспроизводим звук атаки, если он задан и AudioManager найден
        if (attackSound != null && audioManager != null)
        {
            audioManager.PlayOneShotSound(audioSource, attackSound);
        }
    }
}