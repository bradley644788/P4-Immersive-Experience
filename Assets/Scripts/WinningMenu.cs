using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class WinningMenu : MonoBehaviour
{
    public GameObject winMenu;
    public RawImage fadePanel;
    public float fadeDuration = 2f;

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
            if (pc != null) pc.canMove = false;

            StartCoroutine(FadeOutAudio());
            StartCoroutine(FadeScreen());
            Time.timeScale = 0f;
        }
    }

    private IEnumerator FadeOutAudio()
    {
        float startVolume = AudioListener.volume;
        float t = 0f;

        while (t < fadeDuration)
        {
            AudioListener.volume = Mathf.Lerp(startVolume, 0f, t / fadeDuration);
            t += Time.unscaledDeltaTime;
            yield return null;
        }

        AudioListener.volume = 0f;
    }

    private IEnumerator FadeScreen()
    {
        Color c = fadePanel.color;
        float t = 0f;

        while (t < fadeDuration)
        {
            c.a = Mathf.Lerp(0f, 1f, t / fadeDuration);
            fadePanel.color = c;
            t += Time.unscaledDeltaTime;
            yield return null;
        }

        c.a = 1f;
        fadePanel.color = c;
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
