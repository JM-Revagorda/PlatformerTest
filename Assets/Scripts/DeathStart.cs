using UnityEngine;

public class DeathStart : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<ParticleSystem>().Play();
        Destroy(gameObject);
    }
}
