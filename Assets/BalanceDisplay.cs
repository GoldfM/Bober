using UnityEngine;
using TMPro;

public class BalanceDisplay : MonoBehaviour
{
    public TextMeshProUGUI balanceText; // Ссылка на текстовое поле TextMeshPro

    private const string PlayerScoreKey = "PlayerScore"; // Ключ для PlayerPrefs

    void Start()
    {
        UpdateBalanceDisplay();
    }

    private void UpdateBalanceDisplay()
    {
        int playerScore = PlayerPrefs.GetInt(PlayerScoreKey, 400); // Получаем баланс из PlayerPrefs
        balanceText.text = $"Баланс: {playerScore}"; // Обновляем текстовое поле
    }
}