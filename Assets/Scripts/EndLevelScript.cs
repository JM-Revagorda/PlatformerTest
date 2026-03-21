using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class EndLevelScript : MonoBehaviour
{
    [SerializeField] GameObject cutsceneManager;
    [SerializeField] string SceneName;
    CutsceneManager csManager = null;

    void Awake() {
        cutsceneManager = GameObject.Find("CutsceneManager");
        csManager = cutsceneManager.GetComponent<CutsceneManager>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerMovement>().enabled = false;
            csManager.SetSceneName(SceneName);
            csManager.EndLevelScene();
        }
            //StartCoroutine(LoadNextScene(SceneName));   //Starts the Coroutine if Player enters
    }

    //IEnumerator StopPlayer(string sceneName) {
    //    yield return new WaitForSeconds(1f);
    //    SceneManager.LoadScene(sceneName);
    //}
}
