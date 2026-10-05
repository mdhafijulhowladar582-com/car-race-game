using UnityEngine;

public class TrackFeatureBuilder : MonoBehaviour
{
    [SerializeField] private int segmentCount = 36;
    [SerializeField] private float segmentLength = 6f;
    [SerializeField] private float roadWidth = 8f;
    [SerializeField] private float maxBendAngle = 5f;
    [SerializeField] private float rampHeight = 2.2f;
    [SerializeField] private float rampLength = 8f;

    private const string RootName = "GeneratedTrack";

    private void Awake()
    {
        if (GameObject.Find(RootName) == null)
            BuildTrack();
    }

    [ContextMenu("Build Bends and Ramps")]
    public void BuildTrack()
    {
        GameObject old = GameObject.Find(RootName);
        if (old != null)
            Destroy(old);

        GameObject root = new GameObject(RootName);
        Vector3 position = transform.position;
        Quaternion rotation = transform.rotation;

        Material road = CreateMaterial("Road", new Color(0.055f, 0.06f, 0.07f), 0f, 0.25f);
        Material line = CreateMaterial("RoadLine", new Color(0.95f, 0.8f, 0.08f), 0f, 0.35f);
        Material rail = CreateMaterial("Rail", new Color(0.35f, 0.38f, 0.42f), 0.7f, 0.65f);
        Material ramp = CreateMaterial("RampEdge", new Color(0.08f, 0.12f, 0.16f), 0.15f, 0.4f);

        float bend = 0f;
        float height = 0f;

        for (int i = 0; i < segmentCount; i++)
        {
            if (i > 0)
            {
                if (i % 9 == 0)
                    bend = Random.Range(-maxBendAngle, maxBendAngle);

                if (i == 10 || i == 24)
                    height += rampHeight;
                else if (i == 14 || i == 28)
                    height -= rampHeight;
            }

            rotation *= Quaternion.Euler(0f, bend, 0f);

            float y = height;
            if (i == 10 || i == 24)
                y += rampHeight * 0.5f;
            else if (i == 14 || i == 28)
                y -= rampHeight * 0.5f;

            Vector3 center = position + rotation * (Vector3.forward * (segmentLength * 0.5f));
            center.y = y;

            GameObject roadSegment = GameObject.CreatePrimitive(PrimitiveType.Cube);
            roadSegment.name = $"RoadSegment_{i:00}";
            roadSegment.transform.SetParent(root.transform);
            roadSegment.transform.position = center;
            roadSegment.transform.rotation = rotation;
            roadSegment.transform.localScale = new Vector3(roadWidth, 0.35f, segmentLength + 0.15f);
            roadSegment.GetComponent<Renderer>().sharedMaterial = road;

            AddRoadLine(root.transform, center + Vector3.up * 0.19f, rotation, line);
            AddBarrier(root.transform, center, rotation, rail, -1f);
            AddBarrier(root.transform, center, rotation, rail, 1f);

            if ((i >= 9 && i <= 14) || (i >= 23 && i <= 28))
                AddRampEdge(root.transform, center, rotation, ramp);

            position = center + rotation * (Vector3.forward * (segmentLength * 0.5f));
            position.y = height;
        }

        StaticBatchingUtility.Combine(root);
    }

    private void AddRoadLine(Transform parent, Vector3 position, Quaternion rotation, Material material)
    {
        GameObject marker = GameObject.CreatePrimitive(PrimitiveType.Cube);
        marker.name = "CenterLine";
        marker.transform.SetParent(parent);
        marker.transform.position = position;
        marker.transform.rotation = rotation;
        marker.transform.localScale = new Vector3(0.12f, 0.025f, segmentLength * 0.62f);
        marker.GetComponent<Renderer>().sharedMaterial = material;
        RemoveCollider(marker);
    }

    private void AddBarrier(Transform parent, Vector3 center, Quaternion rotation, Material material, float side)
    {
        Vector3 offset = rotation * (Vector3.right * side * (roadWidth * 0.5f + 0.35f));
        GameObject barrier = GameObject.CreatePrimitive(PrimitiveType.Cube);
        barrier.name = side < 0f ? "LeftBarrier" : "RightBarrier";
        barrier.transform.SetParent(parent);
        barrier.transform.position = center + offset + Vector3.up * 0.7f;
        barrier.transform.rotation = rotation;
        barrier.transform.localScale = new Vector3(0.18f, 1.1f, segmentLength);
        barrier.GetComponent<Renderer>().sharedMaterial = material;
    }

    private void AddRampEdge(Transform parent, Vector3 center, Quaternion rotation, Material material)
    {
        for (int side = -1; side <= 1; side += 2)
        {
            Vector3 offset = rotation * (Vector3.right * side * (roadWidth * 0.5f - 0.18f));
            GameObject edge = GameObject.CreatePrimitive(PrimitiveType.Cube);
            edge.name = "RampEdge";
            edge.transform.SetParent(parent);
            edge.transform.position = center + offset + Vector3.up * 0.25f;
            edge.transform.rotation = rotation;
            edge.transform.localScale = new Vector3(0.12f, 0.5f, segmentLength);
            edge.GetComponent<Renderer>().sharedMaterial = material;
            RemoveCollider(edge);
        }
    }

    private Material CreateMaterial(string name, Color color, float metallic, float smoothness)
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

        return material;
    }

    private void RemoveCollider(GameObject obj)
    {
        Collider collider = obj.GetComponent<Collider>();
        if (collider != null)
            Destroy(collider);
    }
}
