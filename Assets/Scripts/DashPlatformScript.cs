using System.Collections;
using UnityEngine;

public class DashPlatformScript : MonoBehaviour
{
    [SerializeField] float dashSpeed = 10f;
    [SerializeField] float slowSpeed = 3f;
    [SerializeField] GameObject targetPoint;
    [SerializeField] GameObject player;
    PlayerMovement playerMove;
    Vector2 startPos;
    bool canMove;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPos = transform.position;
        canMove = true;
        playerMove = player.GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove && playerMove.isDashing) {
            StartCoroutine(Move());
        }
    }

    IEnumerator Move() {
        Vector2 targetPos = targetPoint.transform.position;
        canMove = false;
        while (Vector3.Distance(transform.position, targetPos) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, dashSpeed * Time.deltaTime);
            yield return null; // Wait until the next frame
        }

        transform.position = targetPos;

        yield return new WaitForSeconds(0.5f);

        while (Vector3.Distance(transform.position, startPos) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, startPos, slowSpeed * Time.deltaTime);
            yield return null; // Wait until the next frame
        }

        transform.position = startPos;
        canMove = true;
        StopCoroutine(Move());
    }
}
