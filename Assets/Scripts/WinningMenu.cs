using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class WinningMenu : MonoBehaviour
{
    public GameObject winMenu;
    public RawImage fadePanel;
    public float fadeDuration = 2f;

    public Transform targetPoint;
    public float moveSpeed = 5f;
    public float rotateSpeed = 180f; // degrees per second

    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return;
        if (other.CompareTag("Player"))
        {
            triggered = true;
            winMenu.SetActive(true);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            PlayerController pc = other.GetComponent<PlayerController>();
            if (pc != null) pc.DisableControl();

            StartCoroutine(WinningSequence(pc));
        }
    }

    private IEnumerator WinningSequence(PlayerController player)
    {
        CharacterController cc = player.characterController;
        Transform t = player.transform;

        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;

            // Move toward target
            Vector3 dir = targetPoint.position - t.position;
            dir.y = 0; // keep horizontal
            if (dir.magnitude > 0.01f)
                cc.Move(dir.normalized * Mathf.Min(moveSpeed * Time.unscaledDeltaTime, dir.magnitude));

            // Rotate toward target
            if (dir.magnitude > 0.01f)
            {
                Quaternion targetRot = Quaternion.LookRotation(dir);
                t.rotation = Quaternion.RotateTowards(t.rotation, targetRot, rotateSpeed * Time.unscaledDeltaTime);
            }

            // Fade screen/audio
            float tNorm = Mathf.Clamp01(elapsed / fadeDuration);
            AudioListener.volume = Mathf.Lerp(1f, 0f, tNorm);
            if (fadePanel)
            {
                Color c = fadePanel.color;
                c.a = tNorm;
                fadePanel.color = c;
            }

            yield return null;
        }

        // Snap to target
        t.position = targetPoint.position;

        // Keep original fade panel color but alpha = 1
        if (fadePanel)
        {
            Color c = fadePanel.color;
            c.a = 1f;
            fadePanel.color = c;
        }

        AudioListener.volume = 0f;

        // Stop time
        Time.timeScale = 0f;
    }

    public void Retry()
    {
        AudioListener.volume = 1f;
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void BackToMenu()
    {
        AudioListener.volume = 1f;
        Time.timeScale = 1f;
        SceneManager.LoadScene("StartMenu");
    }
}