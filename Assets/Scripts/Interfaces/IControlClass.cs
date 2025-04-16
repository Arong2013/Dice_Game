public interface IControllable
{
    void OnTurnStart();
    void OnTurnEnd();
    void HandleInput();
}
public interface IAIControllable
{
    void PerformAIAction();
}
public interface IClickable
{
    void OnClick();
}