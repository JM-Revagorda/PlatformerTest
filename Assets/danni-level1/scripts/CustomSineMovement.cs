using UnityEngine;

public class CustomSineMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] float speed = 2.0f;
    [SerializeField] float amount = 1.0f;

    [Header("The Secret Ingredient")]
    [Range(0, 2 * Mathf.PI)]
    [SerializeField] float phaseOffset = 0f; // 0 to 6.28 (a full circle)

    Vector3 startPos;

    void Start()
    {
        // Remembers where you placed it in the level
        startPos = transform.position;
    }

    void Update()
    {
        // We add phaseOffset inside the Sine function to "shift" the starting time
        float movement = Mathf.Sin((Time.time * speed) + phaseOffset) * amount;

        // Apply the movement to the Y axis
        transform.position = new Vector3(startPos.x, startPos.y + movement, startPos.z);
    }
}