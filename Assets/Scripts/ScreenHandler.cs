using UnityEngine;

public class ScreenHandler : MonoBehaviour
{
    [SerializeField] GameObject mainSettings;
    bool isPaused = false;
    Rigidbody2D[] rigidbodies;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused) 
            {
                isPaused = true;
                EnableMainSettings();
            } else
            {
                isPaused = false;
                DisableMainSettings();
            }
        }
    }

    void PausePhysics()
    {
        rigidbodies = FindObjectsOfType<Rigidbody2D>();
        foreach (Rigidbody2D rb in rigidbodies)
        {
            rb.Sleep(); // Stops physics, keeps current position
        }
    }

    void ResumePhysics()
    {
        foreach (Rigidbody2D rb in rigidbodies)
        {
            rb.WakeUp(); // Resumes physics
        }
    }
    public void EnableMainSettings()
    {
        mainSettings.SetActive(true);
        gameObject.SetActive(false);
        PausePhysics();
    }
    public void DisableMainSettings()
    {
        mainSettings.SetActive(false);
        gameObject.SetActive(true);
        ResumePhysics();
    }
}
