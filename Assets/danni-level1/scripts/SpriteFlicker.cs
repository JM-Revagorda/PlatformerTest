using UnityEngine;
using System.Collections;

public class SpriteFlicker : MonoBehaviour
{
    [Header("Sprites")]
    public Sprite offSprite;
    public Sprite onSprite;
    public SpriteRenderer sRenderer;

    [Header("Flicker Settings")]
    public float flickerDelay = 0.12f;
    public int flickerAmount = 4;
    public LayerMask playerLayer;

    private bool isFlickering = false;

    private void Start()
    {
        // Default state is ON
        if (sRenderer != null) sRenderer.sprite = onSprite;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        // If it's the player AND we aren't already in the middle of a flicker
        if (((1 << other.gameObject.layer) & playerLayer) != 0 && !isFlickering)
        {
            StartCoroutine(FlickerRoutine());
        }
    }

    IEnumerator FlickerRoutine()
    {
        isFlickering = true;

        for (int i = 0; i < flickerAmount; i++)
        {
            sRenderer.sprite = offSprite;
            // Shorter random bursts make it feel more like a glitchy sensor
            yield return new WaitForSeconds(Random.Range(flickerDelay * 0.5f, flickerDelay));

            sRenderer.sprite = onSprite;
            yield return new WaitForSeconds(Random.Range(flickerDelay * 0.5f, flickerDelay));
        }

        // Always end on ON
        sRenderer.sprite = onSprite;

        // This is the key: we set this back to false so it can trigger again next time!
        isFlickering = false;
    }
}