using UnityEngine;

public class CarVisualBuilder : MonoBehaviour
{
    [SerializeField] private bool buildOnAwake = true;
    [SerializeField] private Color bodyColor = new Color(0.05f, 0.22f, 0.8f, 1f);

    private void Awake()
    {
        if (buildOnAwake)
            Build();
    }

    [ContextMenu("Build Car")]
    public void Build()
    {
        Transform old = transform.Find("GeneratedCarVisual");
        if (old != null)
            Destroy(old.gameObject);

        GameObject root = new GameObject("GeneratedCarVisual");
        root.transform.SetParent(transform, false);

        Material body = CreateMaterial("Body", bodyColor, 0.25f, 0.55f);
        Material dark = CreateMaterial("Dark", new Color(0.015f, 0.02f, 0.035f), 0.15f, 0.2f);
        Material glass = CreateMaterial("Glass", new Color(0.02f, 0.08f, 0.14f), 0.1f, 0.75f);
        Material tire = CreateMaterial("Tire", new Color(0.01f, 0.01f, 0.012f), 0.05f, 0.15f);
        Material rim = CreateMaterial("Rim", new Color(0.45f, 0.48f, 0.52f), 0.35f, 0.7f);
        Material red = CreateMaterial("TailLight", new Color(0.9f, 0.015f, 0.02f), 0.1f, 0.45f, true);
        Material white = CreateMaterial("HeadLight", new Color(0.85f, 0.95f, 1f), 0.1f, 0.7f, true);

        AddCube(root.transform, "Body", new Vector3(0f, 0.55f, 0f), new Vector3(2.05f, 0.55f, 4.25f), body, 8f);
        AddCube(root.transform, "Hood", new Vector3(0f, 0.86f, 1.05f), new Vector3(1.75f, 0.16f, 1.35f), body, 4f);
        AddCube(root.transform, "Cabin", new Vector3(0f, 1.18f, -0.25f), new Vector3(1.55f, 0.62f, 1.8f), glass, 5f);
        AddCube(root.transform, "Roof", new Vector3(0f, 1.5f, -0.25f), new Vector3(1.35f, 0.12f, 1.35f), body, 4f);

        AddCube(root.transform, "FrontSplitter", new Vector3(0f, 0.31f, 2.03f), new Vector3(1.9f, 0.12f, 0.25f), dark, 2f);
        AddCube(root.transform, "RearDiffuser", new Vector3(0f, 0.31f, -2.03f), new Vector3(1.9f, 0.15f, 0.25f), dark, 2f);

        AddCube(root.transform, "LeftHeadlight", new Vector3(-0.62f, 0.75f, 2.1f), new Vector3(0.48f, 0.12f, 0.12f), white, 2f);
        AddCube(root.transform, "RightHeadlight", new Vector3(0.62f, 0.75f, 2.1f), new Vector3(0.48f, 0.12f, 0.12f), white, 2f);
        AddCube(root.transform, "LeftTailLight", new Vector3(-0.62f, 0.72f, -2.1f), new Vector3(0.58f, 0.14f, 0.12f), red, 2f);
        AddCube(root.transform, "RightTailLight", new Vector3(0.62f, 0.72f, -2.1f), new Vector3(0.58f, 0.14f, 0.12f), red, 2f);

        AddCube(root.transform, "Spoiler", new Vector3(0f, 1.2f, -1.85f), new Vector3(1.8f, 0.12f, 0.28f), dark, 2f);
        AddCube(root.transform, "SpoilerLeft", new Vector3(-0.65f, 1.05f, -1.85f), new Vector3(0.1f, 0.35f, 0.1f), dark, 1f);
        AddCube(root.transform, "SpoilerRight", new Vector3(0.65f, 1.05f, -1.85f), new Vector3(0.1f, 0.35f, 0.1f), dark, 1f);

        AddWheel(root.transform, "FrontLeftWheel", new Vector3(-1.02f, 0.48f, 1.25f), tire, rim);
        AddWheel(root.transform, "FrontRightWheel", new Vector3(1.02f, 0.48f, 1.25f), tire, rim);
        AddWheel(root.transform, "RearLeftWheel", new Vector3(-1.02f, 0.48f, -1.25f), tire, rim);
        AddWheel(root.transform, "RearRightWheel", new Vector3(1.02f, 0.48f, -1.25f), tire, rim);

        AddCube(root.transform, "FrontGrille", new Vector3(0f, 0.48f, 2.16f), new Vector3(0.65f, 0.2f, 0.08f), dark, 1f);
        AddCube(root.transform, "LeftSideIntake", new Vector3(-0.9f, 0.55f, 1.65f), new Vector3(0.12f, 0.25f, 0.5f), dark, 1f);
        AddCube(root.transform, "RightSideIntake", new Vector3(0.9f, 0.55f, 1.65f), new Vector3(0.12f, 0.25f, 0.5f), dark, 1f);
    }

    private void AddWheel(Transform parent, string name, Vector3 position, Material tireMaterial, Material rimMaterial)
    {
        GameObject wheel = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        wheel.name = name;
        wheel.transform.SetParent(parent, false);
        wheel.transform.localPosition = position;
        wheel.transform.localRotation = Quaternion.Euler(0f, 0f, 90f);
        wheel.transform.localScale = new Vector3(0.52f, 0.18f, 0.52f);
        wheel.GetComponent<Renderer>().sharedMaterial = tireMaterial;
        DestroyCollider(wheel);

        GameObject hub = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        hub.name = name + "_Rim";
        hub.transform.SetParent(wheel.transform, false);
        hub.transform.localPosition = new Vector3(0f, 0f, position.x > 0f ? -0.92f : 0.92f);
        hub.transform.localRotation = Quaternion.identity;
        hub.transform.localScale = new Vector3(0.55f, 0.35f, 0.55f);
        hub.GetComponent<Renderer>().sharedMaterial = rim;
        DestroyCollider(hub);
    }

    private GameObject AddCube(Transform parent, string name, Vector3 position, Vector3 scale, Material material, float bevel)
    {
        GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
        obj.name = name;
        obj.transform.SetParent(parent, false);
        obj.transform.localPosition = position;
        obj.transform.localScale = scale;
        obj.GetComponent<Renderer>().sharedMaterial = material;
        DestroyCollider(obj);
        return obj;
    }

    private Material CreateMaterial(string name, Color color, float metallic, float smoothness, bool emission = false)
    {
        Shader shader = Shader.Find("Universal Render Pipeline/Lit");
        if (shader == null)
            shader = Shader.Find("Standard");

        Material material = new Material(shader);
        material.name = name;
        material.color = color;

        if (material.HasProperty("_Metallic"))
            material.SetFloat("_Metallic", metallic);
        if (material.HasProperty("_Smoothness"))
            material.SetFloat("_Smoothness", smoothness);

        if (emission && material.HasProperty("_EmissionColor"))
        {
            material.EnableKeyword("_EMISSION");
            material.SetColor("_EmissionColor", color * 2f);
        }

        return material;
    }

    private void DestroyCollider(GameObject obj)
    {
        Collider collider = obj.GetComponent<Collider>();
        if (collider != null)
            Destroy(collider);
    }
}
