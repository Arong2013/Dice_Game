public class CharacterSelectState : IGameState
{
    private GameStateMachine stateMachine;

    public void Enter(GameStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
        UnityEngine.Debug.Log("CharacterSelectState: Enter");
        stateMachine.ChangeState<MapEnterState>();
    }
    public void Update()
    {
        // DiceRollState logic
    }
    public void Exit()
    {
        UnityEngine.Debug.Log("CharacterSelectState: Exit");
    }
}