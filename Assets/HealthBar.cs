using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image healthBar;
    public Player player;

    void Start()
    {
        healthBar = GetComponent<Image>();
        player = FindFirstObjectByType<Player>();
    }

    public void UpdateHealthBar()
    {
        healthBar.fillAmount = (float)player.health / player.maxHP;
    }
}