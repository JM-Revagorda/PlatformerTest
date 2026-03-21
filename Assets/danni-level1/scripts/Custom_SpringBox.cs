using UnityEngine;

public class CustomSpringBox : MonoBehaviour
{
    [Header("Physics Settings")]
    [SerializeField] float springStrength = 15f;
    [SerializeField] Vector2 boxSize = new Vector2(0.8f, 0.2f);
    [SerializeField] LayerMask playerLayer;
    [SerializeField] Transform collPoint;

    [Header("Visuals")]
    [SerializeField] Animator animator;
    [SerializeField] string bounceTriggerName = "Bounce";

    private void FixedUpdate()
    {
        // Check for player in the detection box
        // We use 'transform.rotation.eulerAngles.z' to make sure the detection box rotates with the sprite
        Collider2D player = Physics2D.OverlapBox(collPoint.position, boxSize, transform.eulerAngles.z, playerLayer);

        if (player != null)
        {
            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
            PlayerMovement pm = player.GetComponent<PlayerMovement>();

            if (rb != null)
            {
                // CHANGE: We now use 'transform.up' (the direction the top of the spring is facing)
                // This converts our 'springStrength' into a diagonal Vector!
                Vector2 launchDirection = transform.up * springStrength;

                // Apply the velocity in the direction the spring is pointed
                rb.linearVelocity = launchDirection;

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
            // This helper makes sure the red box in the Scene view rotates as you rotate the spring
            Matrix4x4 rotationMatrix = Matrix4x4.TRS(collPoint.position, transform.rotation, transform.localScale);
            Gizmos.matrix = rotationMatrix;
            Gizmos.DrawWireCube(Vector3.zero, boxSize);
        }
    }
}