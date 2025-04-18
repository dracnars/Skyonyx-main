using UnityEngine;

public class ButtonTrigger : MonoBehaviour
{
    public GameObject door; // La porte à désactiver
    private bool isPressed = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!isPressed && other.CompareTag("Player"))
        {
            isPressed = true;
            OpenDoor();
        }
    }

    void OpenDoor()
    {
        if (door != null)
        {
            door.GetComponent<Door>().Open();
        }
    }
}