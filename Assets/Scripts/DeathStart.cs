using UnityEngine;

public class DeathStart : MonoBehaviour
{
    void Start()
    {
        // We let the RespawnManager handle Destroy() 
        // We just ensure the Animator is running
        GetComponent<Animator>().Play("DeathAnimation");
    }
}