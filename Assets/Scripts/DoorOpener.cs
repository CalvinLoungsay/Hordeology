using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpener : MonoBehaviour
{
    public roundManager roundManager;

    public GameObject door;

    public int waveOpen;

    // Update is called once per frame
    void Update()
    {
        if(roundManager.roundCount == waveOpen) {
            Destroy(door);
        }
    }
}
