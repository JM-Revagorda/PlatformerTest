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
            csManager.FinalScene(); //Calls csManager's Final Scene Func
        }
    }
}
