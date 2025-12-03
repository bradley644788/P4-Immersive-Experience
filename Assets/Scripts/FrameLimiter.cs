using UnityEngine;
using UnityEngine.Rendering;
using System.Collections;

public class FrameLimiter : MonoBehaviour
{
    private int targetFPS;

    void Awake()
    {
        var ratio = Screen.currentResolution.refreshRateRatio;
        targetFPS = Mathf.RoundToInt((float)ratio.value);
        QualitySettings.vSyncCount = 1;
        Application.targetFrameRate = targetFPS;
        // OnDemandRendering.renderFrameInterval = 2;
        
        StartCoroutine(FrameLimitCoroutine());
    }

    private IEnumerator FrameLimitCoroutine()
    {
        float frameTime = 1f / targetFPS;

        while (true)
        {
            float startTime = Time.realtimeSinceStartup;

            yield return null;

            float delta = Time.realtimeSinceStartup - startTime;
            int sleep = Mathf.RoundToInt((frameTime - delta) * 1000f);
            if (sleep > 0)
            {
                System.Threading.Thread.Sleep(sleep);
            }
        }
    }
}
