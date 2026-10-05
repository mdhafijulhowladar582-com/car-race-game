using UnityEngine;

public class MobileInput : MonoBehaviour
{
    public static float Steering { get; private set; }

    [SerializeField] private float swipeSensitivity = 0.01f;
    [SerializeField] private float steeringSmoothing = 8f;

    private float targetSteering;
    private int activeFingerId = -1;
    private Vector2 touchStart;

    private void Update()
    {
        targetSteering = 0f;

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

                if (touch.fingerId == activeFingerId)
                {
                    if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
                    {
                        float deltaX = touch.position.x - touchStart.x;
                        targetSteering = Mathf.Clamp(deltaX * swipeSensitivity, -1f, 1f);
                    }

                    if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                    {
                        activeFingerId = -1;
                    }
                }
            }
        }
        else
        {
            float keyboardSteering = Input.GetAxisRaw("Horizontal");
            targetSteering = keyboardSteering;
        }

        Steering = Mathf.Lerp(Steering, targetSteering, steeringSmoothing * Time.deltaTime);
    }
}
