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
        if (audioSource.mute) audioSource.mute = false;
        if (audioSource.volume == 0 && PlayerPrefs.GetFloat("Volume") > 0) audioSource.volume = PlayerPrefs.GetFloat("Volume");
        //Debug.Log("Scene " + scene.name + " loaded with mode " + mode);
        // Check if the loaded scene is a specific scene
        if (scene.name == "Menu" || scene.name == "Options")
        {
            audioSource.clip = menuSong;
        } else if (scene.name == "Tutorial")
        {
            audioSource.clip = tutorialSong;
        }else if (scene.name == "level 1")
        {
            audioSource.clip = level1Song;
        }else if (scene.name == "level 2(final)")
        {
            audioSource.clip = level2Song;
        }else if (scene.name == "Finale")
        {
            audioSource.clip = finaleSong;
        }
    }
    /*void OnSceneChange(Scene current, Scene next)
    {
        StartCoroutine(AudioFadeOut());
        //Debug.Log("Previous scene: " + activeScene.name + ", New scene: " + next.name);
        //if ((activeScene.name != "Menu" && next.name != "Options") || (activeScene.name != "Options" && next.name != "Menu"))
        //{
        //    StartCoroutine(AudioFadeOut());
        //}
    }*/

    /*IEnumerator AudioFadeOut()
    {
        float volume = audioSource.volume;
        float startTime = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup < startTime + duration)
        {
            audioSource.volume = Mathf.Lerp(volume, 0f, (Time.realtimeSinceStartup - startTime) / duration);
            yield return null; // Wait for the next frame
        }

        audioSource.volume = 0f; // Ensure volume is exactly zero at the end
        audioSource.mute = true;
        StopCoroutine(AudioFadeOut());
    }*/
    
    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        //SceneManager.activeSceneChanged -= OnSceneChange;
    }
}