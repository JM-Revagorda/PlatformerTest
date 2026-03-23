using UnityEngine;

[System.Serializable]
public class WindForceClass
{
    //Would be used if we had more time
    //Creates a custom "data type" with these fields
    public enum State
    {
        None, Up, Down, Left, Right
    }
    public State windState = State.None;
    public float windSpeed = 0f;
    public float timeUntilNextState = 1f;
}
