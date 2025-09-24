using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetCurrentSensitivity : MonoBehaviour
{
    public Text text;
  
    // Update is called once per frame
    void Update()
    {
        text.text = convertToPercentageString();
    }

    public string convertToPercentageString()
    {
        float currentSen = (2 - GameObject.FindGameObjectWithTag("Camera").GetComponent<FirstPersonCameraScript>().GetSensitivity()) * 100;
        string percent = ((int)currentSen).ToString() + "%";
        return percent;
    }
}
