using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour, IDamageable
{
    public int health;
    public GameObject roundManager;
    public int scoreValue = 0;
    bool dead = false;
    GameObject player;

    public void Start()
    {
        player = GameObject.FindWithTag("Player");
        //find and assign round manager for tracking player score
        if (GameObject.FindWithTag("Round Manager") != null) //round manager should always exist
        {
            roundManager = GameObject.FindWithTag("Round Manager");
        }
    }

    //handles object damage
    public void Damage(int damage) 
    {
        health -= damage;
        if (health <= 0 && !dead) {
            dead = true;
            Debug.Log(health);
            roundManager.GetComponent<roundManager>().IncrementScore(scoreValue); //temp implementation
            Debug.Log("ded");
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (this.gameObject.layer == 10)
        {
            // enemy died
            AudioController.aCtrl.GetSound("enemyDie").Play();
        }

        if (this.gameObject.tag == "canBeGrabbed")
        {
            // hit box
            /*
             for the future work , it should change the large box prefab layer/tag to different to small box prefab layer/tag;
            thus, it is easier to check large box destroyed or small box destroyed
             */


            if (this.gameObject.GetComponent<ObjectDamageScript>() == null)
            {
                // hit large box -- because largebox does not have ObjectDamageScript attached
                
                AudioController.aCtrl.GetSound("largeBoxDestroy").Play();
            }
            else
            {
                // hit small box

                AudioController.aCtrl.GetSound("smallBoxDestroy").Play();
            }
        }

    }
    public void Heal(int hpGain) 
    {
        if (health + hpGain <= player.GetComponent<PlayerStats>().getMaxHealth()) {
            health += hpGain;
        } else {
            health = player.GetComponent<PlayerStats>().getMaxHealth();
        }
    }
}
