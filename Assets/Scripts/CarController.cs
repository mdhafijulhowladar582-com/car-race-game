using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CarController : MonoBehaviour
{
    [SerializeField] private float forwardSpeed = 14f;
    [SerializeField] private float maxSpeed = 18f;
    [SerializeField] private float steeringStrength = 7f;
    [SerializeField] private float turnAtSpeed = 55f;
    [SerializeField] private float acceleration = 8f;

    private Rigidbody rb;
    private float currentSpeed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.centerOfMass = new Vector3(0f, -0.45f, 0f);
        currentSpeed = forwardSpeed;
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance != null && GameManager.Instance.IsGameOver)
        {
            rb.linearVelocity = Vector3.zero;
            return;
        }

        float steering = MobileInput.Steering;

        currentSpeed = Mathf.MoveTowards(currentSpeed, maxSpeed, acceleration * Time.fixedDeltaTime);

        Vector3 forwardVelocity = transform.forward * currentSpeed;
        rb.linearVelocity = new Vector3(forwardVelocity.x, rb.linearVelocity.y, forwardVelocity.z);

        float turnAmount = steering * turnAtSpeed * Time.fixedDeltaTime;
        Quaternion turn = Quaternion.Euler(0f, turnAmount, 0f);
        rb.MoveRotation(rb.rotation * turn);

        Vector3 lateralVelocity = Vector3.Project(rb.linearVelocity, transform.right);
        rb.linearVelocity -= lateralVelocity * steeringStrength * Time.fixedDeltaTime;
    }
}
