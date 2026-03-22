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
        isMusicOn = !isMusicOn; // Flips between true/false
        UpdateMusicUI();
    }

    private void UpdateMusicUI()
    {
        if (musicButtonText != null)
            musicButtonText.text = isMusicOn ? "ON" : "OFF";

        if (volumeSlider != null)
            volumeSlider.interactable = isMusicOn; // Greys out if OFF

        // Mute/Unmute actual game audio
        AudioListener.pause = !isMusicOn;
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetString("Username", usernameInput.text);
        PlayerPrefs.SetFloat("Volume", volumeSlider.value);
        PlayerPrefs.SetInt("MusicEnabled", isMusicOn ? 1 : 0);
        PlayerPrefs.Save();
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

    public void BackToMenu()
    {
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