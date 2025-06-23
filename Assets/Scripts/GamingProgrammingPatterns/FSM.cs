using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The FSM class is responsible for  representing a Finite State Machine.
/// This Finite State Machine is used to manage the states of the NPCS (customers).
/// </summary>
public class FSM : MonoBehaviour
{
    /// <summary>
    /// The states atribute is a list of states of the FSM.
    /// </summary>
    [SerializeField]
    private List<State> states;

    /// <summary>
    /// The transitions attribute is a list of transitions of the FSM.
    /// </summary>
    [SerializeField] 
    private List<Transition> transitions = new();

    /// <summary>
    /// The CurrentState attribute is the current state of the FSM.
    /// </summary>
    /// <value>
    /// The current state of the FSM.
    /// </value>
    public State CurrentState { get; private set; }

    /// <summary>
    /// The Start method is called before the first frame update (Unity Callback).
    /// In this method, the FSM is initialized by setting the current state to the first state in the list of states 
    /// and calling its Enter method.
    /// </summary>
    private void Start()
    {
        CurrentState = states[0];
        CurrentState.Enter();
    }

    /// <summary>
    /// The Update method is called once per frame (Unity Callback).
    /// In this method, the Execute method of the current state is called, to perform the actions of the current state.
    /// </summary>
    private void Update()
    {
        CurrentState.Execute();
    }

    /// <summary>
    /// The ChangeState method is used to change the current state of the FSM.
    /// </summary>
    /// <remarks>
    /// In this method, the list of transitions is iterated to find the transition with the name passed as parameter.
    /// If the transition if found, the Exit method of the current state is called to hanlde its final actions, and the Current State is
    /// updtade to the final state of the transition and is Enter method is called to handle its initial actions.
    /// If the transition is not found, a warning is logged.
    /// A transition is found not only if the its name is the same as the one passed as parameter, 
    /// but also if the from state of the transition is null (meaning multiple states to transition to another)
    /// or if the current state is the same as the from state of the transition.
    /// </remarks>
    /// <param name="transitionName">The name of the transition.</param>
    public void ChangeState (string transitionName)
    {   
        foreach (var transition in transitions)
        {
            // Removes whitespaces and converts to lowercase to avoid case sensitivity and whitespaces issues
            if (transition.name.Replace(" ", "").ToLower() == transitionName.Replace(" ", "").ToLower() && (transition.from == null || CurrentState == transition.from))
            {
                CurrentState.Exit();
                CurrentState = transition.to;
                CurrentState.Enter();

                return;
            }
        }

        Debug.LogWarning($"Transition {transitionName} not found for customer {gameObject.name}, from {CurrentState.StateName}");
    }
}

/// <summary>
/// The Transition class is used to represent a transition between two states.
/// It is System.Serializable to be able to be serialized in the inspector.
/// </summary>
[System.Serializable]
public class Transition
{
    /// <summary>
    /// The name atribute is the name of the transition.
    /// </summary>
    public string name;

    /// <summary>
    /// The from attribute is the state from which the transition is made.
    /// </summary>
    public State from;

    /// <summary>
    /// The to attribute is the state to which the transition is made.
    /// </summary>
    public State to;
}

/// <summary>
/// The State class is used to represent a state of the FSM.
/// It is System.Serializable to be able to be serialized in the inspector.
/// </summary>
[System.Serializable]
public abstract class State : MonoBehaviour
{
    /// <summary>
    /// The fSM attribute is the FSM to which the state belongs.
    /// </summary>
    protected FSM fSM;

    protected Animator animator;

    /// <summary>
    /// The StateName attribute is the name of the state.
    /// </summary>
    /// <value>
    /// The name of the state.
    /// </value>
    public string StateName {get; protected set; }

    /// <summary>
    /// The Enter method is called when the state is entered, to handle its initial actions.
    /// It is overriden by its child classes to handle its specific actions.
    /// </summary>
    public virtual void Enter() {
       // Debug.Log($"Entering {stateName} State");
    }

    /// <summary>
    /// The Execute method is called when the state is executed, to perform the actions of the state.
    /// It is overriden by its child classes to handle its specific actions.
    /// </summary>
    public virtual void Execute() { 
      // Debug.Log($"Executing {stateName} State");
    }

    /// <summary>
    /// The Exit method is called when the state is exited, to handle its final actions.
    /// It is overriden by its child classes to handle its specific actions.
    /// </summary>
    public virtual void Exit() { 
       // Debug.Log($"Exiting {stateName} State");
    }
}
