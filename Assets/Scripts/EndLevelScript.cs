using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class EndLevelScript : MonoBehaviour
{
    [SerializeField] string SceneName;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
            StartCoroutine(LoadNextScene(SceneName));
    }

    IEnumerator LoadNextScene(string sceneName) {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(sceneName);
    }
}
