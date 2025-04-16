public class MapEnterState : IGameState
{
    private GameStateMachine stateMachine;

    public void Enter(GameStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
        UnityEngine.Debug.Log("MapEnterState: Enter");
        stateMachine.ChangeState<TurnStartState>();
    }

    public void Update()
    {
        // MapEnterState logic
    }

    public void Exit()
    {
        UnityEngine.Debug.Log("MapEnterState: Exit");
    }
}
