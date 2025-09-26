using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

// This script dictates enemy behaviour when the player is far away
public class AIStunned : MonoBehaviour
{ 
    NavMeshAgent agent;
    bool isStunned = false;
    public GameObject stunMarkerObj;
    GameObject stunMarker;

    float timer = 0.0f;
    public float targetUpdateTimer = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0.0f)
        {
            if (isStunned) {
                
                agent.SetDestination(transform.position);
                stunMarker.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 2, this.transform.position.z);
            }
            timer = targetUpdateTimer;
        }
          
    }

    void OnDestroy() {
        Destroy(stunMarker);
    }

    void ResetStun() {
        isStunned = false;
        GetComponent<AIAgent>().setMove(true);
        GetComponent<AIAgent>().setAttack(true);
    }

    public void Stun(float stunDuration) {
       
        if (stunMarker != null) {
            Destroy(stunMarker);
        }
        CancelInvoke("ResetStun");
        Vector3 aboveEnemy = new Vector3(this.transform.position.x, this.transform.position.y + 2, this.transform.position.z);
        stunMarker = Instantiate(stunMarkerObj, aboveEnemy, Quaternion.Euler(0,0,0), null);
        stunMarker.GetComponent<TextMeshPro>().text = "Stunned";
        stunMarker.GetComponent<DecayScript>().decayTime = stunDuration;
        isStunned = true;
        GetComponent<AIAgent>().setMove(false);
        GetComponent<AIAgent>().setAttack(false);
        Invoke("ResetStun", stunDuration);
    }
}

//Special Thanks to TheKiwiCoder on Youtube
//https://youtube.com/playlist?list=PLyBYG1JGBcd009lc1ZfX9ZN5oVUW7AFVy