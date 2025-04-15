public interface ITile
{
    int Index { get; }
    void OnLand(ICharacter character);
}
public interface IBoardEvent
{
    void Trigger(ICharacter character);
}
public interface IMovable
{
    int CurrentTileIndex { get; }
    void MoveTo(int targetTileIndex);
}