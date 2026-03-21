using UnityEngine;
using System.Collections;

public class SpriteFlicker : MonoBehaviour
{
    [Header("Sprites")]
    public Sprite offSprite;
    public Sprite onSprite;
    public SpriteRenderer sRenderer;

    [Header("Settings")]
    public float flickerDelay = 0.07f;
    public int flickerAmount = 4;
    public LayerMask playerLayer;

    private bool hasTriggered = false;

    private void Start()
    {
        // Start with the light off
        if (sRenderer != null) sRenderer.sprite = offSprite;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if player entered the trigger zone
        if (!hasTriggered && ((1 << other.gameObject.layer) & playerLayer) != 0)
        {
            StartCoroutine(FlickerRoutine());
        }
    }

    IEnumerator FlickerRoutine()
    {
        hasTriggered = true;

        for (int i = 0; i < flickerAmount; i++)
        {
            sRenderer.sprite = onSprite;
            yield return new WaitForSeconds(flickerDelay);
            sRenderer.sprite = offSprite;
            yield return new WaitForSeconds(flickerDelay);
        }

        // Stay on at the end
        sRenderer.sprite = onSprite;
    }
}