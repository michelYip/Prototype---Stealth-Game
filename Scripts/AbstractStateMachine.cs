using UnityEngine;

public enum State
{
    LOCOMOTION,
    SPRINT,
    CROUCH,
    COVER,
    SLIDE,
    JUMP,
    WALLRUN,
    HIT,
    DIE,
    TAKEDOWN,
}

public abstract class AbstractStateMachine : MonoBehaviour
{
    #region Exposed
    #endregion

    #region Unity API
    protected virtual void Awake()
    {
        _currentState = State.LOCOMOTION;
    }

	protected void Update()
	{
        OnStateUpdate();
	}

	#endregion

	#region Main Methods

	protected abstract void OnStateEnter();
    protected abstract void OnStateUpdate();
    protected abstract void OnStateExit();
    protected void TransitionToState(State _newState)
	{
        OnStateExit();
        _currentState = _newState;
        OnStateEnter();
	}

    public State CurrentState { get => _currentState; set => _currentState = value; }

    #endregion

    #region Privates

    protected State _currentState;


	#endregion
}
