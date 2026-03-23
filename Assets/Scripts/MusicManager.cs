using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;
    [SerializeField] AudioClip menuSong;
    [SerializeField] AudioClip tutorialSong;
    [SerializeField] AudioClip level1Song;
    [SerializeField] AudioClip level2Song;
    [SerializeField] AudioClip finaleSong;
    [Header("Fade Out Settings")]
    [SerializeField] float duration;

    private AudioSource audioSource;
    Scene activeScene;

    void Awake()
    {
        //Singleton Pattern
        //  Basically makes this object persist in all scenes.
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        audioSource = GetComponent<AudioSource>();
        activeScene = SceneManager.GetActiveScene();

        //Runs a custom function everytime SceneManager loads a scene
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Start()
    {
        RefreshSettings();
    }
    // Call this to make the music grab the ACTUAL saved data from disk
    public void RefreshSettings()
    {
        if (audioSource == null) audioSource = GetComponent<AudioSource>();

        float savedVolume = PlayerPrefs.GetFloat("Volume", 1.0f);
        bool musicEnabled = PlayerPrefs.GetInt("MusicEnabled", 1) == 1;

        audioSource.volume = savedVolume;
        audioSource.mute = !musicEnabled;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        activeScene = SceneManager.GetActiveScene();
        //Unmutes audio and Puts volume back on if both permanent variables volume and music enabled is greater than 0
        if (audioSource.mute) audioSource.mute = false;
        if (audioSource.volume == 0 && PlayerPrefs.GetFloat("Volume") > 0 && PlayerPrefs.GetInt("MusicEnabled") != 0) audioSource.volume = PlayerPrefs.GetFloat("Volume");
        
        //Switches Songs for Different Scenes
        if (scene.name == "Menu" || scene.name == "Options")
        {
            audioSource.clip = nextClip;
            audioSource.Play();
        }
        else if (scene.name == "level 1")
        {
            audioSource.clip = level1Song;
        }
        else if (scene.name == "level2(final)")
        {
            audioSource.clip = level2Song;
        
        }else if (scene.name == "Finale")
        {
            audioSource.clip = finaleSong;
        }
        //Plays it
        audioSource.Play();
    }

    public void RunFadeOut() {
        StartCoroutine(AudioFadeOut());
    }

    IEnumerator AudioFadeOut()
    {
        //Allows for Fading Out by simply reducing volume for every frame until volume hits 0 (idk how the while condition is related tho)
        float volume = audioSource.volume;
        float startTime = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup < startTime + duration)
        {
            audioSource.volume = Mathf.Lerp(volume, 0f, (Time.realtimeSinceStartup - startTime) / duration);
            yield return null; // Wait for the next frame
        }

        audioSource.volume = 0f; // Ensure volume is exactly zero at the end
        audioSource.mute = true; // Simply mutes it
        StopCoroutine(AudioFadeOut());
    }
    
    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        //SceneManager.activeSceneChanged -= OnSceneChange;
    }
}