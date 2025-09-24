using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerScript : MonoBehaviour
{
    Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.FindWithTag("Camera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.rotation = cam.transform.rotation;
    }
}
