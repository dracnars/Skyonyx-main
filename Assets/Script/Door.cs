using UnityEngine;

public class Door : MonoBehaviour
{
    public GameObject blockingCollider; // à assigner manuellement dans l'inspector
    public float pixelsPerUnit = 100f;

    private bool isOpen = false;
    private bool hasSlidCamera = false;

    public void Open()
    {
        if (blockingCollider != null)
        {
            blockingCollider.SetActive(false);
            isOpen = true;
        }
    }

    public void OnPlayerPassed()
    {
        if (isOpen && !hasSlidCamera)
        {
            hasSlidCamera = true;

            float screenWidthUnits = Screen.width / pixelsPerUnit;
            Camera.main.GetComponent<CameraController>()?.SlideRight(screenWidthUnits);
        }
    }
}