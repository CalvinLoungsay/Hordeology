using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public GunObject gunObject;
    [SerializeField] protected string weapon;
    [SerializeField] protected int health;
    [SerializeField] protected int maxHealth;
    [SerializeField] protected int ammoCount;
    [SerializeField] protected int maxAmmo;
    [SerializeField] protected int waveCount;
    [SerializeField] protected int score;

    private Target targetScript;


    private void Awake() {
        targetScript = GetComponent<Target>();
    }

    void Start()
    {
        // set variables on start.
        setAmmoCount(gunObject.ammoLeft);
        setMaxAmmo(gunObject.magSize);
        setWeapon(gunObject.name);
        setMaxHealth(targetScript.health);
        setHealth(targetScript.health);
    }


    void Update()
    {
        // Keep updating
        setAmmoCount(gunObject.ammoLeft);
        setHealth(targetScript.health);
    }

    public void setWeapon(string weaponInput)
    {
        weapon = weaponInput;
    }

    public void setWave(int waveInput)
    {
        waveCount = waveInput;
    }

    public void addWaves() // addWaves is a placeholder function for the Alpha test
    {
        waveCount++;
    }
   
    public void setHealth(int healthInput)
    {
        health = healthInput;
    }

    public void setMaxHealth(int maxHealthInput)
    {
        maxHealth = maxHealthInput;
    }

    public void setAmmoCount(int ammoCountInput)
    {
        ammoCount = ammoCountInput;
    }

    public void setMaxAmmo(int maxAmmoInput)
    {
        maxAmmo = maxAmmoInput;
    }
    public void addScore(int scoreAdd) {
        score += scoreAdd;
    }

    public string getWeapon()
    {
        return weapon;
    }

    public int getWave()
    {
        return waveCount;
    }

    public int getHealth()
    {
        return health;
    }

    public int getMaxHealth()
    {
        return maxHealth;
    }

    public int getAmmo()
    {
        return ammoCount;
    }

    public int getMaxAmmo()
    {
        return maxAmmo;
    }

    public int getScore(){
        return score;
    }
}
