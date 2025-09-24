using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecayScript : MonoBehaviour
{
    public float decayTime = 1;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (decayTime > 0) {
            decayTime -= Time.deltaTime;
        } else {
            Destroy(this.gameObject);
        }
    }
}
