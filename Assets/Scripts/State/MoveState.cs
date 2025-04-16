public class MoveState : IGameState
{
    private GameStateMachine stateMachine;

    public void Enter(GameStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
        UnityEngine.Debug.Log("MoveState: Enter");
        // Automatically transition to next state for demonstration
        stateMachine.ChangeState<TileEventState>();
    }

    public void Update()
    {
        // MoveState logic
    }

    public void Exit()
    {
        UnityEngine.Debug.Log("MoveState: Exit");
    }
}