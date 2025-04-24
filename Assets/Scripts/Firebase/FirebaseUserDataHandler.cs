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

            // 🔁 JSON -> UserData 파싱
            string json = snapshot.GetRawJsonValue();
            UserData user = JsonConvert.DeserializeObject<UserData>(json);
            Debug.Log($"불러온 유저 이름: {user.username}");
        }
        else
        {
            var newUser = new UserData
            {
                username = "Player_" + uid.Substring(0, 5),
                email = "guest@example.com"
            };

            // 🔁 UserData -> JSON 변환
            string json = JsonConvert.SerializeObject(newUser);
            await _dbRef.Child("Users").Child(uid).SetRawJsonValueAsync(json);

            Debug.Log("✅ 신규 유저 데이터 저장 완료");
        }
    }
}
