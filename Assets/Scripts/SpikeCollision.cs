using UnityEngine;

public class SpikeCollision : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) {
            collision.gameObject.GetComponent<PlayerMovement>().OnDeath();
        }
    }
}
