using System;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEditor.Build.Reporting;

public static class ApocalypseBuild2019
{
    public static void ExportIOS()
    {
        string[] scenes = EditorBuildSettings.scenes
            .Where(s => s.enabled)
            .Select(s => s.path)
            .ToArray();

        if (scenes.Length == 0 ||
            !scenes.Any(s =>
                s.EndsWith("DEMOLEVEL.unity")))
        {
            throw new Exception(
                "Original DLC scenes missing"
            );
        }

        PlayerSettings.productName =
            "Slendytubbies Apocalypse DLC";

        PlayerSettings.SetApplicationIdentifier(
            BuildTargetGroup.iOS,
            "com.negelinho.apocalypsedlc"
        );

        PlayerSettings.defaultInterfaceOrientation =
            UIOrientation.LandscapeLeft;

        PlayerSettings.SetScriptingBackend(
            BuildTargetGroup.iOS,
            ScriptingImplementation.IL2CPP
        );

        string output =
            Path.GetFullPath("Builds/iOS");

        Directory.CreateDirectory(output);

        BuildReport report =
            BuildPipeline.BuildPlayer(
                new BuildPlayerOptions
                {
                    scenes = scenes,
                    locationPathName = output,
                    target = BuildTarget.iOS,
                    options = BuildOptions.None
                }
            );

        if (report.summary.result !=
            BuildResult.Succeeded)
        {
            throw new Exception(
                "iOS export failed: " +
                report.summary.result
            );
        }

        Debug.Log(
            "APOCALYPSE DLC IOS EXPORT SUCCESS"
        );
    }
}
