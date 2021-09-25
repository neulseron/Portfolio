using System;
using System.Collections.Generic;

public abstract class State<T>
{
#region Variables
    protected StateMachine<T> stateMachine;
    protected T context;
#endregion Variables

    public State() { }

#region Methods
    internal void SetStateMachineAndContext(StateMachine<T> _stateMachine, T _context)
    {
        this.stateMachine = _stateMachine;
        this.context = _context;

        OnInitialized();
    }

    public virtual void OnInitialized() { }

    public virtual void OnEnter() { }

    public abstract void Update(float deltaTime);

    public virtual void OnExit() { }
#endregion Methods
}

public sealed class StateMachine<T>
{
#region Variables
    T context;

    State<T> currState;
    public State<T> CurrState => currState;

    State<T> prevState;
    public State<T> PrevState => prevState;

    float elapsedTimeInState = 0f;
    public float ElapsedTimeInState => elapsedTimeInState;

    Dictionary<Type, State<T>> states = new Dictionary<Type, State<T>>();
#endregion Variables

    public StateMachine(T _context, State<T> _initialState)
    {
        this.context = _context;
        
        // ** set initial state **
        AddState(_initialState);
        currState = _initialState;
        currState.OnEnter();
    }

#region Methods
    public void AddState(State<T> state)
    {
        state.SetStateMachineAndContext(this, context);
        states[state.GetType()] = state;
    }

    public void Update(float deltaTime)
    {
        elapsedTimeInState += deltaTime;

        currState.Update(deltaTime);
    }

    public R ChangeState<R>() where R : State<T>
    {
        var newType = typeof(R);

        if (currState.GetType() == newType)     return currState as R;

        if (currState != null)  currState.OnExit();

        prevState = currState;
        currState = states[newType];
        currState.OnEnter();
        elapsedTimeInState = 0f;

        return currState as R;
    }
#endregion Methods
}
