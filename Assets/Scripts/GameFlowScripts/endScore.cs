using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class endScore : MonoBehaviour
{
    public int finalScore;
    public TextMeshProUGUI _scoreDisplay;
    // Start is called before the first frame update
    void Start()
    {
        finalScore = gameObject.GetComponent<roundManager>().GetFinalScore();
        _scoreDisplay.text = "Final Score: " + finalScore;
    }
}
