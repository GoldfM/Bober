using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    public Slider slider;
    public bool isMusicSlider; // Флаг, чтобы определить, какой это слайдер

    private AudioManager audioManager; // Кэшированная ссылка на AudioManager

    void Start()
    {
        // Находим AudioManager только один раз при создании слайдера
        audioManager = FindObjectOfType<AudioManager>();

        if (audioManager == null)
        {
            Debug.LogWarning("AudioManager не найден на сцене!");
            return;
        }

        slider.onValueChanged.AddListener(OnSliderValueChanged);

        // Устанавливаем начальное значение слайдера
        if (isMusicSlider)
        {
            slider.value = audioManager.musicVolume;
        }
        else
        {
            slider.value = audioManager.soundsVolume;
        }
    }

    void OnSliderValueChanged(float value)
    {
        if (audioManager != null)
        {
            if (isMusicSlider)
            {
                audioManager.SetMusicVolume(value);
            }
            else
            {
                audioManager.SetSoundsVolume(value);
            }
        }
    }
}