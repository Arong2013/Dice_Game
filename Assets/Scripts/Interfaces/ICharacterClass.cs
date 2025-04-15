public interface ICharacter
{
    string ID { get; }
    void Move(int steps);
    void Attack(ICharacter target);
    void TakeDamage(int amount);
}
public interface IBuffable
{
    void ApplyBuff(string buffId);
    void RemoveBuff(string buffId);
}
public interface IDamageable
{
    void TakeDamage(int amount);
    bool IsDead();
}
public interface ITurnActor
{
    int TurnPriority { get; }
    void ExecuteTurn();
}