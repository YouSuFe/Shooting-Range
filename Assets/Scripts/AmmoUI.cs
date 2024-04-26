using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AmmoUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI ammunitionText;

    // Method to be called from the Gun script to update ammo display
    public void UpdateAmmoDisplay(int currentAmmo)
    {
        if (ammunitionText != null)
        {
            ammunitionText.text = currentAmmo.ToString();
        }
    }
}
