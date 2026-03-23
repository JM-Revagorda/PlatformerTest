using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class MenuButtonScript : MonoBehaviour
{
    [Header("UI References")]
    public TMP_InputField usernameInput;
    public Slider volumeSlider;
    public TextMeshProUGUI musicButtonText; // Drag the "ON" text here

    [Header("Scene Settings")]
    [SerializeField] string startGameScene;
    [SerializeField] string optionScene = "Options";
    [SerializeField] GameObject levelLoader;

    private bool isMusicOn = true;

    void Start()
    {
        // Load settings when the scene starts
        if (usernameInput != null)
            usernameInput.text = PlayerPrefs.GetString("Username", "Player");

        if (volumeSlider != null)
            volumeSlider.value = PlayerPrefs.GetFloat("Volume", 1.0f);

        // Load Music state (1 = ON, 0 = OFF)
        isMusicOn = PlayerPrefs.GetInt("MusicEnabled", 1) == 1;
        UpdateMusicUI();
    }

    // This is the function for your Music Button
    public void ToggleMusic()
    {
        isMusicOn = !isMusicOn;
        UpdateMusicUI();

        // LIVE PREVIEW: Talk directly to the persistent MusicManager
        if (MusicManager.instance != null)
        {
            AudioSource musicSource = MusicManager.instance.GetComponent<AudioSource>();
            if (musicSource != null)
            {
                // Force the music source to match our current button click
                musicSource.mute = !isMusicOn;
            }
        }

        // Also update the global listener just in case
        AudioListener.pause = !isMusicOn;
    }

    private void UpdateMusicUI()
    {
        if (musicButtonText != null)
        {
            musicButtonText.text = isMusicOn ? "ON" : "OFF";
        }

        if (volumeSlider != null)
        {
            volumeSlider.interactable = isMusicOn;

            // This is the "Live Preview" for the Volume
            AudioListener.volume = isMusicOn ? volumeSlider.value : 0;
        }

        // Ensure the pause state matches the button state immediately
        AudioListener.pause = !isMusicOn;
    }

    public void SaveSettings()
    {
        // Now we commit the changes to disk
        PlayerPrefs.SetString("Username", usernameInput.text);
        PlayerPrefs.SetFloat("Volume", volumeSlider.value);
        PlayerPrefs.SetInt("MusicEnabled", isMusicOn ? 1 : 0);
        PlayerPrefs.Save();

        // Tell the MusicManager that the new data is the "Official" data
        if (MusicManager.instance != null)
        {
            MusicManager.instance.RefreshSettings();
        }
        SceneManager.LoadScene("Menu");
        Debug.Log("Settings Saved!");
    }

    public void StartGame()
    {
        if (levelLoader != null)
            levelLoader.GetComponent<LevelLoader>().LoadNextScene(startGameScene);
        else
            SceneManager.LoadScene(startGameScene);
    }

    public void goToOptionScene()
    {
        SceneManager.LoadScene(optionScene);
    }

    public void OnVolumeSliderChanged()
    {
        if (volumeSlider != null)
        {
            // This is ONLY a preview. It does NOT save to PlayerPrefs.
            AudioListener.volume = isMusicOn ? volumeSlider.value : 0;
        }
    }

    public void BackToMenu()
    {
        // 1. Reset the MusicManager to the last SAVED data
        if (MusicManager.instance != null)
        {
            MusicManager.instance.RefreshSettings();
        }

        // 2. RESET THE GLOBAL AUDIO: Look at the saved data to decide if we should unpause
        bool savedMusicEnabled = PlayerPrefs.GetInt("MusicEnabled", 1) == 1;
        float savedVolume = PlayerPrefs.GetFloat("Volume", 1.0f);

        AudioListener.pause = !savedMusicEnabled;
        AudioListener.volume = savedMusicEnabled ? savedVolume : 0;

        // 3. Now go back
        SceneManager.LoadScene("Menu");
    }

    public void quitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}