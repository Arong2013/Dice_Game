public class TurnStartState : IGameState
{
    private GameStateMachine stateMachine;

    public void Enter(GameStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
        UnityEngine.Debug.Log("TurnStartState: Enter");
        stateMachine.ChangeState(new DiceRollState());
    }

    public void Update()
    {
        // TurnStartState logic
    }

    public void Exit()
    {
        UnityEngine.Debug.Log("TurnStartState: Exit");
    }
}
