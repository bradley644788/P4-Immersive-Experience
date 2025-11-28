using UnityEngine;

public class FrameLimiter : MonoBehaviour
{
    private int targetFPS;

    void Awake()
    {
        var ratio = Screen.currentResolution.refreshRateRatio;
        targetFPS = Mathf.RoundToInt((float)ratio.value);
        QualitySettings.vSyncCount = 1;
        Application.targetFrameRate = targetFPS;
    }

    void Update()
    {
        float frameTime = 1f / targetFPS;
        if (Time.unscaledDeltaTime < frameTime)
        {
            int sleep = Mathf.RoundToInt((frameTime - Time.unscaledDeltaTime) * 1000f);
            if (sleep > 0)
            {
                System.Threading.Thread.Sleep(sleep);
            }
        }
    }
}
