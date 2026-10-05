using UnityEngine;

public class AndroidOptimization : MonoBehaviour
{
    [SerializeField] private int targetFrameRate = 60;
    [SerializeField] private int physicsRate = 50;
    [SerializeField] private bool disableVSync = true;
    [SerializeField] private bool reduceResolutionOnMobile = false;
    [SerializeField, Range(0.5f, 1f)] private float mobileResolutionScale = 0.85f;

    private void Awake()
    {
        Apply();
    }

    [ContextMenu("Apply Android Optimization")]
    public void Apply()
    {
        if (disableVSync)
            QualitySettings.vSyncCount = 0;

        Application.targetFrameRate = Mathf.Clamp(targetFrameRate, 30, 60);
        QualitySettings.maxQueuedFrames = 2;
        Time.fixedDeltaTime = 1f / Mathf.Clamp(physicsRate, 30, 60);
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        if (Application.platform == RuntimePlatform.Android)
        {
            Screen.orientation = ScreenOrientation.LandscapeLeft;

            if (reduceResolutionOnMobile)
            {
                int width = Mathf.Max(960, Mathf.RoundToInt(Screen.currentResolution.width * mobileResolutionScale));
                int height = Mathf.Max(540, Mathf.RoundToInt(Screen.currentResolution.height * mobileResolutionScale));
                Screen.SetResolution(width, height, FullScreenMode.FullScreenWindow);
            }
        }
    }

    private void OnApplicationPause(bool pause)
    {
        if (!pause)
            Apply();
    }
}
