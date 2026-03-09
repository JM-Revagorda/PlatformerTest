using UnityEngine;
using Unity.Cinemachine;

public class RoomBoundary : MonoBehaviour
{
    public CinemachineConfiner2D boundingArea;
    BoxCollider2D roomBorder;

    private void Awake()
    {
        roomBorder = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) {
            boundingArea.BoundingShape2D = roomBorder;
            boundingArea.InvalidateBoundingShapeCache();
        }
    }
}
