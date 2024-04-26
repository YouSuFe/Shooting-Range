using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Transform shootingPoint;
    public GameObject bulletPrefab; // The bullet prefab
    public AmmoUI ammoUI; // Reference to the AmmoUI script to update ammo count
    public RectTransform crosHair; // Reference to the player's camera

    public int maxAmmo = 30; // Maximum ammo
    public float reloadTime = 1.0f; // Time it takes to reload
    private int currentAmmo; // Current ammo count
    private bool isReloading = false; // Is the gun currently reloading


    private bool isShooting = false;
    private float lastShotTime = 0f;
    private float tiltDelay = 1.0f; // Delay before tilting resets
    private float maxTilt = 200f; // Maximum tilt range
    private int shotCount = 0; // Count the number of shots

    void Start()
    {
        currentAmmo = maxAmmo; // Initialize ammo count
        ammoUI.UpdateAmmoDisplay(currentAmmo); // Update UI at the start
    }

    void Update()
    {
        if (isShooting && Time.time - lastShotTime > tiltDelay)
        {
            StartCoroutine(ResetTilt());
            isShooting = false;
        }
    }

    GameObject CreateBulletAndShoot(Vector3 targetPoint)
    {
        if (currentAmmo > 0)
        {
            Vector3 shootingDirection = (targetPoint - shootingPoint.position).normalized;
            GameObject bullet = Instantiate(bulletPrefab, shootingPoint.position, Quaternion.LookRotation(shootingDirection));
            bullet.GetComponent<Bullet>().Initialize(shootingDirection);
            return bullet;
        }
        return null;
    }

    public void Fire()
    {
        if (isReloading) return;

        /* 
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
        Vector3 targetPoint = ray.GetPoint(1000); // Default target point if ray hits nothing
        */

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(crosHair.transform.position);

        Vector3 targetPoint = ray.GetPoint(1000); // Default target point if ray hits nothing
        Debug.DrawRay(shootingPoint.position, (targetPoint - shootingPoint.position).normalized * 1000, Color.blue, 2f);

        if (Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point; // Update target point if ray hits something
        }

        GameObject bullet = CreateBulletAndShoot(targetPoint);
        if (bullet != null && currentAmmo > 0)
        {
            isShooting = true;
            lastShotTime = Time.time;
            shotCount++;
            StartCoroutine(TiltCrossHair());
            currentAmmo--;
            ShowCurrentAmmo(); // Update the ammo display each time a bullet is fired
            if(currentAmmo <= 0)
            {
                StartCoroutine(Reload());
            }
        }
    }

    void ShowCurrentAmmo()
    {
        ammoUI.UpdateAmmoDisplay(currentAmmo);
    }

    IEnumerator Reload()
    {
        isReloading = true;
        yield return new WaitForSeconds(reloadTime); // Wait for reload time
        currentAmmo = maxAmmo; // Refill ammo
        ShowCurrentAmmo(); // Update the UI
        isReloading = false;
    }


    IEnumerator TiltCrossHair()
    {
        float tiltX = shotCount > 1 ? Random.Range(-30f, 30f) : 0; // No x-axis tilt on the first shot
        float tiltY = Random.Range(50f, 75f);
        float duration = 0.2f;
        float time = 0;

        Vector3 originalPosition = crosHair.transform.localPosition;
        float newYPosition = Mathf.Min(originalPosition.y + tiltY, maxTilt);
        Vector3 targetPosition = new Vector3(originalPosition.x + tiltX, newYPosition, originalPosition.z);

        while (time < duration)
        {
            time += Time.deltaTime;
            crosHair.transform.localPosition = Vector3.Lerp(originalPosition, targetPosition, time / duration);
            yield return null;
        }
    }

    IEnumerator ResetTilt()
    {
        float duration = 0.2f;
        float time = 0;
        Vector3 initialPosition = crosHair.transform.localPosition;
        Vector3 zeroPosition = new Vector3(0, 0, initialPosition.z); // Reset to zero on x and y, keep z unchanged

        while (time < duration)
        {
            time += Time.deltaTime;
            crosHair.transform.localPosition = Vector3.Lerp(initialPosition, zeroPosition, time / duration);
            yield return null;
        }

        shotCount = 0; // Reset shot count after tilting resets
    }
}
