using System.Collections;
using UnityEngine;

public class StartDialogueController : MonoBehaviour
{
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private MonoBehaviour playerMovement; // your movement script

    [SerializeField] private float dialogueDuration = 3f; // seconds

    private void Start()
    {
        StartCoroutine(StartDialogue());
    }

    IEnumerator StartDialogue()
    {
        // Disable player movement
        if (playerMovement != null)
            playerMovement.enabled = false;

        // Show dialogue
        dialogueBox.SetActive(true);

        // Wait (or replace with your typewriter finish later)
        yield return new WaitForSeconds(dialogueDuration);

        // Hide dialogue
        dialogueBox.SetActive(false);

        // Enable player movement
        if (playerMovement != null)
            playerMovement.enabled = true;
    }
}