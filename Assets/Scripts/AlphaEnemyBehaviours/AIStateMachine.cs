using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStateMachine
{
    public State[] states;
    public AIAgent agent;
    public StateId currState;

    // Builds our state controller
    public AIStateMachine(AIAgent agent)
    {
        this.agent = agent;
        int numStates = System.Enum.GetNames(typeof(StateId)).Length;
        states = new State[numStates];
    }

    // Builds our states before startup
    public void RegisterState(State state)
    {
        int index = (int)state.GetId();
        states[index] = state;
    }

    // Returns the current state
    public State GetState(StateId stateId)
    {
        int index = (int)stateId;
        return states[index];
    }

    public void Update()
    {
        GetState(currState)?.Update(agent);
    }

    // Changes our state and activates the state functions in AIState.cs
    public void ChangeState(StateId newState)
    {
        GetState(currState)?.Exit(agent);
        currState = newState;
        GetState(currState)?.Enter(agent);
    }
}
