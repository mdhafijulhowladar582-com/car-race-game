using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class V1SceneBuilder
{
    private const string SceneFolder = "Assets/Scenes";
    private const string ScenePath = "Assets/Scenes/Race.unity";
    private const string ScriptsFolder = "Assets/Scripts";

    [MenuItem("Car Race/V1/Build Race Scene")]
    public static void BuildRaceScene()
    {
        EnsureFolder(SceneFolder);

        Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

        CreateLight();
        CreateTrack();
        GameObject player = CreatePlayer();
        CreateCamera(player.transform);
        CreateGameManager();
        CreateCoins();
        CreateObstacles();
        CreateFinishLine();

        EditorSceneManager.SaveScene(scene, ScenePath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Selection.activeGameObject = player;
        Debug.Log("Car Race V1 Race scene created: " + ScenePath);
    }

    private static void CreateLight()
    {
        GameObject lightObject = new GameObject("Directional Light");
        Light light = lightObject.AddComponent<Light>();
        light.type = LightType.Directional;
        light.intensity = 1.2f;
        lightObject.transform.rotation = Quaternion.Euler(50f, -30f, 0f);
    }

    private static void CreateTrack()
    {
        GameObject road = GameObject.CreatePrimitive(PrimitiveType.Cube);
        road.name = "Road";
        road.transform.position = new Vector3(0f, -0.5f, 70f);
        road.transform.localScale = new Vector3(10f, 1f, 160f);
        CreateMaterial(road, "RoadMaterial", new Color(0.12f, 0.12f, 0.12f));

        GameObject leftBarrier = GameObject.CreatePrimitive(PrimitiveType.Cube);
        leftBarrier.name = "Left Road Barrier";
        leftBarrier.transform.position = new Vector3(-5.5f, 0.25f, 70f);
        leftBarrier.transform.localScale = new Vector3(0.5f, 1.5f, 160f);
        CreateMaterial(leftBarrier, "BarrierMaterial", new Color(0.8f, 0.8f, 0.8f));

        GameObject rightBarrier = GameObject.CreatePrimitive(PrimitiveType.Cube);
        rightBarrier.name = "Right Road Barrier";
        rightBarrier.transform.position = new Vector3(5.5f, 0.25f, 70f);
        rightBarrier.transform.localScale = new Vector3(0.5f, 1.5f, 160f);
        CreateMaterial(rightBarrier, "BarrierMaterial", new Color(0.8f, 0.8f, 0.8f));
    }

    private static GameObject CreatePlayer()
    {
        GameObject player = GameObject.CreatePrimitive(PrimitiveType.Cube);
        player.name = "Player Car";
        player.tag = "Player";
        player.transform.position = new Vector3(0f, 0.75f, 5f);
        player.transform.localScale = new Vector3(2f, 0.8f, 3.6f);
        CreateMaterial(player, "PlayerCarMaterial", new Color(0.05f, 0.35f, 0.9f));

        Rigidbody body = player.AddComponent<Rigidbody>();
        body.mass = 1200f;
        body.drag = 0.2f;
        body.angularDrag = 2f;
        body.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        AddScript(player, "CarController");
        AddScript(player, "PlayerHealth");

        CreateWheel(player.transform, new Vector3(-0.85f, -0.35f, 1.15f));
        CreateWheel(player.transform, new Vector3(0.85f, -0.35f, 1.15f));
        CreateWheel(player.transform, new Vector3(-0.85f, -0.35f, -1.15f));
        CreateWheel(player.transform, new Vector3(0.85f, -0.35f, -1.15f));

        return player;
    }

    private static void CreateWheel(Transform parent, Vector3 localPosition)
    {
        GameObject wheel = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        wheel.name = "Wheel";
        wheel.transform.SetParent(parent);
        wheel.transform.localPosition = localPosition;
        wheel.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
        wheel.transform.localScale = new Vector3(0.45f, 0.18f, 0.45f);
        CreateMaterial(wheel, "WheelMaterial", new Color(0.03f, 0.03f, 0.03f));
        Collider collider = wheel.GetComponent<Collider>();
        Object.DestroyImmediate(collider);
    }

    private static void CreateCamera(Transform player)
    {
        GameObject cameraObject = new GameObject("Main Camera");
        Camera camera = cameraObject.AddComponent<Camera>();
        camera.tag = "MainCamera";
        camera.fieldOfView = 65f;

        cameraObject.transform.position = player.position + new Vector3(0f, 5f, -8f);

        Component follow = AddScript(cameraObject, "CameraFollow");
        SetObjectReference(follow, "target", player.transform);
    }

    private static void CreateGameManager()
    {
        GameObject manager = new GameObject("Game Manager");
        AddScript(manager, "GameManager");
        AddScript(manager, "MobileInput");
    }

    private static void CreateCoins()
    {
        for (int i = 0; i < 12; i++)
        {
            float x = (i % 3 - 1) * 2.4f;
            float z = 18f + i * 10f;

            GameObject coin = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            coin.name = "Coin";
            coin.transform.position = new Vector3(x, 1.2f, z);
            coin.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
            coin.transform.localScale = new Vector3(0.7f, 0.12f, 0.7f);
            CreateMaterial(coin, "CoinMaterial", new Color(1f, 0.7f, 0.05f));

            Collider collider = coin.GetComponent<Collider>();
            collider.isTrigger = true;
            AddScript(coin, "Coin");
        }
    }

    private static void CreateObstacles()
    {
        float[] positions = { -3f, 3f, 0f, -2f, 2f, 0f };

        for (int i = 0; i < positions.Length; i++)
        {
            GameObject obstacle = GameObject.CreatePrimitive(PrimitiveType.Cube);
            obstacle.name = "Obstacle";
            obstacle.transform.position = new Vector3(positions[i], 0.75f, 30f + i * 20f);
            obstacle.transform.localScale = new Vector3(1.8f, 1.5f, 1.8f);
            CreateMaterial(obstacle, "ObstacleMaterial", new Color(0.9f, 0.15f, 0.08f));
            AddScript(obstacle, "Obstacle");
        }
    }

    private static void CreateFinishLine()
    {
        GameObject finish = GameObject.CreatePrimitive(PrimitiveType.Cube);
        finish.name = "Finish Line";
        finish.transform.position = new Vector3(0f, 0.05f, 148f);
        finish.transform.localScale = new Vector3(10f, 0.1f, 1.5f);
        CreateMaterial(finish, "FinishMaterial", new Color(0.95f, 0.95f, 0.95f));

        Collider collider = finish.GetComponent<Collider>();
        collider.isTrigger = true;
        AddScript(finish, "FinishLine");
    }

    private static Component AddScript(GameObject target, string className)
    {
        string[] guids = AssetDatabase.FindAssets(className + " t:MonoScript", new[] { ScriptsFolder });

        if (guids.Length == 0)
        {
            Debug.LogError("Could not find script: " + className);
            return null;
        }

        string path = AssetDatabase.GUIDToAssetPath(guids[0]);
        MonoScript script = AssetDatabase.LoadAssetAtPath<MonoScript>(path);
        System.Type type = script.GetClass();

        if (type == null)
        {
            Debug.LogError("Could not load class: " + className);
            return null;
        }

        return target.AddComponent(type);
    }

    private static void SetObjectReference(Component component, string propertyName, Object value)
    {
        if (component == null)
            return;

        SerializedObject serializedObject = new SerializedObject(component);
        SerializedProperty property = serializedObject.FindProperty(propertyName);

        if (property != null)
        {
            property.objectReferenceValue = value;
            serializedObject.ApplyModifiedPropertiesWithoutUndo();
        }
    }

    private static void CreateMaterial(GameObject target, string materialName, Color color)
    {
        string folder = "Assets/GeneratedMaterials";
        EnsureFolder(folder);

        string path = folder + "/" + materialName + ".mat";
        Material material = AssetDatabase.LoadAssetAtPath<Material>(path);

        if (material == null)
        {
            material = new Material(Shader.Find("Universal Render Pipeline/Lit"));

            if (material.shader == null)
                material.shader = Shader.Find("Standard");

            material.name = materialName;
            material.color = color;
            AssetDatabase.CreateAsset(material, path);
        }
        else
        {
            material.color = color;
            EditorUtility.SetDirty(material);
        }

        Renderer renderer = target.GetComponent<Renderer>();

        if (renderer != null)
            renderer.sharedMaterial = material;
    }

    private static void EnsureFolder(string folder)
    {
        if (AssetDatabase.IsValidFolder(folder))
            return;

        string[] parts = folder.Split('/');
        string current = parts[0];

        for (int i = 1; i < parts.Length; i++)
        {
            string next = current + "/" + parts[i];

            if (!AssetDatabase.IsValidFolder(next))
                AssetDatabase.CreateFolder(current, parts[i]);

            current = next;
        }
    }
}
