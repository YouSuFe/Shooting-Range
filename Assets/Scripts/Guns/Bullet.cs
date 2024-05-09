using System.Collections;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class Bullet : MonoBehaviour
{
    public float speed = 330f;
    private Rigidbody rb;
    private Vector3 previousPosition;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        if (rb == null)
        {
            Debug.LogError("Rigidbody component is missing on the bullet prefab!");
        }

    }

    private void Start()
    {
        previousPosition = transform.position;
    }

    private void Update()
    {
        RaycastHit hit;
        Vector3 rayStart = previousPosition + transform.forward * 0.1f;  // Start the ray a bit in front to avoid self-collision
        Vector3 rayDirection = transform.position + transform.forward * speed * Time.deltaTime;
        float rayLength = (rayDirection - previousPosition).magnitude; // The ray length, added buffer to cover fast movement

        if (Physics.Raycast(rayStart, rayDirection.normalized, out hit, rayLength))
        {
            Debug.Log("Bullet hit: " + hit.collider.gameObject.name);
            HandleCollision(hit);
            HandleTarget(hit);

        }

        // Visual debugging to show the ray in the Scene view
        Debug.DrawRay(rayStart, rayDirection * 10, Color.red);
    }

    public void Initialize(Vector3 bulletDirection)
    {
        if (rb != null)
        {
            rb.velocity = bulletDirection * speed;
        }
        else
        {
            Debug.LogError("Failed to set bullet velocity because Rigidbody is null.");
        }
    }

    private void OnEnable()
    {
        StartCoroutine(InActiveAfterDelay());
    }

    private void HandleCollision(RaycastHit hit)
    {
        // Here you can check if the hit object is part of the AR environment
        // For example, checking if it's an ARPlane:
        if ((hit.collider.gameObject.GetComponent<ARPlane>() != null) || hit.collider.gameObject.CompareTag("Target"))
        {
            // If the ray hits an AR detected plane, destroy the bullet
            Destroy(gameObject);

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

    IEnumerator InActiveAfterDelay()
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(3f);

        // Destroy the current GameObject
        Destroy(gameObject);
    }

}
