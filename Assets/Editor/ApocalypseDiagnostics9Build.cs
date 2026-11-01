using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.Callbacks;
using UnityEngine;

public class ApocalypseDiagnostics9Build : IPreprocessBuildWithReport
{
    public int callbackOrder { get { return 100; } }

    public void OnPreprocessBuild(BuildReport report)
    {
        if (report.summary.platform != BuildTarget.iOS) return;

        var icon = AssetDatabase.LoadAssetAtPath<Texture2D>(
            "Assets/FIX10Icon.png");

        if (icon == null)
            throw new System.Exception("FIX10 icon missing");

        var sizes = PlayerSettings.GetIconSizesForTargetGroup(
            BuildTargetGroup.iOS, IconKind.Application);

        var icons = new Texture2D[sizes.Length];

        for (int i = 0; i < icons.Length; i++)
            icons[i] = icon;

        PlayerSettings.SetIconsForTargetGroup(
            BuildTargetGroup.iOS, icons, IconKind.Application);
    }

    [PostProcessBuild(100)]
    public static void EnableFiles(
        BuildTarget target, string buildPath)
    {
        if (target != BuildTarget.iOS) return;

        #if UNITY_IOS
        string path = Path.Combine(buildPath, "Info.plist");

        var plist = new UnityEditor.iOS.Xcode.PlistDocument();
        plist.ReadFromFile(path);

        plist.root.SetBoolean("UIFileSharingEnabled", true);
        plist.root.SetBoolean(
            "LSSupportsOpeningDocumentsInPlace", true);
        plist.root.SetString(
            "CFBundleDisplayName", "Apocalypse FIX 10");

        plist.WriteToFile(path);
        #endif
    }
}
