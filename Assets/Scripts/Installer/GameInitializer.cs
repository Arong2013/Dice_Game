using UnityEngine;
using System.Threading.Tasks;

public static class GameInitializer
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void OnGameStart()
    {
        _ = InitializeGameAsync(); // async 시작 (await 못 쓰므로 fire-and-forget 패턴)
    }

    private static async Task InitializeGameAsync()
    {
        SODataCenter.Init();

        var installer = new GameInstaller();
        installer.Install();

        var stateMachine = installer.Container.Resolve<GameStateMachine>();
        stateMachine.ChangeState<IGameState>();
    }
}
