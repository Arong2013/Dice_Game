using System;

public static class Logger
{
    /// <summary>
    /// 외부에서 원하는 로거로 덮어쓸 수 있다. (기본은 Console.WriteLine 사용)
    /// </summary>
    public static Action<string> CustomLogger;

    /// <summary>
    /// 외부에서 커스텀 로거를 등록한다.
    /// </summary>
    public static void SetLogger(Action<string> logger)
    {
        CustomLogger = logger;
    }

    /// <summary>
    /// 일반 로그 출력
    /// </summary>
    public static void Log(string message)
    {
        (CustomLogger ?? Console.WriteLine).Invoke(message);
    }

    /// <summary>
    /// 경고 로그 출력
    /// </summary>
    public static void LogWarning(string message)
    {
        (CustomLogger ?? Console.WriteLine).Invoke("[Warning] " + message);
    }

    /// <summary>
    /// 오류 로그 출력
    /// </summary>
    public static void LogError(string message)
    {
        (CustomLogger ?? Console.WriteLine).Invoke("[Error] " + message);
    }
}
