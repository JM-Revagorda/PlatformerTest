using System.Collections;
using UnityEngine;

public class CustomDashPlatform : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] float dashSpeed = 10f;
    [SerializeField] float returnSpeed = 3f;
    [SerializeField] Transform targetPoint;

    [Header("Player Reference")]
    [SerializeField] LayerMask playerLayer;

    private Vector2 startPos;
    private bool isMoving = false;
    private PlayerMovement playerMove;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        // Detect if player is on top to help with jumping
        Collider2D hit = Physics2D.OverlapBox(transform.position, GetComponent<BoxCollider2D>().size, 0, playerLayer);

        if (hit != null)
        {
            playerMove = hit.GetComponent<PlayerMovement>();

            // If the player is dashing, trigger the platform move
            if (playerMove != null && playerMove.isDashing && !isMoving)
            {
                StartCoroutine(MoveRoutine());
            }
        }
    }

    IEnumerator MoveRoutine()
    {
        isMoving = true;
        Vector2 target = targetPoint.position;

        // Move to target
        while (Vector2.Distance(transform.position, target) > 0.01f)
        {
            transform.position = Vector2.MoveTowards(transform.position, target, dashSpeed * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(1f);

        // Move back to start
        while (Vector2.Distance(transform.position, startPos) > 0.01f)
        {
            transform.position = Vector2.MoveTowards(transform.position, startPos, returnSpeed * Time.deltaTime);
            yield return null;
        }

        isMoving = false;
    }
}