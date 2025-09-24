using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class AiAttackState : State
{

    public GameObject player;

    public bool playerInSightRange, playerInAttackRange;
    
    public StateId GetId()
    {
        return StateId.AiAttack;
    }
    public void Enter(AIAgent agent)
    {
        player = GameObject.FindWithTag("Player");
        agent.enemySprite.GetComponent<Animator>().SetBool("Walking", false);
    }


    public void Update(AIAgent agent)
    {
        
        Shoot(agent); // attack
        agent.stateMachine.ChangeState(StateId.AiResetAttack);
    }
    public void Exit(AIAgent agent)
    {
        //throw new System.NotImplementedException();
    }



    private void Shoot(AIAgent agent)
    {

        agent.cam.LookAt(player.transform);
        Debug.Log("Attacking");
        Vector3 forceDirection = agent.cam.transform.forward;

        float x = Random.Range(-agent.attackSpread, agent.attackSpread);
        float y = Random.Range(-agent.attackSpread, agent.attackSpread);
        Vector3 direction = agent.cam.forward + agent.cam.up * y + agent.cam.right * x;
       
        if (Physics.Raycast(agent.cam.position, direction, out RaycastHit rayHit))
        {
            if (rayHit.collider.gameObject.GetComponent<Target>() != null)
            {
                if (LayerMask.LayerToName(rayHit.collider.gameObject.layer) == "Player")
                {   
                    bool playerNearby = Physics.CheckSphere(agent.cam.transform.position, agent.attackRange, agent.whatIsPlayer);
                    if (playerNearby) {
                        Debug.Log(rayHit.transform);
                        player.GetComponent<Target>().Damage(agent.aiDamage);
                        Debug.Log("hit player");
                        LineRenderer enemyAttack = (LineRenderer)Object.Instantiate(agent.line);

                        //LineRenderer enemyAttack = agent.line;
                        enemyAttack.SetPosition(0, agent.transform.position);
                        enemyAttack.SetPosition(1, rayHit.point);
                    } else {
                        agent.stateMachine.ChangeState(StateId.AiResetAttack);
                    }
                    
                }
                else if (LayerMask.LayerToName(rayHit.collider.gameObject.layer) == "Object")
                {
                    if (Physics.CheckSphere(rayHit.collider.transform.position, agent.objAggroRange, agent.whatIsPlayer))
                    {
                        rayHit.collider.gameObject.GetComponent<Target>().Damage(agent.aiDamage);
                        Debug.Log("hit object");
                        if (rayHit.collider.gameObject.GetComponent<Rigidbody>() != null)
                        {
                            rayHit.collider.gameObject.GetComponent<Rigidbody>().AddForce(direction * agent.attackKnockback, ForceMode.Impulse);
                        }
                        LineRenderer enemyAttack = (LineRenderer)Object.Instantiate(agent.line);

                        //LineRenderer enemyAttack = agent.line;
                        enemyAttack.SetPosition(0, agent.transform.position);
                        enemyAttack.SetPosition(1, rayHit.point);
                    } else
                    {
                        agent.stateMachine.ChangeState(StateId.AiResetAttack);
                    }
                }
            }

        }
    }


}
