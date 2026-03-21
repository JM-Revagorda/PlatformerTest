using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtonScript : MonoBehaviour
{
    [SerializeField] string startGameScene;
    [SerializeField] string optionScene;
    [SerializeField] GameObject levelLoader;
    //[SerializeField] string startGameScene;

    public void StartGame()
    {
        levelLoader.GetComponent<LevelLoader>().LoadNextScene();
        //SceneManager.LoadScene(startGameScene);
    }

    public void goToOptionScene()
    {
        Debug.Log("Not Implemented yet!");
    }

    public void quitGame()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }
}
