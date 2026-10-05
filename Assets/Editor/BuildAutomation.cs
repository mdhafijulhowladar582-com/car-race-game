using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class BuildAutomation
{
    private const string ScenePath = "Assets/Scenes/Race.unity";
    private const string OutputPath = "build/Android/CarRaceGame.apk";

    public static void BuildAndroid()
    {
        EnsureScene();

        BuildPlayerOptions options = new BuildPlayerOptions
        {
            scenes = new[] { ScenePath },
            locationPathName = OutputPath,
            target = BuildTarget.Android,
            options = BuildOptions.None
        };

        Directory.CreateDirectory(Path.GetDirectoryName(OutputPath));

        BuildReport report = BuildPipeline.BuildPlayer(options);

        if (report.summary.result != BuildResult.Succeeded)
            throw new System.Exception("Android build failed: " + report.summary.result);

        Debug.Log("Android APK created: " + OutputPath);
    }

    private static void EnsureScene()
    {
        if (!File.Exists(ScenePath))
            V1SceneBuilder.BuildRaceScene();

        V2HUDBuilder.Build();
        V3RaceHUDBuilder.Build();

        EditorSceneManager.SaveOpenScenes();
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        if (!File.Exists(ScenePath))
            throw new System.Exception("Race scene could not be created.");
    }
}
