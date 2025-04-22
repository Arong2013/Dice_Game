using UnityEngine;

public static class GameInitializer
{

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void OnGameStart()
    {
        SODataCenter.Init();    

        var Installer = new GameInstaller();
        Installer.Install();


        var stateMachine = Installer.Container.Resolve<GameStateMachine>();
        stateMachine.ChangeState<IGameState>();
    }
}
