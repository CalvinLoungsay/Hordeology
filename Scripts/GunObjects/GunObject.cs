using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Gun", menuName ="Weapon/Gun")]
public class GunObject : ScriptableObject
{
    // Weapon Info
    public new string name;
    // Shooting Info
    public int damage;
    public float fireRate, spread, range, shotDelay;
    public int magSize, bulletsPerShot;
    public bool isAutomatic;
    public float kickback;
    public string kickbackDirection;
    // Reloading Info
    public int ammoLeft;
    public float reloadTime;
    //[HideInInspector]
    public bool isReloading;

    public int getAmmo()
    {
        return ammoLeft;
    }
}
