using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Grants an ID to the listed state
public enum StateId
{
    FindPlayer,
    AiAttack,
    AiResetAttack,
    AiStunned
}

// Functions for entering and exiting states
public interface State
{
    StateId GetId();
    void Enter(AIAgent agent);
    void Update(AIAgent agent);
    void Exit(AIAgent agent); 
}

