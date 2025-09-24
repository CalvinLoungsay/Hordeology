using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnEnemy : MonoBehaviour
{
    public GameObject enemy;
    public GameObject door;
    public float spawnCooldown;
    public float spawnTimer;

    public roundManager roundManager;

    // Update is called once per frame
    void Update()
    {   
        spawnCooldown -= Time.deltaTime;
        if (spawnCooldown <= 0 && !roundManager.isPeaceful && door == null)
        {
            //Instantiate(enemy, gameObject.transform.position, Quaternion.identity);
            Instantiate(enemy, gameObject.transform);
            //Instantiate(enemy);
            spawnCooldown = spawnTimer / ((float).5 * roundManager.roundCount);
            
        }
    }
}
