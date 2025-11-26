using UnityEngine;

public class FPSCap : MonoBehaviour
{
    void Awake()
    {
        // Make Unity *not* override VSync
        Application.targetFrameRate = 60;

        // Enable VSync (1 = match display refresh rate)
        QualitySettings.vSyncCount = 1;
    }
}