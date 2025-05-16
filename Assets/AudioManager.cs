using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public float musicVolume = 0.5f;
    public float soundsVolume = 0.75f;

    public Slider musicSlider;
    public Slider soundsSlider;

    private const string MusicVolumeKey = "MusicVolume";
    private const string SoundsVolumeKey = "SoundsVolume";

    public static AudioManager Instance { get; private set; } // Singleton instance

    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep AudioManager between scenes
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void OnEnable()
    {
        // Find Sliders by name
        GameObject musicSliderObject = GameObject.Find("SliderMusic");
        if (musicSliderObject != null)
        {
            musicSlider = musicSliderObject.GetComponent<Slider>();
            if (musicSlider == null)
            {
                Debug.LogError("Slider компонент не найден на объекте SliderMusic!");
            }
        }
        else
        {
            Debug.LogError("Объект SliderMusic не найден на сцене!");
        }

        GameObject soundsSliderObject = GameObject.Find("SliderSound");
        if (soundsSliderObject != null)
        {
            soundsSlider = soundsSliderObject.GetComponent<Slider>();
            if (soundsSlider == null)
            {
                Debug.LogError("Slider компонент не найден на объекте SliderSound!");
            }
        }
        else
        {
            Debug.LogError("Объект SliderSound не найден на сцене!");
        }

        // Load saved volumes
        musicVolume = PlayerPrefs.GetFloat(MusicVolumeKey, musicVolume);
        soundsVolume = PlayerPrefs.GetFloat(SoundsVolumeKey, soundsVolume);

        // Set slider values (if sliders are assigned)
        if (musicSlider != null)
        {
            musicSlider.value = musicVolume;
        }
        if (soundsSlider != null)
        {
            soundsSlider.value = soundsVolume;
        }
    }

    //Use this method to play sounds
    public void PlaySound(AudioSource source, AudioClip clip, bool isMusic = false)
    {
        source.clip = clip;
        if (isMusic)
        {
            source.volume = musicVolume;
        }
        else
        {
            source.volume = soundsVolume;
        }
        source.Play();
    }

    // Use this method to play OneShot sounds
    public void PlayOneShotSound(AudioSource source, AudioClip clip)
    {
        source.PlayOneShot(clip, soundsVolume);
    }

    public void SetMusicVolume()
    {
        if (musicSlider != null)
        {
            musicVolume = musicSlider.value;

            //Update volume of all audiosources
            AudioSource[] audios = FindObjectsOfType<AudioSource>();
            foreach (AudioSource audio in audios)
            {
                if (audio.CompareTag("Music"))
                {
                    audio.volume = musicVolume;
                }
            }
            // Save volume
            PlayerPrefs.SetFloat(MusicVolumeKey, musicVolume);
        }
        else
        {
            Debug.LogError("Music Slider не назначен!");
        }
    }

    public void SetSoundsVolume()
    {
        if (soundsSlider != null)
        {
            soundsVolume = soundsSlider.value;
            AudioSource[] audios = FindObjectsOfType<AudioSource>();
            foreach (AudioSource audio in audios)
            {
                if (audio.CompareTag("Sound"))
                {
                    audio.volume = soundsVolume;
                }
            }
            // Save volume
            PlayerPrefs.SetFloat(SoundsVolumeKey, soundsVolume);
        }
        else
        {
            Debug.LogError("Sounds Slider не назначен!");
        }
    }
}