using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class CurrencyManager : MonoBehaviour
{
    public int coins;
    public TextMeshProUGUI  coinText;
    void Start()
    {
        coins = PlayerPrefs.GetInt("PlayerScore", 150); // Загрузка монет
        Debug.Log(coins);
        Debug.Log(coins);
        Debug.Log(coins);
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
        PlayerPrefs.SetInt("PlayerScore", coins);
        PlayerPrefs.Save();
    }
}
