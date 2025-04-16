
public class TurnEndState : IGameState
{
    private GameStateMachine stateMachine;

    public void Enter(GameStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
        UnityEngine.Debug.Log("TurnEndState: Enter");
        // Automatically transition to next state for demonstration
        stateMachine.ChangeState<GameOverState>();
    }

    public void Update()
    {
        // TurnEndState logic
    }

    public void Exit()
    {
        UnityEngine.Debug.Log("TurnEndState: Exit");
    }
}