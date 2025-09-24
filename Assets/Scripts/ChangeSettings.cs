using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChangeSettings : MonoBehaviour
{
    public Slider Slider; 
    public TextMeshProUGUI text;

    public GameObject player;

    void Start() {
        
        Slider.onValueChanged.AddListener((v) => {
            text.text = v.ToString("0.00");
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void setSens() {
        //fpCam.GetComponent<FirstPersonCameraScript>().SetSensitivity(2);
    }
}
