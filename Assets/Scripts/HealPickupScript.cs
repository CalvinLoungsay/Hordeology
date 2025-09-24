using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealPickupScript : MonoBehaviour
{
    public int healingAmount;
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDestroy() {
        if(!this.gameObject.scene.isLoaded) return;
        player.GetComponent<Target>().Heal(healingAmount);
    }
}
