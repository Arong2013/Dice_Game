public class BasicDiceRoller : IDiceRoller
{
    public int Roll()
    {
        return UnityEngine.Random.Range(1, 7);
    }
}