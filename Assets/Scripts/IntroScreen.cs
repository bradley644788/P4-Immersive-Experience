using UnityEngine;

public class StartScreen : MonoBehaviour
{
    public GameObject startUI;
    public PlayerController player;

    void Start()
    {
        Time.timeScale = 0f;
        if (player != null) player.enabled = false;
        if (startUI != null) startUI.SetActive(true);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.touchCount > 0)
        {
            if (startUI != null) startUI.SetActive(false);
            Time.timeScale = 1f;
            if (player != null) player.enabled = true;
            enabled = false;
        }
    }
}
