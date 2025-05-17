using UnityEngine;
using UnityEngine.UI;
using TMPro; // Подключаем пространство имен TextMeshPro

public class HealthBar : MonoBehaviour
{
    public Image healthBar;
    public Player player;
    public TextMeshProUGUI healthText; // Добавляем ссылку на TextMeshProUGUI

    void Start()
    {
        healthBar = GetComponent<Image>();
        player = FindFirstObjectByType<Player>();

        // Загружаем максимальное здоровье из PlayerPrefs
        player.maxHP = PlayerPrefs.GetInt("MaxHealth", 100); // 100 - значение по умолчанию, если в PlayerPrefs ничего нет

        // Загружаем текущее здоровье из PlayerPrefs, если оно есть, иначе устанавливаем максимальное
        player.health = PlayerPrefs.GetInt("CurrentHealth", player.maxHP);

        UpdateHealthBar(); // Обновляем HealthBar и текст при старте
    }

    public void UpdateHealthBar()
    {
        healthBar.fillAmount = (float)player.health / player.maxHP;
        UpdateHealthText(); // Обновляем текст при каждом изменении здоровья
    }

    private void UpdateHealthText()
    {
        // Обновляем текстовое отображение здоровья
        if (healthText != null)
        {
            healthText.text = player.health + " / " + player.maxHP;
        }
    }
}