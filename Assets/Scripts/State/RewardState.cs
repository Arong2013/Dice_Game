
public class RewardState : IGameState
{
    private GameStateMachine stateMachine;

    public void Enter(GameStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
        UnityEngine.Debug.Log("RewardState: Enter");
        // Automatically transition to next state for demonstration
        stateMachine.ChangeState(new TurnEndState());
    }

    public void Update()
    {
        // RewardState logic
    }

    public void Exit()
    {
        UnityEngine.Debug.Log("RewardState: Exit");
    }
}
