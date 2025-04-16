public interface IDiceRoller
{
    int Roll();
}
public interface IRollRule
{
    void Apply(int[] rolls, ICharacter character);
}
public interface IDiceSelector
{
    IDiceRoller SelectForCurrentContext();
}