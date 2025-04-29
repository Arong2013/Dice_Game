using System;

[Serializable]
public class UserSettings
{
    public bool BgmEnabled = true;
    public bool SfxEnabled = true;
    public float BgmVolume = 1.0f;
    public float SfxVolume = 1.0f;

    public string Language = "en"; // "ko", "ja" 등도 가능

    // 추가 설정 예시
    public bool PushNotificationEnabled = true;
}
[Serializable]
public class DailyLoginBonusData
{
    public DateTime LastClaimDate;
    public int Streak;
    public int TotalClaims;
}
[Serializable]
public class UserMetaData
{
    public DateTime CreatedAt;
    public DateTime LastLoginAt;
    public string CountryCode;
    public string DeviceModel;
}