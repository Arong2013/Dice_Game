using System;

public interface IGameState
{
    void Enter(GameStateMachine stateMachine);
    void Update();
    void Exit();
}
public interface ISceneLoader
{
    void Load(SceneName scene);
    void LoadAsync(SceneName scene, Action onComplete = null);
}
