using UnityEngine;

public class ResetLevelPrefs : MonoBehaviour
{
    public string damageLevelKey = "DamageLevel";
    public string healthLevelKey = "HealthLevel";
    public string playerScoreKey = "PlayerScore";
    public int defaultPlayerScore = 1000;
    public int defaultLevel = 1;

    [ContextMenu("Reset Level PlayerPrefs")]
    public void ResetLevelPlayerPrefs()
    {
        // Сбрасываем уровень урона
        PlayerPrefs.SetInt(damageLevelKey, defaultLevel);
        Debug.Log("PlayerPrefs '" + damageLevelKey + "' сброшен до " + defaultLevel);

        // Сбрасываем уровень здоровья
        PlayerPrefs.SetInt(healthLevelKey, defaultLevel);
        Debug.Log("PlayerPrefs '" + healthLevelKey + "' сброшен до " + defaultLevel);

        // Устанавливаем PlayerScore в 1000
        PlayerPrefs.SetInt(playerScoreKey, defaultPlayerScore);
        Debug.Log("PlayerPrefs '" + playerScoreKey + "' установлен в " + defaultPlayerScore);

        //Сохраняем изменения
        PlayerPrefs.Save();

        Debug.Log("PlayerPrefs успешно сброшены!");
    }
}