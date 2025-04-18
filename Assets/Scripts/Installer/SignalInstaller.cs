public class SignalInstaller
{
    private readonly MyContainer _container;
    public SignalInstaller(MyContainer container) => _container = container;

    public void Install()
    {
        _container.Bind<ISignalBus>().To<SignalBus>().AsSingleton();
    }
}
