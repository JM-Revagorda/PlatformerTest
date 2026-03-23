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
        //Runs if Player Dashes
        if (canMove && playerMove.isDashing) {
            StartCoroutine(Move());
        }
    }
    private void FixedUpdate()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            playerMove = player.GetComponent<PlayerMovement>();
        }
    }

    IEnumerator Move() {
        //Move to Target Point
        Vector2 targetPos = targetPoint.transform.position;
        canMove = false;
        while (Vector3.Distance(transform.position, targetPos) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, dashSpeed * Time.deltaTime);
            yield return null; // Wait until the next frame
        }

        transform.position = targetPos;

        yield return new WaitForSeconds(1f);

        //Go back to original position
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
