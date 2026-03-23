using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class EndLevelScript : MonoBehaviour
{
    [SerializeField] GameObject cutsceneManager;
    [SerializeField] GameObject musicManager;
    [SerializeField] GameObject levelLoader;
    [SerializeField] string SceneName;
    CutsceneManager csManager = null;
    [SerializeField] bool canDoEndScene = false;
    MusicManager mManager;
    void Awake() {
        cutsceneManager = GameObject.Find("CutsceneManager");
        
        csManager = cutsceneManager.GetComponent<CutsceneManager>();
        
    }
    void Start()
    {
        musicManager = GameObject.Find("BG-music");
        mManager = musicManager.GetComponent<MusicManager>();
    }
    void FixedUpdate()
    {
        if(musicManager == null)
        {
            musicManager = GameObject.Find("BG-music");
            mManager = musicManager.GetComponent<MusicManager>();
        }
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
            if (canDoEndScene) {
                csManager.EndLevelScene();
                mManager.RunFadeOut();
            }
            else
                levelLoader.GetComponent<LevelLoader>().LoadNextScene();
            
        }
    }

    //IEnumerator StopPlayer(string sceneName) {
    //    yield return new WaitForSeconds(1f);
    //    SceneManager.LoadScene(sceneName);
    //}
}
