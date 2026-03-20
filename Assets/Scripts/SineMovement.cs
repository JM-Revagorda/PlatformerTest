using UnityEngine;

public class SineMovement : MonoBehaviour
{
    public enum SineDirection { Vertical, Horizontal}

    [SerializeField] SineDirection direction = SineDirection.Vertical;
    [SerializeField] float moveSpeed = 2.0f;
    [SerializeField] float frequency = 2.0f;
    [SerializeField] float magnitude = 1.0f;

    //Private Variables
    Vector3 initialPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        initialPosition = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = new Vector3(initialPosition.x, Mathf.Sin(Time.time * frequency) * magnitude + initialPosition.y, initialPosition.z);
        //float x = initialPosition.x + (moveSpeed * Time.deltaTime);
        //float y = initialPosition.y + Mathf.Sin(Time.time * frequency) * magnitude;

        //switch (direction)
        //{
        //    case SineDirection.Vertical: transform.position = new Vector2(transform.position.x, y); break;
        //    case SineDirection.Horizontal: transform.position = new Vector2(x, transform.position.y); break;
        //}
        //initialPosition = transform.position;
    }
}
