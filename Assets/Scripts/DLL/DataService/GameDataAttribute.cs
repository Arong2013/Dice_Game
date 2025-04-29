using System;
public enum GameDTOType
{
    Lobby,
    InGameSession,
    DailyBonus,
    Settings,
}

[AttributeUsage(AttributeTargets.Class)]
public class GameDataAttribute : Attribute
{
    public GameDTOType DataType { get; }

    public GameDataAttribute(GameDTOType dataType)
    {
        DataType = dataType;
    }
}
