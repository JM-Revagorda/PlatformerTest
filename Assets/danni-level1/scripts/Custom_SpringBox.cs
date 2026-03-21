using UnityEngine;

public class CustomSpringBox : MonoBehaviour
{
    [Header("Physics Settings")]
    [SerializeField] float springStrength = 12f;
    [SerializeField] Vector2 boxSize = new Vector2(0.8f, 0.1f); // Thin and flat
    [SerializeField] LayerMask playerLayer;
    [SerializeField] Transform collPoint;

    [Header("Visuals")]
    [SerializeField] Animator animator;
    [SerializeField] string bounceTriggerName = "Bounce";

    private void FixedUpdate()
    {
        // Change: Use OverlapBox instead of OverlapCircle for precision
        Collider2D player = Physics2D.OverlapBox(collPoint.position, boxSize, 0, playerLayer);

        if (player != null)
        {
            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
            PlayerMovement pm = player.GetComponent<PlayerMovement>();

            // Only bounce if the player is actually falling or landing on it
            if (rb != null && rb.linearVelocityY <= 0.1f)
            {
                rb.linearVelocityY = springStrength;

                if (pm != null) pm.canDash = true;
                if (animator != null) animator.SetTrigger(bounceTriggerName);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (collPoint != null)
        {
            Gizmos.color = Color.red;
            // Draws the box in the Scene view so you can align it perfectly
            Gizmos.DrawWireCube(collPoint.position, boxSize);
        }
    }
}