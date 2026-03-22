using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class EndLevelScript : MonoBehaviour
{
    [SerializeField] GameObject cutsceneManager;
    [SerializeField] GameObject levelLoader;
    [SerializeField] string SceneName;
    CutsceneManager csManager = null;
    [SerializeField] bool canDoEndScene = false;
    void Awake() {
        cutsceneManager = GameObject.Find("CutsceneManager");
        csManager = cutsceneManager.GetComponent<CutsceneManager>();
    }
    public void enableEndScene()
    {
        canDoEndScene = true;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerMovement>().enabled = false;
            if(canDoEndScene)
                csManager.EndLevelScene();
            else
                levelLoader.GetComponent<LevelLoader>().LoadNextScene();
            
        }
    }

    //IEnumerator StopPlayer(string sceneName) {
    //    yield return new WaitForSeconds(1f);
    //    SceneManager.LoadScene(sceneName);
    //}
}
