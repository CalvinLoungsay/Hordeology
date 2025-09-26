using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitch : MonoBehaviour
{
    [SerializeField] private Transform[] weapons;

    [SerializeField] private KeyCode[] keys;

    [SerializeField] private float switchTime;

    private int selectedWeapon;
    private float timeSinceLastSwitch;
    
    public PlayerStats stat;

    private void Awake(){
        stat = GetComponent<PlayerStats>();
    }
    private void start() {
        SetWeapons();
        Select(selectedWeapon);
        timeSinceLastSwitch = 0f;
    }

    private void Update() {
        int previousSelectedWeapon = selectedWeapon;
        for(int i = 0; i < keys.Length; i++) {
            if(Input.GetKeyDown(keys[i]) && timeSinceLastSwitch >= switchTime) {
                selectedWeapon = i;
            }
            if (previousSelectedWeapon != selectedWeapon) {
                Select(selectedWeapon);
            }
            timeSinceLastSwitch += Time.deltaTime;
        }
    }

    private void SetWeapons() {
        weapons = new Transform[transform.childCount];
        for (int i = 0; i <transform.childCount; i++) {
            weapons[i] = transform.GetChild(i);
        }
        if (keys == null) keys = new KeyCode[weapons.Length];
    }

    private void Select(int selectedWeapon){
        for (int i = 0; i < weapons.Length; i++) {
            weapons[i].gameObject.SetActive(i == selectedWeapon);
            if( i == selectedWeapon) {
                stat.gunObject = weapons[i].gameObject.GetComponent<GunScript>().gunObject;
                stat.setAmmoCount(stat.gunObject.ammoLeft);
                stat.setMaxAmmo(stat.gunObject.magSize);
            }
        }
        timeSinceLastSwitch = 0f;

    }

}
