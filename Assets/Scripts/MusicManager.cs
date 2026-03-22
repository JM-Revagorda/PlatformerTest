using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;
    private AudioSource audioSource;

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
}