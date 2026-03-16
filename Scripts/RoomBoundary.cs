using UnityEngine;
using Unity.Cinemachine;

public class RoomBoundary : MonoBehaviour
{
    public GameObject respawnManager;
    public GameObject spawnPoint;
    public ICinemachineCamera cam;
    public CinemachineCamera roomCamera;
    public CinemachineCamera otherRoomCamera;
    public CinemachineBrain brain;
    GameObject player = null;

    private void Awake()
    {
        respawnManager = GameObject.FindGameObjectWithTag("RespawnManager");
        brain = Camera.main.GetComponent<CinemachineBrain>();
    }

    private void OnTriggerEnter2D(UnityEngine.Collider2D collision)
    {
        if (player == null && collision.gameObject.CompareTag("Player")) { 
            player = collision.gameObject;
            respawnManager.GetComponent<RespawnManager>().setNewSpawn(spawnPoint);
            roomCamera.Priority = 10;
            Debug.Log("Room Camera: " + roomCamera + "   Priority: " + roomCamera.Priority);
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
                //roomCamera.gameObject.SetActive(false);
                roomCamera.Priority = 0;
                otherRoomCamera.Priority = 10;
            }
        }
    }
}
