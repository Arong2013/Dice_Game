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
        // ✅ 데이터 시스템 초기화
        SODataCenter.Init();

        // ✅ Firebase 로그인
        await FirebaseAuthService.InitializeAndLoginAsync();

        // ✅ 유저 데이터 준비 (존재 확인 후 로드 or 생성)
        var handler = new FirebaseUserDataProvider<PlayerProfile>(FirebaseAuthService.UserId);
        await handler.LoadAsync(FirebaseAuthService.UserId); // 👈 아래에 있는 유틸리티 함수

        // ✅ DI 시스템 설치
        var installer = new GameInstaller();
        installer.Install();

        // ✅ 게임 상태 머신 시작
        var stateMachine = installer.Container.Resolve<GameStateMachine>();
        stateMachine.ChangeState<IGameState>();
    }
}
