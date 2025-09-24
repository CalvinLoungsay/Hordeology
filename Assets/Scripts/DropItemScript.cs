using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItemScript : MonoBehaviour
{
    public GameObject[] items;
    public int numberOfItemsToDrop;
    public int percentDropChance;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDestroy() {
        if(!this.gameObject.scene.isLoaded) return;
        for (int i = 0; i < numberOfItemsToDrop; i++) {
        int randDrop = Random.Range(0, 100);
            if (randDrop <= percentDropChance) {
                int randItem = Random.Range(0, items.Length);
                Instantiate(items[randItem], transform.position, Quaternion.Euler(0, 0, 0), null);
                
            }
        }
    }
}
