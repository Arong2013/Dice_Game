using System;
using System.Threading.Tasks;

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
public interface IUserDataProvider<T>
{
    Task<T> LoadAsync(string uid);
    Task SaveAsync(string uid, T data);
}
