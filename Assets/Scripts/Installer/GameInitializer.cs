using UnityEngine;
using System.Threading.Tasks;

public static class GameInitializer
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void OnGameStart()
    {
        _ = InitializeGameAsync(); // fire-and-forget
    }

    private static async Task InitializeGameAsync()
    {
        Logger.SetLogger(message => Debug.Log(message)); 
        try
        {


            // 1. 게임 내부 초기화
            SODataCenter.Init();

            // 2. Firebase 인증 및 로그인
            await FirebaseAuthService.InitializeAndLoginAsync();
            await FirebaseUserDataIO.PreloadAllAsync(); 

            var installer = new GameInstaller();
            installer.Install();

            // 5. 게임 상태 머신 시작
            var stateMachine = installer.Container.Resolve<GameStateMachine>();
            stateMachine.ChangeState<IGameState>();
        }
        catch (System.Exception ex)
        {
            Logger.LogError($"[GameInitializer] 초기화 실패: {ex}");
            throw;
        }
    }
}
