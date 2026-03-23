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
        AudioClip nextClip = null;

        if (scene.name == "Menu" || scene.name == "Options") nextClip = menuSong;
        else if (scene.name == "Tutorial") nextClip = tutorialSong;
        else if (scene.name == "level 1") nextClip = level1Song;
        else if (scene.name == "level2(final)") nextClip = level2Song;
        else if (scene.name == "Finale") nextClip = finaleSong;

        // ONLY restart if the song actually needs to change
        if (audioSource.clip != nextClip)
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
        audioSource.Play();
    }

    public void RunFadeOut() {
        StartCoroutine(AudioFadeOut());
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

    IEnumerator AudioFadeOut()
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
    }
    
    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        //SceneManager.activeSceneChanged -= OnSceneChange;
    }
}