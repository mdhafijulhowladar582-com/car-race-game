using UnityEngine;

public class MobileInput : MonoBehaviour
{
    public static float Steering { get; private set; }

    [SerializeField] private float swipeSensitivity = 0.008f;
    [SerializeField] private float steeringSmoothing = 10f;
    [SerializeField] private float returnSmoothing = 7f;

    private float targetSteering;
    private int activeFingerId = -1;
    private Vector2 touchStart;

    private void Update()
    {
        float desired = 0f;

        if (Input.touchCount > 0)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch touch = Input.GetTouch(i);

                if (touch.phase == TouchPhase.Began && activeFingerId == -1)
                {
                    activeFingerId = touch.fingerId;
                    touchStart = touch.position;
                }

                if (touch.fingerId != activeFingerId)
                    continue;

                if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
                {
                    float deltaX = touch.position.x - touchStart.x;
                    desired = Mathf.Clamp(deltaX * swipeSensitivity, -1f, 1f);
                }

                if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                {
                    activeFingerId = -1;
                }
            }
        }
        else
        {
            desired = Input.GetAxisRaw("Horizontal");
        }

        targetSteering = desired;
        float smoothing = Mathf.Abs(targetSteering) > 0.01f ? steeringSmoothing : returnSmoothing;
        Steering = Mathf.Lerp(Steering, targetSteering, smoothing * Time.deltaTime);
    }

    public static void ResetInput()
    {
        Steering = 0f;
    }
}
