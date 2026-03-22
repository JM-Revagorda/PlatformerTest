using UnityEngine;

public class EndGameCutscene : MonoBehaviour
{
    [SerializeField] GameObject csManagerObj;
    CutsceneManager csManager;
    void Awake()
    {
        csManager = csManagerObj.GetComponent<CutsceneManager>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            csManager.FinalScene();
        }
    }
    //// Start is called once before the first execution of Update after the MonoBehaviour is created
    //void Start()
    //{
        
    //}

    //// Update is called once per frame
    //void Update()
    //{
        
    //}
}
