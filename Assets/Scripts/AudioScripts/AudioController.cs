using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    /*Notes: 
     * Check loop is on/off, playOnAwake is on/off
     * Use it by :  AudioController.aCtrl.GetSound("bgMusic");
     * Make sure AudioController script run before other scriptss
     */

    // Enemy Sound
    public AudioSource EnemyDamageSound;
    public AudioSource EnemyDamageSound2;
    public AudioSource EnemyDamageSound3;
    public AudioSource EnemyNotifySound;
    public AudioSource EnemyDiedSound;
    public AudioSource EnemyStunSound;
    public AudioSource EnemySpawnSound;


    //Enviroment Sound
    public AudioSource bgMusic;
    public AudioSource SmallBoxSounds;
    public AudioSource LargeBoxSounds;
    public AudioSource LargeBoxDestroyedSound;
    public AudioSource SmallBoxDestroyedSound;
    public AudioSource HitMetalSound;
    public AudioSource hitgroundsound;
    public AudioSource hitwallsound;

    // Player Sound
    public AudioSource PlayerWalkingSound;
    public AudioSource PlayerJumpingSound;
    public AudioSource PlayerHeartBeatSound;


    // AudioController
    public static AudioController aCtrl;

    // volume
    private float volume = 1.0f;
    public void Awake()
    {
        if (aCtrl == null)
        {
            DontDestroyOnLoad(gameObject);
            aCtrl = this;
        }
    }

    public void Update()
    {
        SetSourceVolume();
    }

    public void SetVolume(float volume)
    {
        this.volume = volume;
    }

    public float GetVolume()
    {
        return volume;
    }

    public void PlaySound(AudioSource sound)
    {
        if (sound != null)
        {
            sound.Play();
        }
    }
    public void StopSound(AudioSource sound)
    {
        if (sound != null)
        {
            sound.Stop();
        }
    }

    public AudioSource GetSound(string sound)
    {
        switch (sound)
        {

            //player
            case "playerWalking":
                return PlayerWalkingSound; 
            case "playerJumping":
                return PlayerJumpingSound;
            case "playerHurt":
                return PlayerHeartBeatSound;

            // Enemy
            case "enemyDamage":
                switch (new System.Random().Next(0, 2))
                {
                    case 0:
                        return EnemyDamageSound;
                    case 1:
                        return EnemyDamageSound2;
                    case 2:
                        return EnemyDamageSound3;
                    default:
                        return EnemyDamageSound;
                }
                
            case "enemyNotify":
                return EnemyNotifySound;
            case "enemySpawn":
                return EnemySpawnSound;
            case "enemyStun":
                return EnemyStunSound;
            case "enemyDie":
                return EnemyDiedSound;



            // Enviroment
            case "smallBoxHit":
                return SmallBoxSounds;
            case "largeBoxHit":
                return LargeBoxSounds;
            case "smallBoxDestroy":
                return SmallBoxDestroyedSound;
            case "largeBoxDestroy":
                return LargeBoxDestroyedSound;
            case "hitMetal":
                return HitMetalSound;
            case "hitGround":
                return hitgroundsound;
            case "hitWall":
                return hitwallsound;

            default:
                Debug.Log("Error, check PlaySound(string) in AudioController");
                return null;
        }
    }

    private void SetSourceVolume()
    {
        // Enemy sound Volume
        EnemyDamageSound.volume = AudioController.aCtrl.GetVolume()/5;
        EnemyDamageSound2.volume = AudioController.aCtrl.GetVolume()/5;
        EnemyDamageSound3.volume = AudioController.aCtrl.GetVolume()/5;
        EnemyNotifySound.volume = AudioController.aCtrl.GetVolume()/10;
        EnemyDiedSound.volume = AudioController.aCtrl.GetVolume();
        EnemyStunSound.volume = AudioController.aCtrl.GetVolume();
        EnemySpawnSound.volume = AudioController.aCtrl.GetVolume() / 20;

        // Enviroment sound volume
        
        SmallBoxSounds.volume = AudioController.aCtrl.GetVolume();
        LargeBoxSounds.volume = AudioController.aCtrl.GetVolume();
        LargeBoxDestroyedSound.volume = AudioController.aCtrl.GetVolume()/10;
        SmallBoxDestroyedSound.volume = AudioController.aCtrl.GetVolume()/10;
        HitMetalSound.volume = AudioController.aCtrl.GetVolume()/5;
        hitgroundsound.volume = AudioController.aCtrl.GetVolume()/5;
        hitwallsound.volume = AudioController.aCtrl.GetVolume();

        // Player Sound Volume
        PlayerWalkingSound.volume = AudioController.aCtrl.GetVolume()/10;
        PlayerJumpingSound.volume = AudioController.aCtrl.GetVolume()/10;
        PlayerHeartBeatSound.volume = AudioController.aCtrl.GetVolume()/10;
}

}
