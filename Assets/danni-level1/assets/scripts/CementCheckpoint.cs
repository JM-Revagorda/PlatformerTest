using UnityEngine;

public class CheckpointLight : MonoBehaviour
{
    [Header("Sprites")]
    [SerializeField] Sprite offSprite;
    [SerializeField] Sprite onSprite;
    [SerializeField] SpriteRenderer spriteRenderer;

    [Header("Settings")]
    [SerializeField] LayerMask playerLayer;

    // Track if the checkpoint is already active
    private bool isLit = false;

    private void Start()
    {
        // Ensure we start in the "Off" state
        if (spriteRenderer != null && offSprite != null)
        {
            spriteRenderer.sprite = offSprite;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the player touched it and if it's already lit
        if (!isLit && ((1 << other.gameObject.layer) & playerLayer) != 0)
        {
            LightUp();
        }
    }

    private void LightUp()
    {
        isLit = true;

        // Visual change to the "On" sprite
        if (spriteRenderer != null && onSprite != null)
        {
            spriteRenderer.sprite = onSprite;
        }

        // Add visual flare: A light source!
        // We will create this as a child object in Unity
        Transform lightSource = transform.Find("Point Light");
        if (lightSource != null)
        {
            lightSource.gameObject.SetActive(true);
        }

        Debug.Log("Checkpoint Activated!");
    }
}