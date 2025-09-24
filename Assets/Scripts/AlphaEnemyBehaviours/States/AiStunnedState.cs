using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AiStunnedState : State
{
    // reset attack
    private float TimeInterval;
    

    public void Enter(AIAgent agent)
    {
        //AudioController.aCtrl.GetAudioClip("enemyStun");
        AudioController.aCtrl.GetSound("enemyStun").Play();
        Debug.Log("Stun start");
        Vector3 aboveEnemy = new Vector3(agent.transform.position.x, agent.transform.position.y + 2, agent.transform.position.z);
        agent.stunMarker = Object.Instantiate(agent.stunMarkerObj, aboveEnemy, Quaternion.Euler(0,0,0), null);
        agent.stunMarker.GetComponent<TextMeshPro>().text = "Stunned";
        agent.stunMarker.GetComponent<DecayScript>().decayTime = agent.stunDuration;
        agent.enemySprite.GetComponent<Animator>().SetBool("Walking", false);
        
    }

    public void Exit(AIAgent agent)
    {
        Debug.Log("Stun done");
        Object.Destroy(agent.stunMarker);
    }

    public StateId GetId()
    {
        return StateId.AiStunned;
    }

    public void Update(AIAgent agent)
    {
        agent.navMeshAgent.destination = agent.transform.position;
        agent.stunMarker.transform.position = new Vector3(agent.transform.position.x, agent.transform.position.y + 2, agent.transform.position.z);
        TimeInterval += Time.deltaTime;
        if (TimeInterval >= agent.stunDuration)
        {
            TimeInterval = 0; // reset time
            agent.stateMachine.ChangeState(StateId.FindPlayer);
        }
    }
}
