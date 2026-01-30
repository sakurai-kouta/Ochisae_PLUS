using System.Diagnostics;

public static class VersionManager
{
    // 手動で管理するベースバージョン
    public const string BaseVersion = "ver.0.01";
    // 体験版の場合はtrueにしておくこと。
    public const bool IsTrial = true;

    // ビルド時に上書きされる
    private static string buildVersion = BaseVersion;

    public static string Version => buildVersion;

#if UNITY_EDITOR
    // Editor からのみ変更可能
    public static void SetBuildVersion(string version)
    {
        buildVersion = version;
    }
#endif
}
