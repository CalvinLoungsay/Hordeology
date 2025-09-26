using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class AIAgent : MonoBehaviour
{
  
    public AIStateMachine stateMachine;
    public StateId initialState;
    public NavMeshAgent navMeshAgent;
    public AiAgentConfig config;
    

    //throw object 
    public Transform cam;
    public Transform attackPoint;
    public LineRenderer line;
    public LineRenderer enemyAttack;

    //Attack
    public float attackRange, objAggroRange;
    public float timeBetweenAttacks, targetUpdateTimer, attackSpread, attackKnockback, stunDuration;
    public LayerMask whatIsGroud, whatIsPlayer, whatIsObject;
    public int aiDamage;
    public GameObject stunMarkerObj;
    public GameObject enemySprite;
    public GameObject bloodParticle;

    //bool
    private bool canMove = true; // default by can Move
    private bool canAttack = true; // default by can attack

    // reset attack
    private float TimeInterval;

    //Stun
    public GameObject stunMarker;




    // Builds the state machine for our agent and sets the initial state
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        stateMachine = new AIStateMachine(this);
        stateMachine.RegisterState(new GoToPlayer());
        stateMachine.RegisterState(new AiAttackState());
        stateMachine.RegisterState(new AiResetState());
        stateMachine.RegisterState(new AiStunnedState());
        stateMachine.ChangeState(initialState);
      
        

        

    }

    // Updates state
    void Update()
    {
        stateMachine.Update();
        //Instantiate line
        //enemyAttack = Instantiate(line);
         
    }


    // setter of canAttack
    public void setAttack(bool canAttack)
    {
        this.canAttack = canAttack;
    }

    // getter of canAttack
    public bool getAttack()
    {
        return this.canAttack;
    }

    // setter of canMove
    public void setMove(bool canMove)
    {
        this.canMove = canMove;
    }

    // getter of canMove
    public bool getMove()
    {
        return this.canMove;
    }

   

    

    // reset attack
    public void resetAttack()
    {
        TimeInterval += Time.deltaTime;

        if (TimeInterval >= timeBetweenAttacks)
        {
            
            setAttack(true); // set back allow attack 
            TimeInterval = 0; // reset time
        }
    }
        

    public void Stun(float objStunDuration) {
        stunDuration = objStunDuration;
        stateMachine.ChangeState(StateId.AiStunned);
    }
    void OnDestroy()
    {
        Destroy(stunMarker);
    }


}
