using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// This script dictates enemy behaviour when the player is far away
public class FindPlayer : MonoBehaviour
{
    public Transform targetTransform;
    public LayerMask whatIsPlayer, whatIsObject;
    public Transform enemyCam;
    //Player must be within this distance of an object for an enemy to target it
    public float objectAggroRange;
    public float maxDist = 1.0f;
    
    NavMeshAgent agent;
    public float targetUpdateTimer = 0.1f;
    float timer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
        targetTransform = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0.0f)
        {
            enemyCam.LookAt(targetTransform);
            if (Physics.Raycast(enemyCam.position, enemyCam.forward, out RaycastHit rayHit, Mathf.Infinity, whatIsObject | whatIsPlayer)) {
                bool playerNearby = Physics.CheckSphere(rayHit.collider.transform.position, objectAggroRange, whatIsPlayer);
                if (playerNearby) {
                    agent.SetDestination(rayHit.collider.ClosestPoint(this.transform.position));
                } else {
                    agent.destination = targetTransform.position;
                }
            }
            timer = targetUpdateTimer;
        }   
    }
}

//Special Thanks to TheKiwiCoder on Youtube
//https://youtube.com/playlist?list=PLyBYG1JGBcd009lc1ZfX9ZN5oVUW7AFVy