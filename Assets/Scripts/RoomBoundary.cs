using UnityEngine;
using Unity.Cinemachine;

public class RoomBoundary : MonoBehaviour
{
    public GameObject respawnManager;
    public GameObject spawnPoint;
    public ICinemachineCamera cam;
    public CinemachineCamera roomCamera;
    public CinemachineBrain brain;
    GameObject player = null;

    private void Awake()
    {
        respawnManager = GameObject.FindGameObjectWithTag("RespawnManager");
        brain = Camera.main.GetComponent<CinemachineBrain>(); //Gets the Main Camera Object in the Scene
    }

    private void OnTriggerEnter2D(UnityEngine.Collider2D collision)
    {
        if (player == null && collision.gameObject.CompareTag("Player")) { 
            player = collision.gameObject;
            respawnManager.GetComponent<RespawnManager>().setNewSpawn(spawnPoint);

            //Gets the Room's Camera and makes it a higher priority (trust in the process!)
            roomCamera.Priority = 10;
            cam = brain.ActiveVirtualCamera;
            if (cam is Component component)
            {
                if (component.TryGetComponent<CinemachineCamera>(out CinemachineCamera cmCamera))
                {
                    cmCamera.Follow = player.transform;
                }
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            bool hasDied = respawnManager.GetComponent<RespawnManager>().PlayerDied;
            if (!hasDied)
            {
                player = null;
                roomCamera.Priority = 0; //Sets the Room's Camera to least favorite once Player leaves room
            }
        }
    }
}
