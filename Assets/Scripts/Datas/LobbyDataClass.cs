using System;
using System.Collections.Generic;

using Newtonsoft.Json;

[Serializable]
public class PlayerProfile
{
    // 💰 자원 (로비 자산)
    public LobbyResourceData Resources = new();

    // 🧍 캐릭터
    public int SelectedCharacterId;
    public List<int> UnlockedCharacters;

    [JsonConverter(typeof(AutoKeyDictionaryConverter<int,int>))]
    public Dictionary<int, int> CharacterAffection; // 캐릭터 ID ↔ 호감도 수치

    // 🐾 펫
    public int EquippedPetId;
    public List<int> OwnedPetIds;


    [JsonConverter(typeof(AutoKeyDictionaryConverter<int, int>))]
    public Dictionary<int, int> PetAffection;       // 펫 ID ↔ 호감도 수치
    [JsonConverter(typeof(AutoKeyDictionaryConverter<int, int>))]
    public Dictionary<int, int> PetLevel;           // 펫 ID ↔ 강화 레벨

    // 📈 영구 스탯 성장
    public PermanentBonusData PermanentBonus = new();

    // 🎯 업적/해금
    public List<int> UnlockedUpgrades;
    public List<int> Achievements;

    // 🕹️ 게임 기록
    public int TotalPlayCount;
    public int TotalWinCount;
    public int TotalDeathCount;

    // 📅 마지막 플레이 타임, 저장 타임 등도 추가 가능
}
[Serializable]
public class LobbyResourceData
{
    public int Gold;
    public int StarFragments;
    public int AffectionPoints;
    public int AchievementPoints;
    public int SummonTickets;
    public int Keys;
}
[Serializable]
public class PermanentBonusData
{
    public int MoveBonus;
    public int HealthBonus;
    public int AttackBonus;
}
