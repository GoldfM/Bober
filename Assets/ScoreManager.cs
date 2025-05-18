using UnityEngine;
using TMPro; // Подключаем пространство имен TextMeshPro

public class ScoreManager : MonoBehaviour
{
    public int score = 150; // Текущий счет
    public TMP_Text scoreText; // Ссылка на TextMeshPro Text
    private const string ScoreKey = "PlayerScore"; // Ключ для сохранения счета

    void Start()
    {
        // Загружаем сохраненный счет
        LoadScore();

        // Обновляем текст при старте игры
        UpdateScoreText();
    }

    // Метод для добавления очков
    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreText();
        SaveScore();
    }

    // Метод для обновления текста
    private void UpdateScoreText()
    {
        scoreText.text = score.ToString();
    }

    // Метод для сохранения счета
    private void SaveScore()
    {
        PlayerPrefs.SetInt(ScoreKey, score);
        PlayerPrefs.Save();
        Debug.Log("Score saved: " + score);
    }

    // Метод для загрузки счета
    private void LoadScore()
    {
        // Загрузка из PlayerPrefs
        if (PlayerPrefs.HasKey(ScoreKey))
        {
            score = PlayerPrefs.GetInt(ScoreKey);
            Debug.Log(score);
        }
        else
        {
            score = 150; // Если нет сохраненного счета, начинаем с нуля
            Debug.Log("No score saved, starting from 0");
        }
    }
}