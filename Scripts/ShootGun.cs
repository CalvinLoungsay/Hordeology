using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootGun : MonoBehaviour
{
    public Shaker Shaker;
    public float duration = 0.5f;
    
    [SerializeField] private GunObject gunObject;
    [SerializeField] private Transform muzzle;
    // Start is called before the first frame update
    float timeSinceLastShot;
    Vector3 originalRotation;
    Quaternion originalCameraRotation;
    private void Start() {
        PlayerShoot.shootInput += Shoot;
        PlayerShoot.reloadInput += StartReload;
        originalRotation = transform.localEulerAngles;
        originalCameraRotation = transform.localRotation;
    }

    public void StartReload() {
        if (!gunObject.isReloading) {
            StartCoroutine(Reload());
        }
    }

    private IEnumerator Reload() {
        gunObject.isReloading = true;
        Debug.Log("Reloading now");
        transform.localEulerAngles += new Vector3(0,-40,0);
        yield return new WaitForSeconds(gunObject.reloadTime);
        Debug.Log("Reloading done");
        StopRecoil();
        gunObject.ammoLeft = gunObject.magSize;
        gunObject.isReloading = false;
    }
    private bool CanShoot() {
        if(!gunObject.isReloading && timeSinceLastShot > gunObject.shotDelay){ 
            return true;
        } else {
            return false;
        }
    }
    public void Shoot() {
        if (gunObject.ammoLeft > 0) {
            if (CanShoot()) {
                if (Physics.Raycast(muzzle.position, muzzle.forward, out RaycastHit hitInfo, gunObject.range)) {
                    IDamageable damageable = hitInfo.transform.GetComponent<IDamageable>();
                    damageable?.Damage(gunObject.damage);
                }
                // start shake
                Shaker.Shake(duration);
                
                AddRecoil();
                gunObject.ammoLeft--;
                timeSinceLastShot=0;
                Invoke("StopRecoil", 0.5f);
                //OnGunShot();
            }
        }
    }

    private void AddRecoil() {
        transform.localEulerAngles += new Vector3(gunObject.kickback,0,0);
        transform.localRotation = Quaternion.Euler(gunObject.kickback, 0f, 0f);
    }
    private void StopRecoil() {
        transform.localEulerAngles = originalRotation;
        transform.localRotation = originalCameraRotation;
    }
    private  void Update() {
        timeSinceLastShot += Time.deltaTime;
        Debug.DrawRay(muzzle.position, muzzle.forward);
    }

    private void OnGunShot() {
        StopRecoil();
    }
}
