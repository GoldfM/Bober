using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradePanel : MonoBehaviour
{
    [Header("Base Stats - Damage")]
    public bool hasDamageUpgrade = true;
    public float baseDamageMultiplierIncrease = 0.2f;
    public int baseDamageUpgradeCost = 40;
    public TextMeshProUGUI damageLevelText;
    public TextMeshProUGUI damageValueText;
    public TextMeshProUGUI damageUpgradeCostText;
    public Button damageUpgradeButton;
    private int currentDamageLevel = 1;
    private int damageUpgradeCost;
    private const string DamageMultiplierKey = "DamageMultiplier";
    private const string DamageLevelKey = "DamageLevel";
    private const string DamageUpgradeCostKey = "DamageUpgradeCost";

    [Header("Base Stats - Health")]
    public bool hasHealthUpgrade = true;
    public int baseHealthIncrease = 50;
    public int baseHealthUpgradeCost = 40;
    public TextMeshProUGUI healthLevelText;
    public TextMeshProUGUI healthValueText;
    public TextMeshProUGUI healthUpgradeCostText;
    public Button healthUpgradeButton;
    private int currentHealthLevel = 1;
    private int healthUpgradeCost;
    private const string MaxHealthKey = "MaxHealth";
    private const string HealthLevelKey = "HealthLevel";
    private const string HealthUpgradeCostKey = "HealthUpgradeCost";

    [Header("Upgrade Configuration")]
    public float costIncreaseFactor = 1.5f;

    [Header("Balance Display")]
    public TextMeshProUGUI balanceText;
    private const string CurrencyKey = "PlayerScore";

    void Start()
    {
        LoadData();
        InitializeUpgradeCosts();
        UpdateUI();
    }

    private void InitializeUpgradeCosts()
    {
        if (hasDamageUpgrade)
        {
            damageUpgradeCost = PlayerPrefs.GetInt(DamageUpgradeCostKey, baseDamageUpgradeCost);
        }
        if (hasHealthUpgrade)
        {
            healthUpgradeCost = PlayerPrefs.GetInt(HealthUpgradeCostKey, baseHealthUpgradeCost);
        }
    }

    public void UpgradeDamage()
    {
        if (hasDamageUpgrade)
        {
            TryUpgrade(damageUpgradeCost, () =>
            {
                DamageMultiplier += baseDamageMultiplierIncrease;
                currentDamageLevel++;
                damageUpgradeCost = CalculateUpgradeCost(damageUpgradeCost);
                PlayerPrefs.SetInt(DamageUpgradeCostKey, damageUpgradeCost);
                PlayerPrefs.SetInt(DamageLevelKey, currentDamageLevel);
                PlayerPrefs.Save();
                return true;
            });
        }
    }

    public void UpgradeHealth()
    {
        if (hasHealthUpgrade)
        {
            TryUpgrade(healthUpgradeCost, () =>
            {
                MaxHealth += baseHealthIncrease;
                currentHealthLevel++;
                healthUpgradeCost = CalculateUpgradeCost(healthUpgradeCost);
                PlayerPrefs.SetInt(HealthUpgradeCostKey, healthUpgradeCost);
                PlayerPrefs.SetInt(HealthLevelKey, currentHealthLevel);
                PlayerPrefs.Save();
                return true;
            });
        }
    }

    private void TryUpgrade(int upgradeCost, System.Func<bool> upgradeAction)
    {
        if (CanAffordUpgrade(upgradeCost))
        {
            upgradeAction.Invoke();
            SubtractCurrency(upgradeCost);
            UpdateUI();
        }
        else
        {
            Debug.Log("Not enough currency to upgrade!");
        }
    }

    private bool CanAffordUpgrade(int upgradeCost)
    {
        return Currency >= upgradeCost;
    }

    private void SubtractCurrency(int cost)
    {
        Currency -= cost;
        PlayerPrefs.SetInt(CurrencyKey, Currency);
        PlayerPrefs.Save();
    }

    private int CalculateUpgradeCost(int currentCost)
    {
        return Mathf.RoundToInt(currentCost * costIncreaseFactor);
    }

    private void UpdateUI()
    {
        if (hasDamageUpgrade)
        {
            damageLevelText.text = "Lvl " + currentDamageLevel;
            damageValueText.text = DamageMultiplier.ToString("F2");
            damageUpgradeCostText.text = damageUpgradeCost.ToString();
        }

        if (hasHealthUpgrade)
        {
            healthLevelText.text = "Lvl " + currentHealthLevel;
            healthValueText.text = MaxHealth.ToString();
            healthUpgradeCostText.text = healthUpgradeCost.ToString();
        }

        balanceText.text = $"Баланс: {Currency}";
    }

    private void LoadData()
    {
        Currency = PlayerPrefs.GetInt(CurrencyKey, 500);
        DamageMultiplier = PlayerPrefs.GetFloat(DamageMultiplierKey, 1.0f);
        MaxHealth = PlayerPrefs.GetInt(MaxHealthKey, 100);
        currentDamageLevel = PlayerPrefs.GetInt(DamageLevelKey, 1);
        currentHealthLevel = PlayerPrefs.GetInt(HealthLevelKey, 1);
        PlayerPrefs.SetInt("CurrentHealth", MaxHealth);
    }

    public int Currency
    {
        get => PlayerPrefs.GetInt(CurrencyKey, 500);
        set
        {
            PlayerPrefs.SetInt(CurrencyKey, value);
            PlayerPrefs.Save();
        }
    }

    public float DamageMultiplier
    {
        get => PlayerPrefs.GetFloat(DamageMultiplierKey, 1.0f);
        set
        {
            PlayerPrefs.SetFloat(DamageMultiplierKey, value);
            PlayerPrefs.Save();
        }
    }

    public int MaxHealth
    {
        get => PlayerPrefs.GetInt(MaxHealthKey, 100);
        set
        {
            PlayerPrefs.SetInt(MaxHealthKey, value);
            PlayerPrefs.Save();
        }
    }

        public int Health
    {
        get => PlayerPrefs.GetInt("Health", 100);
        set
        {
            PlayerPrefs.SetInt("Health", value);
            PlayerPrefs.Save();
        }
    }
}