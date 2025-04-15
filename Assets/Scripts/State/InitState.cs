public class InitState : IGameState
{
    public void Enter(GameStateMachine stateMachine)
    {
        throw new System.NotImplementedException();
    }

    public void Update()
    {
        // InitState logic here
    }

    public void Exit()
    {
        UnityEngine.Debug.Log("InitState: Exit");
    }

}
