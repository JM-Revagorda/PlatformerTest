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

        // Try to get the component right now
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // Add a "Null Check" to prevent the MissingComponentException
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            return; // Skip this frame if it's still missing
        }

        float savedVolume = PlayerPrefs.GetFloat("Volume", 1.0f);
        bool musicEnabled = PlayerPrefs.GetInt("MusicEnabled", 1) == 1;

        audioSource.volume = savedVolume;
        audioSource.mute = !musicEnabled;
    }
}