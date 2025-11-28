using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Maze");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
