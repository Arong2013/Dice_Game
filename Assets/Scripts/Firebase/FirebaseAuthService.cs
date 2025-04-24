using System;
using System.Threading.Tasks;
using Firebase;
using Firebase.Auth;
using UnityEngine;

public static class FirebaseAuthService
{
    private static FirebaseAuth _auth;
    private static string _userId;
    public static string UserId => _userId;

    public static async Task InitializeAndLoginAsync()
    {
        _userId = "TEST_EDITOR_UID_001";
        Debug.Log($"[Auth] 에디터 고정 UID 사용: {_userId}");
#if UNITY_EDITOR
        // ✅ 에디터에서는 고정 UID 사용

#else
        var status = await FirebaseApp.CheckAndFixDependenciesAsync();
        if (status != DependencyStatus.Available)
            throw new Exception($"Firebase dependencies not available: {status}");

        _auth = FirebaseAuth.DefaultInstance;

        var result = await _auth.SignInAnonymouslyAsync();
        _userId = result.User.UserId;

        Debug.Log($"[Auth] Firebase 익명 로그인 성공: {_userId}");
#endif
    }
}
[System.Serializable]
public class UserData
{
    public string username;
    public string email;
}
