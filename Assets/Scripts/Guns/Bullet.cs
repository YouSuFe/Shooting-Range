using System.Collections;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class Bullet : MonoBehaviour
{
    public float speed = 330f;
    public float rayStartMargin = 0.2f;
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
        // Update position based on Rigidbody movement
        Vector3 currentPosition = transform.position;
        Vector3 direction = (currentPosition - previousPosition).normalized;
        float distance = Vector3.Distance(currentPosition, previousPosition);

        if (Physics.Raycast(previousPosition, direction, out RaycastHit hit, distance))
        {
            Debug.Log("Bullet hit: " + hit.collider.gameObject.name);
            HandleCollision(hit);
            HandleTarget(hit);
        }

        // Update previousPosition for next frame
        previousPosition = currentPosition;

        // Visual debugging to show the ray in the Scene view
        Debug.DrawRay(previousPosition, direction * distance * 10, Color.red);
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
        StartCoroutine(DestroyAfterDelay());
    }

    private void HandleCollision(RaycastHit hit)
    {
       
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

    IEnumerator DestroyAfterDelay()
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(3f);

        // Destroy the current GameObject
        Destroy(gameObject);
    }

}
