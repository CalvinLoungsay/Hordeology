using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectsPlayer : MonoBehaviour
{
    public AudioSource src;
    public AudioClip walking, jumping, heartbeat;

    public PlayerStats playerStats;
    public int oldHealth = 200;

    public void Jump()
    {
        src.clip = jumping;
        src.Play();
    }

    public void Walk()
    {
        src.clip = walking;
        src.Play();
    }

    public void HeartBeat()
    {
        src.clip = heartbeat;
        src.Play();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
        {
            Walk();
        }
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
        if (playerStats.getHealth() < oldHealth)
        {
            HeartBeat();
            oldHealth = playerStats.getHealth();
        }  
    }
}
