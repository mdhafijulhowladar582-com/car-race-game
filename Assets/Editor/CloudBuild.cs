using UnityEditor;
using UnityEditor.SceneManagement;

public static class CloudBuild
{
    public static void PreExport()
    {
        const string scenePath = "Assets/Scenes/Race.unity";

        if (!System.IO.File.Exists(scenePath))
            V1SceneBuilder.BuildRaceScene();

        V2HUDBuilder.Build();
        V3RaceHUDBuilder.Build();

        EditorSceneManager.SaveOpenScenes();
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        EditorBuildSettings.scenes = new[]
        {
            new EditorBuildSettingsScene(scenePath, true)
        };
    }
}
