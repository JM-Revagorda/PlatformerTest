using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    public GameObject player;
    public GameObject playerInScene;
    public GameObject spawnPoint;
    public CinemachineBrain brain;

    private void Start()
    {
        if (Camera.main != null)
        {
            brain = Camera.main.GetComponent<CinemachineBrain>();
        }
    }
    public void setNewSpawn(GameObject spawnPoint)
    {
        this.spawnPoint = spawnPoint;
    }

    public void runRespawnFunc(GameObject objectInScene) { playerInScene = objectInScene; StartCoroutine(respawnCharacter());}
    public IEnumerator respawnCharacter() {
        Destroy(playerInScene);
        yield return new WaitForSeconds(1f);
        playerInScene = Instantiate(player, spawnPoint.transform.position, Quaternion.identity);
        playerInScene.transform.position = spawnPoint.transform.position;

        ICinemachineCamera roomCamera = brain.ActiveVirtualCamera;
        if (roomCamera is Component component) { 
            if(component.TryGetComponent<CinemachineCamera>(out CinemachineCamera cmCamera))
            {
                cmCamera.Follow = playerInScene.transform;
            }
        }

        StopCoroutine(respawnCharacter());
    }
}
