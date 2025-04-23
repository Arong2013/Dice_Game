using System;

public static class Utils
{
    public static string ToSceneString(this SceneName scene)
    {
        return scene switch
        {
            SceneName.Title => "TitleScene",
            SceneName.Lobby => "LobbyScene",
            SceneName.Setup => "GameSetupScene",
            SceneName.Battle => "BattleScene",
            SceneName.Result => "ResultScene",
            _ => throw new ArgumentOutOfRangeException(nameof(scene), scene, null)
        };
    }
}
