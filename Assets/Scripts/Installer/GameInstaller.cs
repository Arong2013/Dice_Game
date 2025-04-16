public class GameInstaller
{
    public MyContainer Container { get; private set; }

    public void Install()
    {
        Container = new MyContainer();
        Container.Bind<MyContainer>().ToInstance(Container);

        new DiceInstaller(Container).Install();
        new StateInstaller(Container).Install();
        new UIInstaller(Container).Install();
    }
}
