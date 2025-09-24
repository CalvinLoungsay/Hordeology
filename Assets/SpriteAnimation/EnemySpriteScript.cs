using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpriteScript : MonoBehaviour
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
        
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, cam.transform.eulerAngles.y, cam.transform.eulerAngles.z);
    }
}
