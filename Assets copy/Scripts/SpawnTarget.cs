using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTarget : MonoBehaviour
{
    public GameObject targetPrefab; // Assign this in the Inspector
    public Transform playerCamera; // Assign the main camera or AR camera here

    // Method to spawn a target in front of the player
    public void SpawnTargetPrefab()
    {
        if (targetPrefab && playerCamera)
        {
            // Determine spawn position in front of the camera
            Vector3 spawnPosition = playerCamera.position + playerCamera.forward * 5f; // Adjust 5 to your preferred distance
            Quaternion spawnRotation = Quaternion.identity; // Default rotation, modify if needed

            // Instantiate the target at the calculated position and rotation
            Instantiate(targetPrefab, spawnPosition, spawnRotation);
        }
    }

}
