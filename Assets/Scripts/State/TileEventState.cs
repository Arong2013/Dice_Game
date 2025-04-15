public class TileEventState : IGameState
{
    private GameStateMachine stateMachine;

    public void Enter(GameStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
        UnityEngine.Debug.Log("TileEventState: Enter");
        // Automatically transition to next state for demonstration
        stateMachine.ChangeState(new BattleState());
    }
    public void Update()
    {
        // TileEventState logic
    }
    public void Exit()
    {
        UnityEngine.Debug.Log("TileEventState: Exit");
    }
}
