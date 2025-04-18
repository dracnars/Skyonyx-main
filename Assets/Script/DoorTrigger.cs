using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public Door parentDoor;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            parentDoor.OnPlayerPassed();
        }
    }
}