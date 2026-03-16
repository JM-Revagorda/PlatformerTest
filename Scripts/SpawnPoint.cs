using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    GameObject respawnManager;
    bool wasEntered;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        wasEntered = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player") && !wasEntered)
        {
            wasEntered = true;
            Debug.Log(wasEntered);
            respawnManager.GetComponent<RespawnManager>().setNewSpawn(gameObject);
        }
    }
}
