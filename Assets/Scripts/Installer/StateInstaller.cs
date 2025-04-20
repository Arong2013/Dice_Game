public class StateInstaller
{
    private readonly MyContainer _container;

    public StateInstaller(MyContainer container) => _container = container;

    public void Install()
    {
        _container.Bind<GameStateMachine>().To<GameStateMachine>().AsSingleton();
        _container.Bind<IGameState>().To<DiceRollState>().AsTransient();
    }
}
