using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponButtonHandler : MonoBehaviour
{
    public GunSwitcher gunSwitcher; // Reference to the GunSwitcher

    // This function will be called by the UI buttons
    public void OnWeaponSelected(Gun gunToSelect)
    {
        gunSwitcher.SetGun(gunToSelect);
    }
}
