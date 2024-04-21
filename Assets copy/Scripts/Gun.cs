using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Transform shootingPoint;
    public float speed = 20f;

    public void Fire()
    {
        GameObject bullet = BulletPool.Instance.GetBullet();
        if (bullet != null)
        {
            bullet.transform.position = shootingPoint.position;
            bullet.transform.rotation = shootingPoint.rotation;
            bullet.SetActive(true);

            // Now, directly set the bullet's velocity upon firing
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = shootingPoint.forward * speed; // Set the speed here; adjust as necessary
            }
            else
            {
                Debug.LogError("Bullet prefab is missing a Rigidbody component.");
            }
        }
    }
}
