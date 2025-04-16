public class DiceInstaller
{
    private readonly MyContainer _container;

    public DiceInstaller(MyContainer container) => _container = container;

    public void Install()
    {
        //_container.Bind<DiceContainer>().To<DiceContainer>().AsSingleton();
        //_container.Bind<IDiceSelector>().To<DiceSelector>().AsTransient();

        //var diceContainer = _container.Resolve<DiceContainer>();
        //diceContainer.Register("strength", new StrengthDice());
        //diceContainer.Register("magic", new MagicDice());
    }
}
