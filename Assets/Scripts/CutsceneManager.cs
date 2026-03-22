using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using Unity.Cinemachine;
using UnityEngine.SceneManagement;

public class CutsceneManager : MonoBehaviour
{
    [SerializeField] TimelineAsset startScene;
    [SerializeField] TimelineAsset endScene;
    [SerializeField] TimelineAsset finalScene;
    [SerializeField] GameObject LevelLoader;
    public GameObject player;

    string sceneName;
    PlayableDirector director;
    [SerializeField] bool isPaused = false;
    GameObject mainCamera;
    CinemachineBrain camBrain;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        director = GetComponent<PlayableDirector>();
        mainCamera = Camera.main.gameObject;
        camBrain = mainCamera.GetComponent<CinemachineBrain>();
        if (startScene != null)
        {
            director.playableAsset = startScene;
            director.RebuildGraph();
            director.time = 0;
            director.Play();
        }
    }
    public void SetSceneName(string scene)
    {
        sceneName = scene;
    }
    public void EndLevelScene() {
        //director = GetComponent<PlayableDirector>();
        director.playableAsset = endScene;
        foreach (var track in endScene.GetOutputTracks())
        {
            if (track.name == "PlayerPosition" || track.name == "PlayerSprites")
            {
                director.SetGenericBinding(track, player);
            }
        }
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
            }
        }
        if (finalScene != null && !isPaused)
        {
            if (director.playableAsset == finalScene && director.state != PlayState.Playing)
            {
                LevelLoader.GetComponent<LevelLoader>().LoadNextScene("Menu");
            }
        }
    }
    void FixedUpdate()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
}
