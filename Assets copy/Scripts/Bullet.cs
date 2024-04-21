using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class Bullet : MonoBehaviour
{
    public float speed = 330f;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;
        
    }

    private void OnEnable()
    {
        StartCoroutine(DestroyAfterDelay());
    }

    void Update()
    {
        RaycastHit hit;
        Vector3 rayStart = transform.position + transform.forward * 0.1f;  // Start the ray a bit in front to avoid self-collision
        Vector3 rayDirection = transform.forward;
        float rayLength = speed * Time.deltaTime + 0.5f; // The ray length, added buffer to cover fast movement

        if (Physics.Raycast(rayStart, rayDirection, out hit, rayLength))
        {
            Debug.Log("Bullet hit: " + hit.collider.gameObject.name);
            HandleCollision(hit);
            HandleTarget(hit);

        }

        // Visual debugging to show the ray in the Scene view
        Debug.DrawRay(rayStart, rayDirection * rayLength, Color.red);
    }

    private void HandleCollision(RaycastHit hit)
    {
        // Here you can check if the hit object is part of the AR environment
        // For example, checking if it's an ARPlane:
        if ((hit.collider.gameObject.GetComponent<ARPlane>() != null) || hit.collider.gameObject.CompareTag("Target"))
        {
            // If the ray hits an AR detected plane, destroy the bullet
            gameObject.SetActive(false);
            
        }
    }

    void HandleTarget(RaycastHit hit)
    {
        Target target = hit.collider.GetComponent<Target>();
        if (target != null)  // Ensures that the target script is actually found
        {
            target.OnHit();
        }
    }

    IEnumerator DestroyAfterDelay()
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(3f);

        // Destroy the current GameObject
        gameObject.SetActive(false);
    }

}
