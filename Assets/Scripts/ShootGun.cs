// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class ShootGun : MonoBehaviour
// {
//     [SerializeField] public GunObject gunObject;
//     [SerializeField] private Transform cam;
//     // Start is called before the first frame update
//     float timeSinceLastShot;
//     Vector3 originalRotation;
//     Quaternion originalCameraRotation;
//     private void Start() {
//         PlayerShoot.shootInput += Shoot;
//         PlayerShoot.reloadInput += StartReload;
//         originalRotation = transform.localEulerAngles;
//         originalCameraRotation = transform.localRotation;
//         gunObject.isReloading = false;
//         gunObject.ammoLeft = gunObject.magSize;
//     }

//     private void OnDisable() {
//         gunObject.isReloading = false;
//     }
//     public void StartReload() {
//         if (!gunObject.isReloading && this.gameObject.activeSelf && gunObject.ammoLeft != gunObject.magSize) {
//             StartCoroutine(Reload());
//         }
//     }

//     private IEnumerator Reload() {
//         gunObject.isReloading = true;
//         Debug.Log("Reloading now");
//         transform.localEulerAngles += new Vector3(0,-40,0);
//         yield return new WaitForSeconds(gunObject.reloadTime);
//         Debug.Log("Reloading done");
//         StopRecoil();
//         gunObject.ammoLeft = gunObject.magSize;
//         gunObject.isReloading = false;
//     }
//     private bool CanShoot() {
//         if(!gunObject.isReloading && timeSinceLastShot > gunObject.shotDelay){ 
//             return true;
//         } else {
//             return false;
//         }
//     }
//     public void Shoot() {
//         if (gunObject.ammoLeft > 0) {
//             if (CanShoot()) {
//                 // if (Physics.Raycast(muzzle.position, muzzle.forward, out RaycastHit hitInfo, gunObject.range)) {
//                 //     IDamageable damageable = hitInfo.transform.GetComponent<IDamageable>();
//                 //     damageable?.Damage(gunObject.damage);
//                 // }
//                 if(Physics.Raycast(cam.position, cam.forward, out RaycastHit rayHit, gunObject.range))
//                 {
//                     if (LayerMask.LayerToName(rayHit.collider.gameObject.layer) == "Enemy" || LayerMask.LayerToName(rayHit.collider.gameObject.layer) == "Object") {
//                         Debug.Log("Shot: " + gunObject.damage);
//                         rayHit.collider.gameObject.GetComponent<Target>().Damage(gunObject.damage);
//                     }
//                 }
//                 AddRecoil();
//                 gunObject.ammoLeft--;
//                 timeSinceLastShot=0;
//                 Invoke("StopRecoil", 0.5f);
//                 //OnGunShot();
//             }
//         }
//     }

//     private void AddRecoil() {
//         transform.localEulerAngles += new Vector3(gunObject.kickback,0,0);
//         transform.localRotation = Quaternion.Euler(gunObject.kickback, 0f, 0f);
//     }
//     private void StopRecoil() {
//         transform.localEulerAngles = originalRotation;
//         transform.localRotation = originalCameraRotation;
//     }
//     private  void Update() {
//         timeSinceLastShot += Time.deltaTime;
//         Debug.DrawRay(cam.position, cam.forward);
//     }

//     private void OnGunShot() {
//         StopRecoil();
//     }
// }
