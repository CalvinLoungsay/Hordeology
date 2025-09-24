using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PickupHoverScript : MonoBehaviour
{
    // Start is called before the first frame update
    float timer = 0;
    float direction = 0.25f;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > 1) {
            timer = 0;
            direction = direction * -1;
        }
        this.gameObject.transform.position = new Vector3(
            this.transform.position.x,
            this.transform.position.y + direction * Time.deltaTime,
            this.transform.position.z);
    }
}
