using UnityEngine;

public class GameStateMachine : MonoBehaviour
{
    private IGameState currentState;

    public void ChangeState(IGameState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter(this);
    }
    public void Update()
    {
        currentState?.Update();
    }
}
