using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenRaycast : MonoBehaviour
{
    public static ScreenRaycast Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public Vector3 ScreenRaycastEquation(RectTransform crosshair, Transform shootingPoint)
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(crosshair.transform.position);

        Vector3 targetPoint = ray.GetPoint(1000); // Default target point if ray hits nothing
        Debug.DrawRay(shootingPoint.position, (targetPoint - shootingPoint.position).normalized * 1000, Color.blue, 2f);

        if (Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point; // Update target point if ray hits something
        }

        return targetPoint;
    }
}
