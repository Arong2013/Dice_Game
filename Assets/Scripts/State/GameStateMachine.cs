using UnityEngine;
using System;
using System.Collections.Generic;

public class GameStateMachine
{
    [Inject] private MyContainer _container;
    private IGameState _currentState;
    public void ChangeState<T>() where T : IGameState
    {
        _currentState?.Exit();
        IGameState newState = _container.Resolve<T>();
        _currentState = newState;
        _currentState.Enter(this);
    }
    private void Update()
    {
        _currentState?.Update();
    }
}
