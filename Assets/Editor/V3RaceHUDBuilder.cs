using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public static class V3RaceHUDBuilder
{
    [MenuItem("Car Race/V3/Build Start & Win UI")]
    public static void Build()
    {
        const string scenePath = "Assets/Scenes/Race.unity";

        if (!System.IO.File.Exists(scenePath))
        {
            Debug.LogError("Race.unity was not found. Build V1 scene first.");
            return;
        }

        var scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);

        Canvas canvas = Object.FindFirstObjectByType<Canvas>();
        if (canvas == null)
        {
            var canvasObject = new GameObject("Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            canvas = canvasObject.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        }

        canvas.GetComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvas.GetComponent<CanvasScaler>().referenceResolution = new Vector2(1080, 1920);

        var existing = Object.FindFirstObjectByType<RaceHUD>();
        if (existing != null)
            Object.DestroyImmediate(existing.gameObject);

        var startPanel = CreatePanel(canvas.transform, "StartPanel", new Vector2(0, 0), new Vector2(1, 1));
        CreateText(startPanel.transform, "Title", "CAR RACE", 96, new Vector2(0.5f, 0.62f));
        var startButton = CreateButton(startPanel.transform, "StartButton", "START RACE", new Vector2(0.5f, 0.42f));

        var winPanel = CreatePanel(canvas.transform, "WinPanel", new Vector2(0, 0), new Vector2(1, 1));
        CreateText(winPanel.transform, "Title", "YOU WIN!", 100, new Vector2(0.5f, 0.62f));
        var winRestartButton = CreateButton(winPanel.transform, "RestartButton", "RESTART", new Vector2(0.5f, 0.42f));
        winPanel.SetActive(false);

        var hudObject = new GameObject("RaceHUD");
        var hud = hudObject.AddComponent<RaceHUD>();

        var so = new SerializedObject(hud);
        so.FindProperty("startPanel").objectReferenceValue = startPanel;
        so.FindProperty("winPanel").objectReferenceValue = winPanel;
        so.FindProperty("startButton").objectReferenceValue = startButton;
        so.FindProperty("winRestartButton").objectReferenceValue = winRestartButton;
        so.ApplyModifiedPropertiesWithoutUndo();

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);

        Debug.Log("V3 Start & Win UI built successfully.");
    }

    private static GameObject CreatePanel(Transform parent, string name, Vector2 anchorMin, Vector2 anchorMax)
    {
        var go = new GameObject(name, typeof(RectTransform), typeof(Image));
        go.transform.SetParent(parent, false);

        var rect = go.GetComponent<RectTransform>();
        rect.anchorMin = anchorMin;
        rect.anchorMax = anchorMax;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;

        var image = go.GetComponent<Image>();
        image.color = new Color(0, 0, 0, 0.72f);

        return go;
    }

    private static Text CreateText(Transform parent, string name, string value, int size, Vector2 anchor)
    {
        var go = new GameObject(name, typeof(RectTransform), typeof(Text));
        go.transform.SetParent(parent, false);

        var rect = go.GetComponent<RectTransform>();
        rect.anchorMin = anchor;
        rect.anchorMax = anchor;
        rect.sizeDelta = new Vector2(900, 180);

        var text = go.GetComponent<Text>();
        text.text = value;
        text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        text.fontSize = size;
        text.alignment = TextAnchor.MiddleCenter;
        text.color = Color.white;

        return text;
    }

    private static Button CreateButton(Transform parent, string name, string label, Vector2 anchor)
    {
        var go = new GameObject(name, typeof(RectTransform), typeof(Image), typeof(Button));
        go.transform.SetParent(parent, false);

        var rect = go.GetComponent<RectTransform>();
        rect.anchorMin = anchor;
        rect.anchorMax = anchor;
        rect.sizeDelta = new Vector2(520, 150);

        var image = go.GetComponent<Image>();
        image.color = new Color(0.12f, 0.55f, 1f, 1f);

        var button = go.GetComponent<Button>();

        var text = CreateText(go.transform, "Label", label, 52, new Vector2(0.5f, 0.5f));
        text.rectTransform.sizeDelta = new Vector2(500, 140);

        return button;
    }
}
