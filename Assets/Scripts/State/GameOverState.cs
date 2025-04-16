
public class GameOverState : IGameState
{
    private GameStateMachine stateMachine;

    public void Enter(GameStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
        UnityEngine.Debug.Log("GameOverState: Enter");
        // Automatically transition to next state for demonstration
        stateMachine.ChangeState<CharacterSelectState>();
    }

    public void Update()
    {
        // GameOverState logic
    }

    public void Exit()
    {
        UnityEngine.Debug.Log("GameOverState: Exit");
    }
}
