using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiResetState : State
{
    // reset attack
    private float TimeInterval;
    private GameObject player;

    public void Enter(AIAgent agent)
    {
        Debug.Log("Reset start");
        player = GameObject.FindWithTag("Player");
        agent.enemySprite.GetComponent<Animator>().SetBool("Walking", false);
    }

    public void Exit(AIAgent agent)
    {
        Debug.Log("Reset done");
    }

    public StateId GetId()
    {
        return StateId.AiResetAttack;
    }

    public void Update(AIAgent agent)
    {
        agent.cam.LookAt(player.transform);
        bool playerNearbyObj = false;
        bool objNearbyPlayer = false;
        float objDist = Mathf.Infinity;
        float playerDist = Mathf.Infinity;
        TimeInterval += Time.deltaTime;
        RaycastHit[] hits = Physics.RaycastAll(agent.cam.position, agent.cam.forward, agent.objAggroRange, agent.whatIsPlayer | agent.whatIsObject);
        foreach (RaycastHit rayHit in hits) {
            if (LayerMask.LayerToName(rayHit.collider.gameObject.layer) == "Player") {
                playerNearbyObj = true;
                playerDist = rayHit.distance;
                //Debug.Log(playerDist);
            } else if (LayerMask.LayerToName(rayHit.collider.gameObject.layer) == "Object") {
                objNearbyPlayer = true;
                if (objDist > rayHit.distance) {
                    objDist = rayHit.distance;
                    //Debug.Log(objDist);
                }
            }
        }
        bool playerNearby = Physics.CheckSphere(agent.transform.position, agent.attackRange, agent.whatIsPlayer);
        if (playerNearbyObj && objNearbyPlayer && objDist < playerDist && objDist <= agent.attackRange)
        {
            if (TimeInterval >= agent.timeBetweenAttacks)
            {
                TimeInterval = 0; // reset time
                agent.stateMachine.ChangeState(StateId.AiAttack);

            }
        }
        else if (playerNearby) {
            if (TimeInterval >= agent.timeBetweenAttacks)
            {
                TimeInterval = 0; // reset time
                agent.stateMachine.ChangeState(StateId.AiAttack);

            }
        } else
        {
            agent.stateMachine.ChangeState(StateId.FindPlayer);
        }
    }
}
