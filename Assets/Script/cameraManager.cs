using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    public float slideDuration = 1f; // temps pour glisser
    private bool sliding = false;
    public float PixelsPerUnit = 100f; // ou la valeur que tu utilises dans ton projet

    public void SlideRight(float distance)
    {
        if (!sliding) StartCoroutine(SlideCamera(new Vector3(distance, 0, 0)));
    }

    IEnumerator SlideCamera(Vector3 offset)
    {
        sliding = true;
        Vector3 startPos = transform.position;
        Vector3 targetPos = startPos + offset;

        float elapsed = 0f;
        while (elapsed < slideDuration)
        {
            transform.position = Vector3.Lerp(startPos, targetPos, elapsed / slideDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos;
        sliding = false;
    }
}