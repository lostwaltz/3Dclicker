using System;
using System.Collections.Generic;
using System.Linq;

#region INTERRFACE
public interface IState
{
    void OnEnter();
    void Update();
    void FixedUpdate();
    void OnExit();
}

public interface IPredicate
{
    bool Evaluate();
}

public interface ITransition
{
    IState To { get; }
    IPredicate Condition { get; }
}
#endregion

public class StateMachine
{
    private StateNode _current;
    private readonly Dictionary<Type, StateNode> _nodes = new();
    private readonly HashSet<ITransition> _anyTransitions = new();

    public void Update()
    {
        var transition = GetTransition();
        if (transition != null)
            ChangeState(transition.To);
        
        _current.State?.Update();
    }

    public void FixedUpdate()
    {
        _current.State?.FixedUpdate();
    }

    public void SetState(IState state)
    {
        _current = _nodes[state.GetType()];
        _current.State?.OnEnter();
    }

    private  void ChangeState(IState state)
    {
        if(state == _current.State) return;

        var previous = _current.State;
        var nextState = _nodes[state.GetType()].State;
        
        previous.OnExit();
        nextState.OnEnter();
        
        _current = _nodes[state.GetType()];
    }

    private ITransition GetTransition()
    {
        return _anyTransitions.FirstOrDefault(t => t.Condition.Evaluate()) ??
               _current.Transitions.FirstOrDefault(t => t.Condition.Evaluate());
    }

    public void AddTransition(IState from, IState to, IPredicate condition)
    {
        GetOrAddNode(from).AddTransition(GetOrAddNode(to).State, condition);
    }

    public void AddAnyTransition(IState to, IPredicate condition)
    {
        _anyTransitions.Add(new Transition(GetOrAddNode(to).State, condition));
    }
    
    private StateNode AddNewNode(IState state)
    {
        var node = new StateNode(state);
        _nodes.Add(state.GetType(), node);

        return node;
    }
    
    private StateNode GetOrAddNode(IState state)
    {
        var node = _nodes.GetValueOrDefault(state.GetType());

        return node ?? AddNewNode(state);
    }
    
    private class StateNode
    {
        public IState State { get; }
        public HashSet<ITransition> Transitions { get; }

        public StateNode(IState state)
        {
            State = state;
            Transitions = new HashSet<ITransition>();
        }

        public void AddTransition(IState to, IPredicate condition)
        {
            Transitions.Add(new Transition(to, condition));
        }
    }
}