using System.Collections.Generic;
using UnityEngine;

public class FSM : MonoBehaviour
{
    [SerializeField]
    private List<State> states;

    [SerializeField] 
    private List<Transition> transitions = new();

    public State CurrentState { get; private set; }

    private void Start()
    {
        CurrentState = states[0];

        foreach (var state in states)
        {
            state.enabled = true;
        }

        CurrentState.Enter();
    }


    private void Update()
    {
        CurrentState.Execute();
    }


    public void ChangeState (string transitionName)
    {
        foreach (var transition in transitions)
        {
            if (transition.name.ToLower() == transitionName.ToLower() && (transition.from == null || CurrentState == transition.from))
            {
                CurrentState.Exit();
                CurrentState = transition.to;
                CurrentState.Enter();

                return;
            }
        }

        Debug.LogWarning($"Transition {transitionName} not found");
    }
}


[System.Serializable]
public class Transition
{
    public string name;
    public State from;
    public State to;
}

[System.Serializable]
public abstract class State : MonoBehaviour
{
    protected FSM fSM;

    protected string stateName;

    public virtual void Enter() {
       // Debug.Log($"Entering {stateName} State");
    }

    public virtual void Execute() { 
      // Debug.Log($"Executing {stateName} State");
    }

    public virtual void Exit() { 
       // Debug.Log($"Exiting {stateName} State");
    }
}
