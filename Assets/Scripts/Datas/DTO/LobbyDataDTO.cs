using System;

[GameData(GameDTOType.Lobby)]
[Serializable]
public class LobbyDataDTO
{
    //public PlayerProfile Profile = new();
    public UserSettings Settings = new();
    public DailyLoginBonusData DailyBonus = new();
    public UserMetaData Meta = new();
    public int Tet;
}
