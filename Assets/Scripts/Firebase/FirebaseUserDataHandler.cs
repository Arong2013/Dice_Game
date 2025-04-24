using System.Threading.Tasks;
using Firebase;
using Firebase.Database;
using UnityEngine;
using Newtonsoft.Json;

public class FirebaseUserDataHandler
{
    private DatabaseReference _dbRef;

    public async Task PrepareUserData()
    {
        string uid = FirebaseAuthService.UserId;

        var app = FirebaseApp.DefaultInstance;
        _dbRef = FirebaseDatabase.GetInstance(app).RootReference;

        var snapshot = await _dbRef.Child("Users").Child(uid).GetValueAsync();

        if (snapshot.Exists)
        {
            Debug.Log("✅ 유저 데이터 로드 완료");

            // 🔁 JSON → PlayerProfile 변환
            string json = snapshot.GetRawJsonValue();
            PlayerProfile profile = JsonConvert.DeserializeObject<PlayerProfile>(json);
            Debug.Log($"불러온 캐릭터 ID: {profile.SelectedCharacterId}");
        }
        else
        {
            // 🔁 신규 플레이어 데이터 생성
            var newProfile = new PlayerProfile
            {
                SelectedCharacterId = 1,
                UnlockedCharacters = new() { 0 },
                CharacterAffection = new() { { 1, 20 }, { 2, 50 } },
                EquippedPetId = -1,
                OwnedPetIds = new() { 3 },
                PetAffection = new() { { 3, 100 } },
                PetLevel = new() { { 3, 2 } },
                UnlockedUpgrades = new(),
                Achievements = new(),
                TotalPlayCount = 1,
                TotalWinCount = 0,
                TotalDeathCount = 1,
                Resources = new LobbyResourceData(),
                PermanentBonus = new PermanentBonusData()
            };

            var json = JsonConvert.SerializeObject(newProfile, new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Include,
                MissingMemberHandling = MissingMemberHandling.Ignore
            });
            await _dbRef.Child("Users").Child(uid).SetRawJsonValueAsync(json);

            Debug.Log("✅ 신규 플레이어 데이터 저장 완료");
        }
    }
}
