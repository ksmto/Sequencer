using System;

namespace kSequencer;

/// <summary>
/// Allows the user to check if a "sequence" happened
/// </summary>
public class Sequencer
{
    private Sequencer _sequencer;
    private Func<bool> _condition;
    private Action _action;
    private State _state = State.Start;

    /// <summary>
    /// Creates an instance of the sequencer (use this in a Start method, NEVER in Update)
    /// </summary>
    /// <returns>A instance of the sequencer</returns>
    public static Sequencer Start()
    {
        return new Sequencer();
    }

    /// <summary>
    /// Uses the sequencer to check if a condition was met
    /// </summary>
    /// <param name="condition"></param>
    /// <returns></returns>
    public Sequencer If(Func<bool> condition)
    {
        _sequencer = new Sequencer { _condition = condition, _state = State.If };
        return _sequencer;
    }

    /// <summary>
    /// Uses the sequencer to check if a condition was met but in a while loop
    /// </summary>
    /// <param name="condition"></param>
    /// <returns></returns>
    public Sequencer While(Func<bool> condition)
    {
        _sequencer = new Sequencer { _condition = condition, _state = State.While };
        return _sequencer;
    }

    /// <summary>
    /// Uses the sequencer to do an action
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    public Sequencer Do(Action action)
    {
        _sequencer = new Sequencer { _action = action, _state = State.Do };
        return _sequencer;
    }

    /// <summary>
    /// Updates the sequencer (use in an Update method for the sequencer to work properly)
    /// </summary>
    public void Update()
    {
        switch (_state)
        {
            case State.Start:
                Transition();
                break;

            case State.If:
                if (_condition.Invoke())
                {
                    Transition();
                }

                break;

            case State.While:
                while (_condition.Invoke())
                {
                    _action.Invoke();
                }

                Transition();
                break;

            case State.Do:
                _action.Invoke();
                Transition();
                break;

            case State.End:
                return;
        }
    }

    // Used to transition to the next state
    private void Transition()
    {
        if (_sequencer is null)
        {
            _state = State.End;
            return;
        }

        _state = _sequencer._state;
        _action = _sequencer._action;
        _condition = _sequencer._condition;
        _sequencer = _sequencer._sequencer;
        Update();
    }
    
    private enum State
    {
        Start,
        If,
        Do,
        While,
        End
    }
}