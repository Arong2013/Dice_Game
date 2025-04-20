using DocumentFormat.OpenXml.Vml.Spreadsheet;

public class GameInstaller
{
    public MyContainer Container { get; private set; }

    public void Install()
    {
        Container = new MyContainer();
        Container.RegisterInstance(typeof(MyContainer),Container);

        new SignalInstaller(Container).Install();
        new UIInstaller(Container).Install();
        new DiceInstaller(Container).Install();
        new StateInstaller(Container).Install();

    }
}
