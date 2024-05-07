using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Gun
{
    private bool isShooting = false;
    private float lastShotTime = 0f;
    private float tiltDelay = 0.5f; // Delay before tilting resets
    private float maxTilt = 200f; // Maximum tilt range
    private int shootCount = 0; // Count the number of shots

    public override void Fire()
    {
        if (isReloading) return;

        Vector3 targetPoint = ScreenRaycast.Instance.ScreenRaycastEquation(crosshair, shootingPoint);

        GameObject bullet = CreateBulletAndShoot(targetPoint);
        if (bullet != null && currentAmmo > 0 && !isReloading)
        {
            isShooting = true;
            gunAnimator.Play(SHOOTING_ANIMATION, -1, 0f);
            lastShotTime = Time.time;
            shootCount++;
            StartCoroutine(CrosshairTilting.Instance.TiltCrossHair(crosshair, shootCount, maxTilt));
            currentAmmo--;
            ShowCurrentAmmo(); // Update the ammo display each time a bullet is fired
            if (currentAmmo <= 0)
            {
                StartCoroutine(Reload());
            }
        }
    }

    void Update()
    {
        if (isShooting && Time.time - lastShotTime > tiltDelay)
        {
            StartCoroutine(CrosshairTilting.Instance.ResetTilt(crosshair));
            shootCount = 0; // Reset shot count after tilting resets
            isShooting = false;
        }
    }

}
