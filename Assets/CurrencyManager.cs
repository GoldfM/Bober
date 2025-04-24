using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class CurrencyManager : MonoBehaviour
{
    public int coins;
    public TextMeshProUGUI  coinText;
    void Start()
    {
        coins = PlayerPrefs.GetInt("Coins", 0); // Загрузка монет
        UpdateCoinText();
    }
    public void AddCoins(int amount)
    {
        coins += amount;
        UpdateCoinText();
        SaveCoins();
    }
    public void SubtractCoins(int amount)
    {
        if (CanAfford(amount))
        {
            coins -= amount;
            UpdateCoinText();
            SaveCoins();
        }
    }
    public bool CanAfford(int amount)
    {
        return coins >= amount;
    }
    void UpdateCoinText()
    {
        coinText.text = "Coins: " + coins;
    }
    void SaveCoins()
    {
        PlayerPrefs.SetInt("Coins", coins);
        PlayerPrefs.Save();
    }
}
