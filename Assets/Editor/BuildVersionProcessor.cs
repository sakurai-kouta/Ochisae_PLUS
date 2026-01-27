using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using System;

public class BuildVersionProcessor : IPreprocessBuildWithReport
{
    // ŽÀs‡i¬‚³‚¢‚Ù‚Çæj
    public int callbackOrder => 0;

    public void OnPreprocessBuild(BuildReport report)
    {
        string baseVersion = VersionManager.BaseVersion;
        bool isTrial = VersionManager.IsTrial;
        string date = DateTime.Now.ToString("yyyyMMdd");
        string fullVersion;
        if (isTrial) 
        {
            fullVersion = $"{baseVersion}T_{date}";
        }
        else
        {
            fullVersion = $"{baseVersion}X_{date}";
        }

        // PlayerSettings ‚É”½‰f
        PlayerSettings.bundleVersion = fullVersion;

        // VersionManager ‚É‚à”½‰f
        VersionManager.SetBuildVersion(fullVersion);

        UnityEngine.Debug.Log($"Build Version: {fullVersion}");
    }
}
