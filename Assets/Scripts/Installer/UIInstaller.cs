public class UIInstaller
{
    private readonly MyContainer _container;

    public UIInstaller(MyContainer container) => _container = container;

    public void Install()
    {
        //_container.Bind<UIManager>().To<UIManager>().AsSingleton();
        //_container.Bind<PopupManager>().To<PopupManager>().AsSingleton();
    }
}
