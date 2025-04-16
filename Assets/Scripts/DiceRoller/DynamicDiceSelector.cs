public class DynamicDiceSelector : IDiceSelector
{
    [Inject] private MyContainer _container;
    public IDiceRoller SelectForCurrentContext()
    {
        string type = DetermineType(); // 예: "basic", "fixed"
        return _container.Resolve<IDiceRoller>();
    }
    private string DetermineType()
    {
        return "basic"; // 나중에 게임 상태 기반으로 구현
    }
}
