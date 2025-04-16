using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    public static GameInstaller Installer { get; private set; }

    private void Awake()
    {
        Installer = new GameInstaller();
        Installer.Install();

        var stateMachine = Installer.Container.Resolve<GameStateMachine>();
        stateMachine.ChangeState<InitState>();
    }
}
