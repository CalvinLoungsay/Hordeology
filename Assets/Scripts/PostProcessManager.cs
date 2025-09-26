using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessManager : MonoBehaviour
{
    public PlayerStats playerStats;
    public Target target;
    public int oldHealth = 200;
    public PostProcessVolume volume;
    private Vignette vignette;

    // Start is called before the first frame update
    void Start()
    {
        playerStats = GameObject.FindWithTag("Player").GetComponent<PlayerStats>();
        target = GameObject.FindWithTag("Player").GetComponent<Target>();
        volume.profile.TryGetSettings(out vignette);
        vignette.intensity.value = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        // Check if user was hurt.
        if (playerStats.getHealth() < oldHealth)
        {
            if(!AudioController.aCtrl.GetSound("playerSounds").isPlaying)
            {
                AudioController.aCtrl.GetSound("playerHurt").Play();
            }
            
            vignette.intensity.value = 0.65f;
            oldHealth = playerStats.getHealth();
        }
        else
        {
            vignette.intensity.value -= 0.004f;
        }
        
    }
}
