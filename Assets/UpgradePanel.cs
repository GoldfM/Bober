using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class UpgradePanel : MonoBehaviour
{
 public float baseDamageMultiplierIncrease = 0.1f;
 public int baseHealthIncrease = 50;
 public int baseUpgradeCost = 40;


 public float costIncreaseFactor = 1.5f;


 public TextMeshProUGUI damageLevelText;
 public TextMeshProUGUI damageValueText;
 public TextMeshProUGUI damageUpgradeCostText;
 public Button damageUpgradeButton;


 public TextMeshProUGUI healthLevelText;
 public TextMeshProUGUI healthValueText;
 public TextMeshProUGUI healthUpgradeCostText;
 public Button healthUpgradeButton;


 private int currentDamageLevel = 1;
 private int currentHealthLevel = 1;
 private int damageUpgradeCost;
 private int healthUpgradeCost;


 private const string CurrencyKey = "PlayerScore";
 private const string DamageMultiplierKey = "DamageMultiplier";
 private const string DamageKey = "Damage";
 private const string MaxHealthKey = "MaxHealth";
 private const string HealthKey = "Health";
 private const string DamageUpgradeCostKey = "DamageUpgradeCost";
 private const string HealthUpgradeCostKey = "HealthUpgradeCost";


 void Start()
 {
 LoadData();
 PlayerPrefs.SetInt(CurrencyKey,500);
 PlayerPrefs.Save();
 if (!PlayerPrefs.HasKey(DamageUpgradeCostKey))
 {
 damageUpgradeCost = baseUpgradeCost;
 healthUpgradeCost = baseUpgradeCost;
 }
 else
 {
 damageUpgradeCost = PlayerPrefs.GetInt(DamageUpgradeCostKey);
 healthUpgradeCost = PlayerPrefs.GetInt(HealthUpgradeCostKey);
 }
 UpdateUI();
 }


 public void UpgradeDamage()
 {
 if (CanAffordUpgrade(damageUpgradeCost))
 {
 DamageMultiplier += baseDamageMultiplierIncrease;
 Damage += 50;
 SubtractCurrency(damageUpgradeCost);
 currentDamageLevel++;
 damageUpgradeCost = CalculateUpgradeCost(damageUpgradeCost);
 PlayerPrefs.SetInt(DamageUpgradeCostKey, damageUpgradeCost);
 PlayerPrefs.Save();
 UpdateUI();
 }
 else
 {
 Debug.Log("Not enough currency to upgrade damage!");
 }
 }


 public void UpgradeHealth()
 {
 if (CanAffordUpgrade(healthUpgradeCost))
 {
 MaxHealth += baseHealthIncrease;
 Health += baseHealthIncrease;
 SubtractCurrency(healthUpgradeCost);
 currentHealthLevel++;
 healthUpgradeCost = CalculateUpgradeCost(healthUpgradeCost);
 PlayerPrefs.SetInt(HealthUpgradeCostKey, healthUpgradeCost);
 PlayerPrefs.Save();
 UpdateUI();
 }
 else
 {
 Debug.Log("Not enough currency to upgrade health!");
 }
 }


 private bool CanAffordUpgrade(int upgradeCost)
 {
 return Currency >= upgradeCost;
 }


 private void SubtractCurrency(int cost)
 {
 Currency -= cost;
 PlayerPrefs.SetInt(CurrencyKey,Currency);
 PlayerPrefs.Save();
 }


 private int CalculateUpgradeCost(int currentCost)
 {
 return Mathf.RoundToInt(currentCost * costIncreaseFactor);
 }


 private void UpdateUI()
 {
 damageLevelText.text = "Lvl " + currentDamageLevel;
 damageValueText.text = Damage.ToString();
 damageUpgradeCostText.text = damageUpgradeCost.ToString();


 healthLevelText.text = "Lvl " + currentHealthLevel;
 healthValueText.text = MaxHealth.ToString();
 healthUpgradeCostText.text = healthUpgradeCost.ToString();
 }


 private void LoadData()
 {
 Currency = PlayerPrefs.GetInt(CurrencyKey, 500);
 DamageMultiplier = PlayerPrefs.GetFloat(DamageMultiplierKey, 1.0f);
 Damage = PlayerPrefs.GetInt(DamageKey, 100);
 MaxHealth = PlayerPrefs.GetInt(MaxHealthKey, 40);
 Health = PlayerPrefs.GetInt(HealthKey, 40);
 }


 public int Currency
 {
 get { return PlayerPrefs.GetInt(CurrencyKey, 500); }
 set
 {
 PlayerPrefs.SetInt(CurrencyKey, value);
 PlayerPrefs.Save();
 }
 }


 public float DamageMultiplier
 {
 get { return PlayerPrefs.GetFloat(DamageMultiplierKey, 1.0f); }
 set
 {
 PlayerPrefs.SetFloat(DamageMultiplierKey, value);
 PlayerPrefs.Save();
 }
 }


 public int Damage
 {
 get { return PlayerPrefs.GetInt(DamageKey, 100); }
 set
 {
 PlayerPrefs.SetInt(DamageKey, value);
 PlayerPrefs.Save();
 }
 }


 public int MaxHealth
 {
 get { return PlayerPrefs.GetInt(MaxHealthKey, 40); }
 set
 {
 PlayerPrefs.SetInt(MaxHealthKey, value);
 PlayerPrefs.Save();
 }
 }


 public int Health
 {
 get { return PlayerPrefs.GetInt(HealthKey, 40); }
 set
 {
 PlayerPrefs.SetInt(HealthKey, value);
 PlayerPrefs.Save();
 }
 }
}