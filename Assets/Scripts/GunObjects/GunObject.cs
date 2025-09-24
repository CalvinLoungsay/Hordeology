using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Gun", menuName ="Weapon/Gun")]
public class GunObject : ScriptableObject
{
    public int damage, ammoUsed;
    public float fireRate, spread, range, reloadTime, knockback;
    //public float shotDelay;
    public int magSize, bulletsPerShot;
    public int bulletPenetration;
    public bool isAutomatic;
    public bool canReload;
    public bool ammoVisible;
    [HideInInspector]
    public int ammoLeft, bulletsShot;
    public GameObject shotMarker;
    public int sprite;
    public GameObject hitParticle;
    public GameObject muzzleFlash;
}
