using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State<T>
{
    protected StateMachine<T> stateMachine;
    protected T context;

    public State() { }

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
}

public sealed class StateMachine<T>
{
    T context;

    State<T> currState;
    public State<T> CurrState => currState;   // 외부에서 수정하지 못하도록

    State<T> prevState;
    public State<T> PrevState => prevState;

    float elapsedTimeInState = 0f;    // 흐른 시간
    public float ElapsedTimeInState => elapsedTimeInState;

    Dictionary<Type, State<T>> states = new Dictionary<Type, State<T>>();

    public StateMachine(T _context, State<T> _initialState)
    {
        this.context = _context;
        
        // ** set initial state **
        AddState(_initialState);
        currState = _initialState;
        currState.OnEnter();
    }

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
}
