using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToPlayer : State
{
    public Transform targetTransform;

    float timer = 0.0f;
    bool playerInAttackRange;


    public StateId GetId()
    {
        return StateId.FindPlayer;
    }

    public void Enter(AIAgent agent)
    {
        // in case targetTransform does not exist
        if (targetTransform == null)
        {
            targetTransform = GameObject.FindWithTag("Player").transform;
        }
        agent.enemySprite.GetComponent<Animator>().SetBool("Walking", true);
    }

    public void Exit(AIAgent agent)
    {
        //throw new System.NotImplementedException();
    }



    public void Update(AIAgent agent)
    {
        // in case agent does not exist 
        if (!agent.enabled)
        {
            return;
        }

        timer -= Time.deltaTime;
        if (timer < 0.0f)
        {
            agent.cam.LookAt(targetTransform);

            // if allow to move - not stunned
            if (agent.getMove())
            {
                // find player
                if (Physics.Raycast(agent.cam.position, agent.cam.forward, out RaycastHit rayHit, Mathf.Infinity, agent.whatIsObject | agent.whatIsPlayer))
                {
                    bool playerNearby = Physics.CheckSphere(rayHit.collider.transform.position, agent.objAggroRange, agent.whatIsPlayer);
                    if (playerNearby)
                    {
                        agent.navMeshAgent.destination = rayHit.collider.ClosestPoint(agent.transform.position);
                    }
                    else
                    {
                        agent.navMeshAgent.destination = targetTransform.position;
                        
                    }
                }
            }
           

          
                // check attack within range
                if (Physics.Raycast(agent.cam.position, agent.cam.forward, out RaycastHit rayHit2, agent.attackRange))
                {
                    // if attack not in cooldown 
                    if (agent.getAttack())
                    {
                        bool playerNearby = Physics.CheckSphere(rayHit2.collider.transform.position, agent.objAggroRange, agent.whatIsPlayer);
                        if (playerNearby) {
                            agent.navMeshAgent.destination = agent.transform.position;
                            if (LayerMask.LayerToName(rayHit2.collider.gameObject.layer) == "Player"
                                || LayerMask.LayerToName(rayHit2.collider.gameObject.layer) == "Object")
                            {
                                
                                agent.stateMachine.ChangeState(StateId.AiResetAttack); // change state to attack

                            }
                        }
                    }
                }
            
            //else
            //{
            //    // reset attack
            //    agent.resetAttack();
            //}
           

            timer = agent.targetUpdateTimer;
        }

    }
}
