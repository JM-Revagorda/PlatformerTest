using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    public GameObject player;
    public GameObject playerInScene;
    public GameObject spawnPoint;
    public CinemachineBrain brain;
    public GameObject currentRoom;
    GameObject deathParticles;
    public bool PlayerDied = false;


    //Camera Brain - The Main Controller of all the Cinemachines
    int deathCounter = 0;

    private void Start()
    {
        //Gets the Main Camera object, kek.
        if (Camera.main != null)
        {
            brain = Camera.main.GetComponent<CinemachineBrain>();
        }
    }
    //Encapsulation baby!
    public void setNewSpawn(GameObject spawnPoint)
    {
        this.spawnPoint = spawnPoint;
    }

    public void runRespawnFunc(GameObject objectInScene) { 
        playerInScene = objectInScene;
        objectInScene.GetComponent<PlayerMovement>().animator.SetBool("isDead", true); //Allows this script to set the animation
        objectInScene.GetComponent<PlayerMovement>().enabled = false;   // Disables the Script to probably prevent 'double' deaths
        PlayerDied = true;
        deathParticles = objectInScene.GetComponent<PlayerMovement>().deathParticles;
        StartCoroutine(respawnCharacter());
    }

    public IEnumerator respawnCharacter() {
        if(playerInScene != null) // Just making sure that those double Respawn bugs are gone for good!
            Destroy(playerInScene);

        if (playerInScene == null) { 
            deathCounter++;
            Debug.Log("Deaths: " + deathCounter);
        }
        yield return new WaitForSeconds(1f);
        Destroy(deathParticles);
        playerInScene = Instantiate(player, spawnPoint.transform.position, Quaternion.identity);
        playerInScene.transform.position = spawnPoint.transform.position;

        ICinemachineCamera roomCamera = brain.ActiveVirtualCamera;

        //Got this from Gemini
        //Basically checks if the component is 'real' and seperates it from roomCamera and places it to component, to then cmCamera where the component can be interacted as it is
        if (roomCamera is Component component) { 
            if(component.TryGetComponent<CinemachineCamera>(out CinemachineCamera cmCamera))
            {
                cmCamera.Follow = playerInScene.transform;
            }
        }
        PlayerDied = false;
        StopCoroutine(respawnCharacter());
    }
}
