using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameOverMenu : MonoBehaviour
{
    public GameObject gameOverUI;
    public Camera playerCamera;
    public Transform monster;
    public float headOffsetY = 1.5f;
    public float lookSpeed = 2f;
    // public Light torchLight;
    // private float originalTorchIntensity;  

    public void ShowGameOver()  
    {  
        gameOverUI.SetActive(true);  

        Cursor.lockState = CursorLockMode.None;  
        Cursor.visible = true;  

        // if (torchLight != null)  
        // {  
        //     originalTorchIntensity = torchLight.intensity;  
        //     StartCoroutine(SmoothDimTorch());  
        // }  

        StartCoroutine(SmoothLookAtMonster());  
        StartCoroutine(FreezeTimeNextFrame());  
    }  

    private IEnumerator SmoothLookAtMonster()  
    {  
        Vector3 targetPosition = monster.position + new Vector3(0, headOffsetY, 0);  
        Quaternion startRotation = playerCamera.transform.rotation;  
        Quaternion targetRotation = Quaternion.LookRotation(targetPosition - playerCamera.transform.position);  

        float t = 0f;  
        while (t < 1f)  
        {  
            t += Time.unscaledDeltaTime * lookSpeed;  
            playerCamera.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);  
            yield return null;  
        }  
        playerCamera.transform.rotation = targetRotation;  
    }  

    // private IEnumerator SmoothDimTorch()  
    // {  
    //     float targetIntensity = originalTorchIntensity * 0.05f;  
    //     float startIntensity = torchLight.intensity;  
    //     float t = 0f;  
    //     while (t < 1f)  
    //     {  
    //         t += Time.unscaledDeltaTime * lookSpeed;  
    //         torchLight.intensity = Mathf.Lerp(startIntensity, targetIntensity, t);  
    //         yield return null;  
    //     }  
    //     torchLight.intensity = targetIntensity;  
    // }  

    private IEnumerator FreezeTimeNextFrame()  
    {  
        yield return null;  
        Time.timeScale = 0f;  
    }  

    public void Retry()  
    {  
        // if (torchLight != null) torchLight.intensity = originalTorchIntensity;  

        Time.timeScale = 1f;  
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);  
    }  

    public void BackToMenu()  
    {  
        // if (torchLight != null) torchLight.intensity = originalTorchIntensity;  

        Time.timeScale = 1f;  
        SceneManager.LoadScene("StartMenu");  
    }
}
