using System.Xml;

public class BattleState : IGameState
{
    private GameStateMachine stateMachine;

    public void Enter(GameStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
        UnityEngine.Debug.Log("BattleState: Enter");
        // Automatically transition to next state for demonstration
        stateMachine.ChangeState<RewardState>();
    }

    public void Update()
    {
        // BattleState logic
    }

    public void Exit()
    {
        UnityEngine.Debug.Log("BattleState: Exit");
    }
}
