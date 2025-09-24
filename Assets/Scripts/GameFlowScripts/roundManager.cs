using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//placeholder implementation for beta
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class roundManager : MonoBehaviour
{
    //Input Actions
    public static Action pauseInput;
    private InputActions inputActions;
    private InputAction pause;
    private InputAction restart;
    private InputAction quit;

    public PlayerStats stats;
    public GameObject player;
    public float roundCountdown;
    public bool isCounting = true; //placeholder for alpha test
    public bool isPeaceful = false;
    public static int finalScore;

    public int roundLength;
    public int peaceLength;
    public int roundCount = 1;

    // pause menu
    public GameObject pauseMenuUi; // GameController prefab
    private bool _isPaused = false;
    // Start is called before the first frame update
    void Start()
    {
        //placeholder pausing for beta testing
        inputActions = new InputActions();
        pause = inputActions.Testing.Pause;
        restart = inputActions.Testing.Reload;
        inputActions.Enable();

        roundCountdown = roundLength;
        player = GameObject.FindWithTag("Player");
        stats = player.GetComponent<PlayerStats>();
        stats.addWaves();

        
    }

    // Update is called once per frame
    void Update()
    {
        if (pause.triggered) { TogglePause(); }
        if (restart.triggered) { ChangeScene("WareHouseMap"); }
        //if (quit.triggered) {ExitGame();}
        if (isCounting == true) { TimerUpdate(); } //placeholder for alpha test
        if (player == null) { GameEnd(); }
    }

    public void IncrementScore(int score) { stats.addScore(score); }
    public int GetScore() { return stats.getScore(); }
    public int GetFinalScore() { return finalScore; }
    public float GetTime() { return roundCountdown; }

    void TimerUpdate()
    {
        roundCountdown -= Time.deltaTime;
        if(isPeaceful &&  roundCountdown <= 0.0) {
            RoundStart();
        } else if (roundCountdown <= 0.0 && !isPeaceful) {
            RoundEnd();
        }
    }

    void RoundStart(){
        isPeaceful = false;
        GameObject[] allObjects = GameObject.FindGameObjectsWithTag("canBeGrabbed");  //returns GameObject[]
        foreach (GameObject box in allObjects)
        {
            if(box.GetComponent<ObjectDamageScript>() != null)
            {
                box.GetComponent<ObjectDamageScript>().objectDamage = box.GetComponent<ObjectDamageScript>().origDamage;
            }
          
        }
        roundCountdown = roundLength;
        roundCount++;
        stats.addWaves();
        //AudioController.aCtrl.GetAudioClip("enemySpawn");
        AudioController.aCtrl.GetSound("enemySpawn").Play();
    }

    void RoundEnd(){
        roundCountdown = peaceLength;
        isPeaceful = true;
        GameObject[] allObjects = GameObject.FindGameObjectsWithTag("canBeGrabbed");
        foreach (GameObject box in allObjects)
        {
            if (box.GetComponent<ObjectDamageScript>() != null)
            {
                box.GetComponent<ObjectDamageScript>().objectDamage = box.GetComponent<ObjectDamageScript>().objectDamage * 2;
            }
            
        }
    }

    void GameEnd() //placeholder for beta test
    {
        finalScore = GetScore();
        gameObject.GetComponent<sceneChanger>().ChangeScene("EndScreen"); 
    }

    //void TogglePause() //placeholder for beta test
    //{
    //    if (_isPaused == false)
    //    {
    //        Time.timeScale = 1;
    //        _isPaused = true;
    //    } else
    //    {
    //        Time.timeScale = 0;
    //        _isPaused = false;
    //    }
    //}
    void TogglePause() //placeholder for beta test
    {

        // Final
        if (_isPaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    public void Resume()
    {
        // resume game
        pauseMenuUi.SetActive(false);
        Time.timeScale = 1f;
        _isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Pause()
    {
        // pause game
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        Debug.Log(Cursor.lockState);
        pauseMenuUi.SetActive(true);
        Time.timeScale = 0f;
        _isPaused = true;
    }
    public void ExitGame() {
        Application.Quit();
    }

    public void ChangeScene(string sceneName) => SceneManager.LoadScene(sceneName); //placeholder implementation for beta test
}
