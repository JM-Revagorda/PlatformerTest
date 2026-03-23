using UnityEngine;

public class VelocityCalculator : MonoBehaviour
{
    private Vector3 _velocity;
    private Vector3 _previousPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _previousPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //Gets velocity of object by doing this equation, followed by replacing previousPosition into the currentPosition this frame
        _velocity = (transform.position - _previousPosition) / Time.deltaTime;
        _previousPosition = transform.position;
    }

    //Getter
    public Vector2 GetVelocity()
    {
        return _velocity;
    }
}
