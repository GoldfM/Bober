using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class UpgradeManager : MonoBehaviour
{
    public int damageLevel = 1;
    public int healthLevel = 1;
    public int damageCost = 10;
    public int healthCost = 15;
    public CurrencyManager currencyManager;
    public TextMeshProUGUI  damageLevelText;
    public TextMeshProUGUI  healthLevelText;
    public TextMeshProUGUI  damageCostText;
    public TextMeshProUGUI  healthCostText;
    //public PlayerController playerController;
    void Start()
    {
        
        LoadUpgrades();
        UpdateUI();
    }
    public void UpgradeDamage()
    {
        if (currencyManager.CanAfford(damageCost))
        {
            currencyManager.SubtractCoins(damageCost);
            damageLevel++;
            //playerController.SetDamageMultiplier(1 + damageLevel * 0.2f); // Увеличение урона
            damageCost += 5;
            UpdateUI();
            SaveUpgrades();
        }
    }
    public void UpgradeHealth()
    {
        if (currencyManager.CanAfford(healthCost))
        {
            currencyManager.SubtractCoins(healthCost);
            healthLevel++;
            // Реализация увеличения здоровья
            healthCost += 5;
            UpdateUI();
            SaveUpgrades();
        }
    }
    void UpdateUI()
    {
        damageLevelText.text = "Damage Level: " + damageLevel;
        healthLevelText.text = "Health Level: " + healthLevel;
        damageCostText.text = "Damage Cost: " + damageCost;
        healthCostText.text = "Health Cost: " + healthCost;
    }
    void SaveUpgrades()
    {
        PlayerPrefs.SetInt("DamageLevel", damageLevel);
        PlayerPrefs.SetInt("HealthLevel", healthLevel);
        PlayerPrefs.Save();
    }
    void LoadUpgrades()
    {
        damageLevel = PlayerPrefs.GetInt("DamageLevel", 1);
        healthLevel = PlayerPrefs.GetInt("HealthLevel", 1);
        //playerController.SetDamageMultiplier(1 + damageLevel * 0.2f);
    }
}
