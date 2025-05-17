using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public float musicVolume = 0.5f;
    public float soundsVolume = 0.75f;

    private const string MusicVolumeKey = "MusicVolume";
    private const string SoundsVolumeKey = "SoundsVolume";

    public static AudioManager Instance { get; private set; } // Singleton instance

    [System.Serializable]
    public struct SceneMusic
    {
        public string sceneName;
        public AudioClip musicClip;
    }

    public List<SceneMusic> sceneMusicList = new List<SceneMusic>();
    private AudioSource musicSource;

    public Slider musicSlider;
    public Slider soundsSlider;

    public delegate void VolumeChanged(float value);
    public static event VolumeChanged OnMusicVolumeChanged;
    public static event VolumeChanged OnSoundsVolumeChanged;

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

        // Create AudioSource for music
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.loop = true;
        musicSource.volume = musicVolume;
        musicSource.tag = "Music";
    }

    void Start()
    {
        // Find Sliders from LevelManager
        FindSliders();

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

        // Play music for current scene
        PlayMusicForCurrentScene();

        // Подписываемся на событие загрузки сцены
        SceneManager.sceneLoaded += OnSceneLoaded;

        if (musicSlider != null)
        {
            musicSlider.onValueChanged.AddListener(delegate { SetMusicVolume(musicSlider.value); });
        }

        if (soundsSlider != null)
        {
            soundsSlider.onValueChanged.AddListener(delegate { SetSoundsVolume(soundsSlider.value); });
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindSliders();
        PlayMusicForCurrentScene();
    }

    void OnDestroy()
    {
        // Отписываемся от события, чтобы избежать утечек памяти
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void PlayMusicForCurrentScene()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        foreach (SceneMusic sceneMusic in sceneMusicList)
        {
            if (sceneMusic.sceneName == sceneName)
            {
                PlayMusic(sceneMusic.musicClip);
                return;
            }
        }
        Debug.LogWarning("No music found for scene: " + sceneName);
        musicSource.Stop(); // Stop music if no clip is found
    }

    void PlayMusic(AudioClip clip)
    {
        if (musicSource.clip != clip)
        {
            musicSource.clip = clip;
            musicSource.Play();
        }
        else if (!musicSource.isPlaying)
        {
            musicSource.Play();
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

    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;

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

    public void SetSoundsVolume(float volume)
    {
        soundsVolume = volume;
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

    private void FindSliders()
    {
        GameObject levelManager = GameObject.Find("LevelManager");
        if (levelManager != null)
        {
            SliderReferences sliderReferences = levelManager.GetComponent<SliderReferences>();
            if (sliderReferences != null)
            {
                musicSlider = sliderReferences.musicSlider;
                soundsSlider = sliderReferences.soundsSlider;
            }
            else
            {
                Debug.LogError("SliderReferences component not found on LevelManager!");
            }
        }
        else
        {
            Debug.LogError("LevelManager object not found in the scene!");
        }
    }
}