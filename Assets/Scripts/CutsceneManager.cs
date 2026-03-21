using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.SceneManagement;

public class CutsceneManager : MonoBehaviour
{
    [SerializeField] TimelineAsset startScene;
    [SerializeField] TimelineAsset endScene;
    [SerializeField] TimelineAsset finalScene;
    [SerializeField] GameObject LevelLoader;
    string sceneName;
    PlayableDirector director;

    void Start()
    {
        director = GetComponent<PlayableDirector>();
        director.playableAsset = startScene; 
        director.RebuildGraph();    
        director.time = 0;
        director.Play();
    }
    public void SetSceneName(string scene)
    {
        sceneName = scene;
    }
    public void EndLevelScene() {
        director.playableAsset = endScene;
        director.RebuildGraph();
        director.time = 0;
        director.Play();
    }
    public void FinalScene()
    {
        director.playableAsset = finalScene;
        director.RebuildGraph();
        director.time = 0;
        director.Play();
    }
    void Update()
    {
        if (endScene != null)
        {
            if (director.playableAsset == endScene && director.state != PlayState.Playing)
            {
                LevelLoader.GetComponent<LevelLoader>().LoadNextScene();
                //MoveNextScene(sceneName);
            }
        }
    }

    void MoveNextScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
