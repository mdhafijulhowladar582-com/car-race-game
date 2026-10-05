using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 baseOffset = new Vector3(0f, 5f, -8f);
    [SerializeField] private float positionSmooth = 7f;
    [SerializeField] private float rotationSmooth = 9f;
    [SerializeField] private float speedHeight = 1.2f;
    [SerializeField] private float speedDistance = 2.2f;
    [SerializeField] private float speedFov = 12f;

    private Camera cam;
    private CarController car;
    private float baseFov = 65f;

    private void Awake()
    {
        cam = GetComponent<Camera>();
        baseFov = cam != null ? cam.fieldOfView : baseFov;
    }

    private void Start()
    {
        if (target != null)
            car = target.GetComponent<CarController>();
    }

    private void LateUpdate()
    {
        if (target == null)
            return;

        float speed01 = car != null ? car.Speed01 : 0f;
        Vector3 offset = baseOffset;
        offset.y += speed01 * speedHeight;
        offset.z -= speed01 * speedDistance;

        Vector3 desiredPosition = target.TransformPoint(offset);
        transform.position = Vector3.Lerp(transform.position, desiredPosition, 1f - Mathf.Exp(-positionSmooth * Time.deltaTime));

        Vector3 lookPoint = target.position + Vector3.up * (1.2f + speed01 * 0.35f) + target.forward * (4.5f + speed01 * 2f);
        Quaternion desiredRotation = Quaternion.LookRotation(lookPoint - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, 1f - Mathf.Exp(-rotationSmooth * Time.deltaTime));

        if (cam != null)
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, baseFov + speed01 * speedFov, 1f - Mathf.Exp(-6f * Time.deltaTime));
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
        car = target != null ? target.GetComponent<CarController>() : null;
    }
}
