using UnityEngine;

public class SpringBoxScript : MonoBehaviour
{
    [SerializeField] GameObject other;
    [SerializeField] GameObject collPoint;
    [SerializeField] float collRadius;
    [SerializeField] float springStrength;
    [SerializeField] LayerMask playerLayer;

    bool playerEnters = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    // Update is called once per frame
    private void Update()
    {
        if (playerEnters) { 
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            PlayerMovement pm = other.GetComponent<PlayerMovement>();
            pm.canDash = true;
            rb.linearVelocityY = springStrength;
        }
    }

    void FixedUpdate()
    {
        playerEnters = Physics2D.OverlapCircle(collPoint.transform.position, collRadius, playerLayer);
        other = GameObject.FindWithTag("Player");
    }
}
