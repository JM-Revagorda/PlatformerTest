using UnityEngine;

[System.Serializable]
public class WindForceClass
{
    public enum State
    {
        None, Up, Down, Left, Right
    }
    public State windState = State.None;
    public float windSpeed = 0f;
    public float timeUntilNextState = 1f;
}
