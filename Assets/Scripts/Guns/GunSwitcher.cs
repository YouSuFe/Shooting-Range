using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSwitcher : MonoBehaviour
{
    public Gun currentGun;

    public void SetGun(Gun newGun)
    {
        if(currentGun != null)
        {
            currentGun.StopFiring();
        }

        currentGun = newGun;
    }

    public void ReloadCurrentGun()
    {
        currentGun.StartReload();
    }

    public void OnPointerDown()
    {
        currentGun.StartFiring();
    }

    public void OnPointerUp()
    {
        currentGun.StopFiring();
    }
}
