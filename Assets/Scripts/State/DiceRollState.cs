public class DiceRollState : IGameState
{
    private GameStateMachine stateMachine;

    public void Enter(GameStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
        UnityEngine.Debug.Log("DiceRollState: Enter");
        stateMachine.ChangeState(new MoveState());
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