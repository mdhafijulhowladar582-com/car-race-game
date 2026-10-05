using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CarController : MonoBehaviour
{
    [SerializeField] private float minSpeed = 8f;
    [SerializeField] private float maxSpeed = 18f;
    [SerializeField] private float acceleration = 10f;
    [SerializeField] private float braking = 18f;
    [SerializeField] private float steeringStrength = 7f;
    [SerializeField] private float turnAtSpeed = 65f;
    [SerializeField] private float lateralGrip = 9f;

    private Rigidbody rb;
    private float currentSpeed;
    private AudioManager audioManager;

    public float Speed01 => maxSpeed <= 0f ? 0f : Mathf.Clamp01(currentSpeed / maxSpeed);
    public float CurrentSpeed => currentSpeed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.centerOfMass = new Vector3(0f, -0.45f, 0f);
        currentSpeed = 0f;
        audioManager = AudioManager.GetOrCreate();
    }

    private void Update()
    {
        if (GameManager.Instance != null &&
            (GameManager.Instance.IsGameOver || GameManager.Instance.RaceFinished || !GameManager.Instance.RaceStarted))
        {
            audioManager.StopEngine();
            return;
        }

        audioManager.UpdateEngine(Speed01);
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance != null)
        {
            if (GameManager.Instance.IsGameOver || GameManager.Instance.RaceFinished || !GameManager.Instance.RaceStarted)
            {
                StopCar();
                return;
            }
        }

        float steering = MobileInput.Steering;
        currentSpeed = Mathf.MoveTowards(currentSpeed, maxSpeed, acceleration * Time.fixedDeltaTime);

        Vector3 forwardVelocity = transform.forward * currentSpeed;
        Vector3 velocity = rb.linearVelocity;
        rb.linearVelocity = new Vector3(forwardVelocity.x, velocity.y, forwardVelocity.z);

        float turnScale = Mathf.Lerp(0.65f, 1.15f, Speed01);
        float turnAmount = steering * turnAtSpeed * turnScale * Time.fixedDeltaTime;
        rb.MoveRotation(rb.rotation * Quaternion.Euler(0f, turnAmount, 0f));

        Vector3 lateralVelocity = Vector3.Project(rb.linearVelocity, transform.right);
        rb.linearVelocity -= lateralVelocity * steeringStrength * lateralGrip * 0.1f * Time.fixedDeltaTime;
    }

    private void StopCar()
    {
        currentSpeed = 0f;
        Vector3 velocity = rb.linearVelocity;
        rb.linearVelocity = new Vector3(0f, velocity.y, 0f);
    }

    public void SetMaxSpeed(float value)
    {
        maxSpeed = Mathf.Max(minSpeed, value);
    }

    public void SetAcceleration(float value)
    {
        acceleration = Mathf.Max(0f, value);
    }
}
