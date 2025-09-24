using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class alphaRoundManager : MonoBehaviour
{
    public PlayerStats stats;
    public GameObject player;
    public float roundCountdown = 240.0f;
    public bool isCounting = true; //placeholder for alpha test
    public static int finalScore;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        stats = player.GetComponent<PlayerStats>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isCounting == true) { TimerUpdate(); } //placeholder for alpha test
        if (player == null) { RoundEnd(); }
    }

    public void IncrementScore(int score) { stats.addScore(score); }
    public int GetScore() { return stats.getScore(); }
    public int GetFinalScore() { return finalScore; }
    public float GetTime() { return roundCountdown; }

    void TimerUpdate()
    {
        roundCountdown -= Time.deltaTime;
        if (roundCountdown <= 0.0)
        {
            RoundEnd();
        }
    }

    void RoundEnd() //placeholder for alpha test
    {
        finalScore = GetScore();
        gameObject.GetComponent<sceneChanger>().ChangeScene("EndScreen"); 
    }
}
