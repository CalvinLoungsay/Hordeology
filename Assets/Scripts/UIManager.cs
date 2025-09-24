using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public PlayerStats stats;
    public GameObject roundManager; //temp implementation for alpha test
    [SerializeField]
    public TextMeshProUGUI _ammoText;
    public TextMeshProUGUI _score;
    public TextMeshProUGUI _health;
    public TextMeshProUGUI _wave;
    public TextMeshProUGUI _reloadWarning;
    public TextMeshProUGUI _timer; //temp implementation for alpha test
    // Start is called before the first frame update
    void Start()
    {
        stats = GameObject.FindWithTag("Player").GetComponent<PlayerStats>();
        roundManager = GameObject.FindWithTag("Round Manager");
        if (stats.gunObject.ammoVisible == true) { _ammoText.text = stats.gunObject.name + "  " + stats.gunObject.ammoLeft.ToString() + " / " + stats.gunObject.magSize.ToString(); } 
        else { _ammoText.text = ""; }
        _score.text = "Score: " + stats.getScore().ToString(); 
        _health.text = "Health: " + stats.getHealth().ToString();
        _wave.text = "Wave: " + stats.getWave().ToString(); 
        _timer.text = "Survive: " + roundManager.GetComponent<roundManager>().GetTime().ToString(); //temp implementation for alpha test
    }

    void Update()
    {
        if (stats.gunObject.ammoVisible == true) { _ammoText.text = stats.gunObject.name + "  " + stats.gunObject.ammoLeft.ToString() + " / " + stats.gunObject.magSize.ToString(); }
        else { _ammoText.text = ""; }
        _health.text = "Health: " + stats.getHealth().ToString();
        _score.text = "Score: " + stats.getScore().ToString(); //wave changed to score for alpha test
        _wave.text = "Wave: " + stats.getWave().ToString(); 
        if(roundManager.GetComponent<roundManager>().isPeaceful) {
            _timer.text = "Peace: " + roundManager.GetComponent<roundManager>().GetTime().ToString("000.0"); //temp implementation for alpha test
        } else {
            _timer.text = "Survive: " + roundManager.GetComponent<roundManager>().GetTime().ToString("000.0"); //temp implementation for alpha test
        }
    }
}
