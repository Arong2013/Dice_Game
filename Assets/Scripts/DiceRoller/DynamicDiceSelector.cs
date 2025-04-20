public class DynamicDiceSelector : IDiceSelector
{
    [Inject] private MyContainer _container;
    public IDiceRoller SelectForCurrentContext()
    {
        return _container.Resolve<IDiceRoller>();
    }
}
