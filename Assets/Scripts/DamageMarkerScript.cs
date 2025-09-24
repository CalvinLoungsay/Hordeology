using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class DamageMarkerScript : MonoBehaviour
{
    public float markerDecayTime = 1;
    public CinemachineVirtualCamera cam;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        this.gameObject.transform.position = new Vector3(
            this.transform.position.x,
            this.transform.position.y + 1 * Time.deltaTime,
            this.transform.position.z);
    }
}
