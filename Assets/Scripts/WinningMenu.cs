using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class WinningMenu : MonoBehaviour
{
    public GameObject winMenu;
    public RawImage fadePanel;
    public float fadeDuration = 2f;

    public bool triggered = false;
    public static bool hasWon = false;

    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return;
        if (other.CompareTag("Player"))
        {
            triggered = true;
            WinningMenu.hasWon = true;

            winMenu.SetActive(true);

            StartCoroutine(FadeOutAudio());
            StartCoroutine(FadeScreen());
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

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        PlayerController pc = player.GetComponent<PlayerController>();
        if (pc != null) pc.canMove = false;

        Time.timeScale = 0f;
    }


    public void Retry()
    {
        StopAllCoroutines();
        AudioListener.volume = 1f;
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void BackToMenu()
    {
        StopAllCoroutines();
        AudioListener.volume = 1f;
        Time.timeScale = 1f;
        SceneManager.LoadScene("StartMenu");
    }

    private void Awake()
    {
        AudioListener.volume = 1f;
    }
}
