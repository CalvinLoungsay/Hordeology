using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuFunc : MonoBehaviour
{
    private GameObject RoundManager; // the gameObject attached with pause resume()..
    private GameObject FirstCam; // for setting the sensitivity
    public float SensitivityChange = 0.1f;

    public Slider sensitivitySlider;
    public Slider volumeSlider;
    private void Start()
    {
        RoundManager = GameObject.FindGameObjectWithTag("Round Manager");
        FirstCam = GameObject.FindGameObjectWithTag("Camera"); // change this if there are more than one camera tag
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void Resume()
    {
        // resume the game
        Debug.Log("resume clicked");
        RoundManager.GetComponent<roundManager>().Resume();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void BackToStart()
    {
        // backToStart clicked in pause
        Debug.Log("Back To Start");
        RoundManager.GetComponent<roundManager>().Resume(); // need to resume first -> cursor state back to locked
        RoundManager.GetComponent<roundManager>().ChangeScene("StartScreen");
    }

    public void Quit()
    {
        Application.Quit();

    }

    public void SetSensitivityLevel()
    {

        float SensitivityChange = sensitivitySlider.value;
        FirstCam.GetComponent<FirstPersonCameraScript>().SetSensitivity(SensitivityChange);
    }

    public void SetVolume()
    {
        float volume = volumeSlider.value;
        AudioController.aCtrl.SetVolume(volume);
    }
}
