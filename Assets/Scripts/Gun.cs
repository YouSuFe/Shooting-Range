using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gun : MonoBehaviour
{
    protected const string RELOAD_ANIMATION = "Reload";
    protected const string SHOOTING_ANIMATION = "Fire_Anim";

    [SerializeField] protected Transform shootingPoint;
    [SerializeField] protected Animator gunAnimator;
    [SerializeField] protected GameObject bulletPrefab; // The bullet prefab
    [SerializeField] protected AmmoUI ammoUI; // Reference to the AmmoUI script to update ammo count
    [SerializeField] protected RectTransform crosshair; // Reference to the player's camera


    [SerializeField] protected int maxAmmo = 30; // Maximum ammo
    [SerializeField] protected float reloadTime = 1.5f; // Time it takes to reload
    protected int currentAmmo; // Current ammo count
    protected bool isReloading = false; // Is the gun currently reloading

    protected virtual void Awake()
    {
        gunAnimator = GetComponent<Animator>();
    }

    protected virtual void Start()
    {
        currentAmmo = maxAmmo;
        ammoUI.UpdateAmmoDisplay(currentAmmo);
    }

    public abstract void Fire();

    protected virtual IEnumerator Reload()
    {
        isReloading = true;
        gunAnimator.SetTrigger(RELOAD_ANIMATION);
        yield return new WaitForSeconds(reloadTime);
        currentAmmo = maxAmmo; // Refill Ammo
        ShowCurrentAmmo(); // Update UI
        isReloading = false;
    }

    protected void ShowCurrentAmmo()
    {
        ammoUI.UpdateAmmoDisplay(currentAmmo);
    }

    protected GameObject CreateBulletAndShoot(Vector3 targetPoint)
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
}
