using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public static class V2HUDBuilder
{
    private const string ScenePath = "Assets/Scenes/Race.unity";

    [MenuItem("Car Race/V2/Build Health & Coin UI")]
    public static void Build()
    {
        var scene = EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);

        Canvas canvas = Object.FindFirstObjectByType<Canvas>();
        if (canvas == null)
        {
            GameObject canvasObject = new GameObject("Game HUD");
            canvas = canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObject.AddComponent<CanvasScaler>();
            canvasObject.AddComponent<GraphicRaycaster>();
        }

        GameObject oldHud = GameObject.Find("HUD");
        if (oldHud != null)
            Object.DestroyImmediate(oldHud);

        GameObject hud = new GameObject("HUD");
        hud.transform.SetParent(canvas.transform, false);

        Text coins = CreateText(hud.transform, "CoinText", "COINS  0", new Vector2(0f, 1f), new Vector2(40f, -40f), TextAnchor.UpperLeft, 32);
        Text health = CreateText(hud.transform, "HealthText", "HP  3/3", new Vector2(1f, 1f), new Vector2(-40f, -40f), TextAnchor.UpperRight, 32);

        GameObject panel = new GameObject("GameOverPanel");
        panel.transform.SetParent(hud.transform, false);
        Image panelImage = panel.AddComponent<Image>();
        panelImage.color = new Color(0f, 0f, 0f, 0.82f);
        RectTransform panelRect = panel.GetComponent<RectTransform>();
        panelRect.anchorMin = new Vector2(0.15f, 0.25f);
        panelRect.anchorMax = new Vector2(0.85f, 0.75f);
        panelRect.offsetMin = Vector2.zero;
        panelRect.offsetMax = Vector2.zero;

        Text title = CreateText(panel.transform, "Title", "GAME OVER", new Vector2(0.5f, 0.72f), Vector2.zero, TextAnchor.MiddleCenter, 52);
        title.rectTransform.sizeDelta = new Vector2(500f, 100f);

        GameObject buttonObject = new GameObject("RestartButton");
        buttonObject.transform.SetParent(panel.transform, false);
        Button button = buttonObject.AddComponent<Button>();
        Image buttonImage = buttonObject.AddComponent<Image>();
        buttonImage.color = new Color(1f, 1f, 1f, 0.95f);

        RectTransform buttonRect = buttonObject.GetComponent<RectTransform>();
        buttonRect.anchorMin = new Vector2(0.3f, 0.18f);
        buttonRect.anchorMax = new Vector2(0.7f, 0.38f);
        buttonRect.offsetMin = Vector2.zero;
        buttonRect.offsetMax = Vector2.zero;

        Text buttonText = CreateText(buttonObject.transform, "Text", "RESTART", new Vector2(0.5f, 0.5f), Vector2.zero, TextAnchor.MiddleCenter, 30);
        buttonText.rectTransform.sizeDelta = new Vector2(300f, 70f);

        GameObject hudLogic = new GameObject("GameHUD");
        GameHUD logic = hudLogic.AddComponent<GameHUD>();
        SetReference(logic, "coinText", coins);
        SetReference(logic, "healthText", health);
        SetReference(logic, "gameOverPanel", panel);

        button.onClick.AddListener(logic.Restart);

        panel.SetActive(false);

        EditorSceneManager.SaveScene(scene, ScenePath);
        AssetDatabase.SaveAssets();
        Debug.Log("V2 Health + Coin UI created.");
    }

    private static Text CreateText(Transform parent, string name, string value, Vector2 anchor, Vector2 position, TextAnchor alignment, int size)
    {
        GameObject obj = new GameObject(name);
        obj.transform.SetParent(parent, false);
        Text text = obj.AddComponent<Text>();
        text.text = value;
        text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        text.fontSize = size;
        text.alignment = alignment;
        text.color = Color.white;

        RectTransform rect = text.rectTransform;
        rect.anchorMin = anchor;
        rect.anchorMax = anchor;
        rect.pivot = new Vector2(anchor.x, anchor.y);
        rect.anchoredPosition = position;
        rect.sizeDelta = new Vector2(350f, 80f);

        return text;
    }

    private static void SetReference(Object target, string property, Object value)
    {
        SerializedObject so = new SerializedObject(target);
        SerializedProperty p = so.FindProperty(property);
        p.objectReferenceValue = value;
        so.ApplyModifiedPropertiesWithoutUndo();
    }
}
