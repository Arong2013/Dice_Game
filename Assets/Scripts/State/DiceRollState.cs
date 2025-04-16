public class DiceRollState : IGameState
{
    private GameStateMachine stateMachine;

    public void Enter(GameStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
        UnityEngine.Debug.Log("DiceRollState: Enter");
        stateMachine.ChangeState<MoveState>();
    }
    public void Update()
    {
        // DiceRollState logic
    }
    public void Exit()
    {
        UnityEngine.Debug.Log("DiceRollState: Exit");
    }
}