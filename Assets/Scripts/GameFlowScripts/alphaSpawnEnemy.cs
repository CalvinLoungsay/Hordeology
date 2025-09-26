using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class alphaSpawnEnemy : MonoBehaviour
{
    public GameObject enemy;
    public float spawnCooldown;
    public float spawnTimer;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        spawnCooldown -= Time.deltaTime;
        if (spawnCooldown <= 0)
        {
            //Instantiate(enemy, gameObject.transform.position, Quaternion.identity);
            Instantiate(enemy, gameObject.transform);
            //Instantiate(enemy);
            spawnCooldown = spawnTimer;
        }
    }
}
