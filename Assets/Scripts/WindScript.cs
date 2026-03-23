using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class WindScript : MonoBehaviour
{
    public List<WindForceClass> stateList = new List<WindForceClass>();

    int counter = 0;
    bool isPlayerHere = false;
    Rigidbody2D playerRB = null;
    Vector2 windForce;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isPlayerHere)
        {
            StartCoroutine(WindEffector());
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerHere = true;
            playerRB= collision.GetComponent<Rigidbody2D>();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerHere = false;
            playerRB = null;
        }
    }

    IEnumerator WindEffector()
    {
        if (isPlayerHere)
        {
            //Loops Basing on the list of Effectors it has and applying it to the Player as External Force
            float speed = stateList[counter].windSpeed;
            float time = stateList[counter].timeUntilNextState;
            switch (stateList[counter].windState)
            {
                case WindForceClass.State.Up: windForce = new Vector2(0, speed); break;
                case WindForceClass.State.Down: windForce = new Vector2(0, -speed); break;
                case WindForceClass.State.Left: windForce = new Vector2(-speed, 0); break;
                case WindForceClass.State.Right: windForce = new Vector2(speed, 0); break;
                case WindForceClass.State.None: windForce = new Vector2(0, 0); break;
            }
            playerRB.AddForce(Vector2.ClampMagnitude(windForce, speed * 1.5f)); 
            // If Force applied gets too much, clamp it down to a maximum of 1.5 * the speed applied
            counter++;
            if (counter > stateList.Count - 1) counter = 0;
            yield return new WaitForSeconds(time);
        } else StopCoroutine(WindEffector());
    }
}
