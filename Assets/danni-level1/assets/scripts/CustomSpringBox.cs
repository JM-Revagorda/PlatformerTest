using UnityEngine;

public class CustomSpringBox : MonoBehaviour
{
    [Header("Physics Settings")]
    [SerializeField] float springStrength = 12f;
    [SerializeField] float collRadius = 0.5f;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] Transform collPoint;

    [Header("Visuals")]
    [SerializeField] Animator animator;
    [SerializeField] string bounceTriggerName = "Bounce";

    private void FixedUpdate()
    {
        // Check if the player is touching the spring area
        Collider2D player = Physics2D.OverlapCircle(collPoint.position, collRadius, playerLayer);

        if (player != null)
        {
            // 1. Launch the Player
            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocityY = springStrength;
            }

            // 2. Refresh the Dash (optional, like your groupmate's script)
            PlayerMovement pm = player.GetComponent<PlayerMovement>();
            if (pm != null)
            {
                pm.canDash = true;
            }

            // 3. Play the Animation
            if (animator != null)
            {
                animator.SetTrigger(bounceTriggerName);
            }
        }
    }

    // This lets you see the detection circle in the Scene view
    private void OnDrawGizmosSelected()
    {
        if (collPoint != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(collPoint.position, collRadius);
        }
    }
}