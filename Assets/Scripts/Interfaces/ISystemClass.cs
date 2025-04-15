public interface IGameState
{
    void Enter(GameStateMachine stateMachine);
    void Update();
    void Exit();
}
