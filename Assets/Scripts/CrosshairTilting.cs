using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairTilting : MonoBehaviour
{
    public static CrosshairTilting Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public IEnumerator TiltCrossHair(RectTransform crosshair, int shootCount, float maxTilt)
    {
        float tiltX = shootCount > 1 ? Random.Range(-30f, 30f) : 0; // No x-axis tilt on the first shot
        float tiltY = Random.Range(50f, 75f);
        float duration = 0.2f;
        float time = 0;

        Vector3 originalPosition = crosshair.transform.localPosition;
        float newYPosition = Mathf.Min(originalPosition.y + tiltY, maxTilt);
        Vector3 targetPosition = new Vector3(originalPosition.x + tiltX, newYPosition, originalPosition.z);

        while (time < duration)
        {
            time += Time.deltaTime;
            crosshair.transform.localPosition = Vector3.Lerp(originalPosition, targetPosition, time / duration);
            yield return null;
        }
    }

    public IEnumerator ResetTilt(RectTransform crosshair)
    {
        float duration = 0.2f;
        float time = 0;
        Vector3 initialPosition = crosshair.transform.localPosition;
        Vector3 zeroPosition = new Vector3(0, 0, initialPosition.z); // Reset to zero on x and y, keep z unchanged

        while (time < duration)
        {
            time += Time.deltaTime;
            crosshair.transform.localPosition = Vector3.Lerp(initialPosition, zeroPosition, time / duration);
            yield return null;
        }
    }
}
