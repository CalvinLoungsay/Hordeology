using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cinemachine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class GunScript : MonoBehaviour
{
    //Input Actions
    private InputActions inputActions;
    private InputAction primaryFire;
    //private InputAction secondaryFire;
    private InputAction reload;
    // public int damage;
    // public float fireRate, spread, range, reloadTime, knockback;
    // //public float shotDelay;
    // public int magSize, bulletsPerShot;
    // //public int bulletPenetration;
    // public bool isAutomatic;
    // int ammoLeft, bulletsShot;
    [SerializeField] public GunObject gunObject;
    [SerializeField] public GunObject meleeObject;

    bool shooting, canShoot, reloading, grabbing;

    public CinemachineVirtualCamera cam;
    //public Transform attackPoint;
    public RaycastHit rayHit;
    public LayerMask isEnemy;
    public GameObject damageMarker;
    public GameObject playerObj;
    public GameObject[] weaponSprites;
    public GameObject weaponSprite;
    bool alternateAnimation = false;
    // Start is called before the first frame update
    void Start()
    {
        weaponSprite = weaponSprites[0];
        //Weapon property initialization
        canShoot = true;
        // reloading = false;
        grabbing = false;
        //Input Action initialization
        inputActions = new InputActions();
        primaryFire = inputActions.Actions.PrimaryFire;
        //secondaryFire = inputActions.Actions.SecondaryFire;
        //reload = inputActions.Actions.Reload;
        inputActions.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerObj.GetComponent<GrabScript>().obj != null)
        {
            grabbing = true;
            CancelInvoke("ResetGrabbing");
            Invoke("ResetGrabbing", 0.2f);
        }
        if (gunObject != null)
        {
            if (gunObject.isAutomatic)
            {
                //shooting = Input.GetKey(KeyCode.Mouse0);
                primaryFire.performed += AutoWeaponShootingHold;
            }
            else
            {
                //shooting = Input.GetKeyDown(KeyCode.Mouse0);
                shooting = primaryFire.triggered;
            }
            if (shooting == true) { Shoot(gunObject); }
        }
        else
        {
            shooting = primaryFire.triggered;
            if (shooting == true) { Shoot(meleeObject); }
        }


        // if the weapon can be reloaded, run the reload function
        // if (gunObject.canReload == true) { Reload(); }
        // if the weapon is being shot, run the shoot function

    }
    void ResetShot()
    {
        canShoot = true;
    }
    // void ResetReload() {
    //     gunObject.ammoLeft = gunObject.magSize;
    //     reloading = false;
    // }

    //Placeholder animations
    void StartGunAnimation(GunObject obj)
    {
        if (alternateAnimation && weaponSprite.GetComponent<SpriteScript>().GetSpriteArrayLength() >= 3)
        {
            weaponSprite.GetComponent<SpriteScript>().ChangeSprite(2);
            alternateAnimation = false;
        }
        else
        {
            weaponSprite.GetComponent<SpriteScript>().ChangeSprite(1);
            alternateAnimation = true;
        }

        // if (animationOrder) {
        //     weaponModel1.transform.localRotation = Quaternion.Euler(-20f, 20f, 0f);
        //     weaponModel1.transform.localPosition = new Vector3(-0.4f,0.1f, 0f);
        // } else {
        //     weaponModel2.transform.localRotation = Quaternion.Euler(-20f, -20f, 0f);
        //     weaponModel2.transform.localPosition = new Vector3(0.4f,0.1f, 0f);
        // }
        Invoke("StopGunAnimation", 30 / obj.fireRate);
    }
    void StopGunAnimation()
    {
        weaponSprite.GetComponent<SpriteScript>().ChangeSprite(0);
        // if (animationOrder) {
        //     weaponModel1.transform.localRotation = Quaternion.Euler(-45f, 20f, 0f);
        //     weaponModel1.transform.localPosition = new Vector3(-0.4f,-0.1f, 0f);
        //     animationOrder = false;
        // } else {
        //     weaponModel2.transform.localRotation = Quaternion.Euler(-45f, -20f, 0f);
        //     weaponModel2.transform.localPosition = new Vector3(0.4f,-0.1f, 0f);
        //     animationOrder = true;
        // }
    }

    // void StartReloadAnimation() {
    //     transform.localEulerAngles = new Vector3(0,-40,0);
    //     CancelInvoke("StopGunAnimation");
    //     Invoke("StopGunAnimation", gunObject.reloadTime);
    // }

    void ResetGrabbing()
    {
        grabbing = false;
    }

    void AutoWeaponShootingHold(InputAction.CallbackContext obj)
    {
        shooting = true;
        primaryFire.canceled += AutoWeaponShootingRelease;
    }

    void AutoWeaponShootingRelease(InputAction.CallbackContext obj)
    {
        shooting = false;
    }

    // This function reloads the weapon if the player is trying to reload (reload.triggered)
    // and the weapon has missing ammo to reload (gunObject.ammoLeft < gunObject.magSize) so long
    // as the player isn't already reloading.
    // void Reload()
    // {
    //     if (reload.triggered && gunObject.ammoLeft < gunObject.magSize && !reloading)
    //     {
    //         reloading = true;
    //         StartReloadAnimation();
    //         Invoke("ResetReload", gunObject.reloadTime);
    //     }
    // }

    void Shoot(GunObject obj)
    {
        if (canShoot && obj.ammoLeft > 0 && !grabbing)
        {
            canShoot = false;
            if (obj.bulletPenetration > 0)
            {
                for (int i = 0; i < obj.bulletsPerShot; i++)
                {
                    float x = Random.Range(-obj.spread, obj.spread);
                    float y = Random.Range(-obj.spread, obj.spread);
                    Vector3 direction = cam.transform.forward + cam.transform.up * y + cam.transform.right * x;
                    RaycastHit[] hits = Physics.RaycastAll(cam.transform.position, direction, obj.range);
                    Array.Sort(hits, delegate (RaycastHit hit1, RaycastHit hit2) {
                        return hit1.distance.CompareTo(hit2.distance);
                    });
                    for (int j = 0; j < hits.Length; j++)
                    {
                        RaycastHit rayHit = hits[j];
                        if (j <= obj.bulletPenetration)
                        {
                            if (LayerMask.LayerToName(rayHit.collider.gameObject.layer) == "Wall")
                            {
                                // hit wall
                                //AudioController.aCtrl.GetAudioClip("hitWall");
                                AudioController.aCtrl.GetSound("hitWall").Play();
                            }
                            else if (LayerMask.LayerToName(rayHit.collider.gameObject.layer) == "Floor")
                            {
                                // hit ground
                               // AudioController.aCtrl.GetAudioClip("hitGround");
                                AudioController.aCtrl.GetSound("hitGround").Play();
                            }
                            else if (LayerMask.LayerToName(rayHit.collider.gameObject.layer) == "Metal")
                            {
                                // hit metal
                               // AudioController.aCtrl.GetAudioClip("hitMetal");
                                AudioController.aCtrl.GetSound("hitMetal").Play();
                            }
                            if (LayerMask.LayerToName(rayHit.collider.gameObject.layer) == "Enemy" || LayerMask.LayerToName(rayHit.collider.gameObject.layer) == "Object")
                            {
                                if (rayHit.collider.gameObject.GetComponent<Rigidbody>() != null)
                                {
                                    rayHit.collider.gameObject.GetComponent<Rigidbody>().AddForce(direction * obj.knockback, ForceMode.Impulse);
                                }
                                if (rayHit.collider.gameObject.GetComponent<Target>() != null)
                                {
                                    if (LayerMask.LayerToName(rayHit.collider.gameObject.layer) == "Enemy")
                                    {
                                        //AudioController.aCtrl.GetAudioClip("enemyDamage");
                                        AudioController.aCtrl.GetSound("enemyDamage").Play();
                                    }
                                    if (rayHit.collider.gameObject.tag == "canBeGrabbed")
                                    {
                                        if (rayHit.collider.gameObject.name.Contains("Big"))
                                        {
                                            // hit large box
                                         
                                            //AudioController.aCtrl.GetAudioClip("largeBoxHit");
                                            AudioController.aCtrl.GetSound("largeBoxHit").Play();
                                        }
                                        else
                                        {
                                           
                                            // hit small box
                                            //AudioController.aCtrl.GetAudioClip("smallBoxHit");
                                            AudioController.aCtrl.GetSound("smallBoxHit").Play();
                                        }
                                    }
                                    GameObject dmgMarker = Instantiate(damageMarker, rayHit.point, Quaternion.Euler(0, 0, 0), null);
                                    dmgMarker.GetComponent<DamageMarkerScript>().cam = cam;
                                    dmgMarker.GetComponent<TextMeshPro>().text = obj.damage.ToString();
                                    Debug.Log("Shot: " + obj.damage);
                                    rayHit.collider.gameObject.GetComponent<Target>().Damage(obj.damage);
                                }
                                else
                                {
                                    GameObject dmgMarker = Instantiate(damageMarker, rayHit.point, Quaternion.Euler(0, 0, 0), null);
                                    dmgMarker.GetComponent<DamageMarkerScript>().cam = cam;
                                    dmgMarker.GetComponent<TextMeshPro>().text = "0";
                                }
                                if (LayerMask.LayerToName(rayHit.collider.gameObject.layer) == "Enemy")
                                {
                                    Instantiate(rayHit.collider.gameObject.GetComponent<AIAgent>().bloodParticle, rayHit.point, Quaternion.LookRotation(rayHit.normal), null);
                                }
                                else
                                {
                                    if (obj.hitParticle != null)
                                    {
                                        Instantiate(obj.hitParticle, rayHit.point, Quaternion.LookRotation(rayHit.normal), null);
                                    }
                                }
                            }
                            else if (LayerMask.LayerToName(rayHit.collider.gameObject.layer) == "WeaponPickup" || LayerMask.LayerToName(rayHit.collider.gameObject.layer) == "HealthPickup")
                            {

                            }
                            else
                            {
                                if (obj.shotMarker != null)
                                {
                                    GameObject shotMarker = Instantiate(obj.shotMarker, rayHit.point, Quaternion.LookRotation(rayHit.normal), null);
                                    shotMarker.transform.position += rayHit.normal / 1000;
                                }
                                if (obj.hitParticle != null)
                                {
                                    Instantiate(obj.hitParticle, rayHit.point, Quaternion.LookRotation(rayHit.normal), null);
                                }
                                break;

                                //shotMarker.transform.SetParent(rayHit.collider.gameObject.transform, true);
                            }
                        }
                        else
                        {
                            break;
                        }
                    }

                }
            }
            else
            {
                for (int i = 0; i < obj.bulletsPerShot; i++)
                {
                    float x = Random.Range(-obj.spread, obj.spread);
                    float y = Random.Range(-obj.spread, obj.spread);
                    Vector3 direction = cam.transform.forward + cam.transform.up * y + cam.transform.right * x;
                    if (Physics.Raycast(cam.transform.position, direction, out RaycastHit rayHit, obj.range))
                    {
                        if (LayerMask.LayerToName(rayHit.collider.gameObject.layer) == "Wall")
                        {
                            // hit wall
                           // AudioController.aCtrl.GetAudioClip("hitWall");
                            AudioController.aCtrl.GetSound("hitWall").Play();
                        }
                        else if (LayerMask.LayerToName(rayHit.collider.gameObject.layer) == "Floor")
                        {
                            // hit ground
                          //  AudioController.aCtrl.GetAudioClip("hitGround");
                            AudioController.aCtrl.GetSound("hitGround").Play();
                        }
                        else if (LayerMask.LayerToName(rayHit.collider.gameObject.layer) == "Metal")
                        {
                            // hit metal
                           // AudioController.aCtrl.GetAudioClip("hitMetal");
                            AudioController.aCtrl.GetSound("hitMetal").Play();
                        }
                        if (LayerMask.LayerToName(rayHit.collider.gameObject.layer) == "Enemy" || LayerMask.LayerToName(rayHit.collider.gameObject.layer) == "Object")
                        {
                            Debug.Log(rayHit.collider.gameObject.tag);
                            if (rayHit.collider.gameObject.GetComponent<Rigidbody>() != null)
                            {
                                rayHit.collider.gameObject.GetComponent<Rigidbody>().AddForce(direction * obj.knockback, ForceMode.Impulse);
                            }
                            if (rayHit.collider.gameObject.GetComponent<Target>() != null)
                            {
                                if (LayerMask.LayerToName(rayHit.collider.gameObject.layer) == "Enemy")
                                {
                                    //AudioController.aCtrl.GetAudioClip("enemyDamage");
                                    AudioController.aCtrl.GetSound("enemyDamage").Play();
                                }
                                if (rayHit.collider.gameObject.tag == "canBeGrabbed")
                                {
                                   
                                    if (rayHit.collider.gameObject.name.Contains("Big"))
                                    {
                                        // hit large box
                                      
                                       // AudioController.aCtrl.GetAudioClip("largeBoxHit"); 
                                        AudioController.aCtrl.GetSound("largeBoxHit").Play();
                                    }
                                    else
                                    {
                                      
                                        // hit small box
                                        //AudioController.aCtrl.GetAudioClip("smallBoxHit");
                                        AudioController.aCtrl.GetSound("smallBoxHit").Play();
                                    }
                                }

                                GameObject dmgMarker = Instantiate(damageMarker, rayHit.point, Quaternion.Euler(0, 0, 0), null);
                                dmgMarker.GetComponent<DamageMarkerScript>().cam = cam;
                                dmgMarker.GetComponent<TextMeshPro>().text = obj.damage.ToString();
                                Debug.Log("Shot: " + obj.damage);
                                rayHit.collider.gameObject.GetComponent<Target>().Damage(obj.damage);
                            }
                            else
                            {
                                GameObject dmgMarker = Instantiate(damageMarker, rayHit.point, Quaternion.Euler(0, 0, 0), null);
                                dmgMarker.GetComponent<DamageMarkerScript>().cam = cam;
                                dmgMarker.GetComponent<TextMeshPro>().text = "0";
                            }
                            if (LayerMask.LayerToName(rayHit.collider.gameObject.layer) == "Enemy")
                            {
                                Instantiate(rayHit.collider.gameObject.GetComponent<AIAgent>().bloodParticle, rayHit.point, Quaternion.LookRotation(rayHit.normal), null);
                            }
                            else
                            {
                                if (obj.hitParticle != null)
                                {
                                    Instantiate(obj.hitParticle, rayHit.point, Quaternion.LookRotation(rayHit.normal), null);
                                }
                            }
                        }
                        else if (LayerMask.LayerToName(rayHit.collider.gameObject.layer) == "WeaponPickup" || LayerMask.LayerToName(rayHit.collider.gameObject.layer) == "HealthPickup")
                        {

                        }
                        else
                        {
                            if (obj.shotMarker != null)
                            {
                                GameObject shotMarker = Instantiate(obj.shotMarker, rayHit.point, Quaternion.LookRotation(rayHit.normal), null);
                                shotMarker.transform.position += rayHit.normal / 1000;
                            }
                            if (obj.hitParticle != null)
                            {
                                Instantiate(obj.hitParticle, rayHit.point, Quaternion.LookRotation(rayHit.normal), null);
                            }
                            //shotMarker.transform.SetParent(rayHit.collider.gameObject.transform, true);
                        }
                    }
                }
            }
            if (obj.muzzleFlash != null)
            {
                Instantiate(obj.muzzleFlash, cam.transform.position + cam.transform.forward, Quaternion.Euler(0, 0, 0), cam.transform);
            }
            obj.ammoLeft = obj.ammoLeft - obj.ammoUsed;
            if (obj.ammoLeft <= 0)
            {
                gunObject = null;
                ResetShot();
                weaponSprite.SetActive(false);
                weaponSprite = weaponSprites[meleeObject.sprite];
                weaponSprite.SetActive(true);
                weaponSprite.GetComponent<SpriteScript>().ChangeSprite(0);
            }
            else
            {
                StartGunAnimation(obj);
                Invoke("ResetShot", 60 / obj.fireRate);
            }
        }
    }
}