using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunScript : MonoBehaviour
{
    public int damage;
    public float fireRate, spread, range, reloadTime, shotDelay;
    public int magSize, bulletsPerShot;
    public bool isAutomatic;
    int ammoLeft, bulletsShot;

    bool shooting, canShoot, reloading;

    public Camera fpsCam;
    public Transform attackPoint;
    public RaycastHit rayHit;
    public LayerMask isEnemy;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isAutomatic) {
            shooting = Input.GetKey(KeyCode.Mouse0);
        } 
        else {
            shooting = Input.GetKeyDown(KeyCode.Mouse0);
        }

        if (Input.GetKeyDown(KeyCode.R) && ammoLeft < magSize && !reloading) {
            reloading = true;
            Invoke("ReloadFinished", reloadTime);
        }

        if (canShoot && shooting && !reloading && ammoLeft > 0) {
            canShoot = false;

            float x = Random.Range(-spread, spread);
            float y = Random.Range(-spread, spread);
            Vector3 direction = fpsCam.transform.forward + new Vector3(x, y, 0);

            // if(Physics.Raycast(fpsCam.transform.position, direction, out rayHit, range, isEnemy))
            // {
            //     if (rayHit.collider.CompareTag("Enemy")) {
            //         rayHit.collider.GetComponent<ShootingAi>().TakeDamage(damage);
            //     }
            // }
            ammoLeft--;
            Invoke("ResetShot", fireRate);
        }
    }
    void ResetShot() {
        canShoot = true;
    }
    void ResetReload() {
        ammoLeft = magSize;
        reloading = false;
    }
}
