using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void PlayGame()
    {
        Debug.Log("PLAY CLICKED");
        SceneManager.LoadScene(1);
    }

    public void OpenSettings()
    {
        Debug.Log("SETTINGS CLICKED");
    }

    public void CloseSettings()
    {
        Debug.Log("BACK CLICKED");
    }

    public void QuitGame()
    {
        Debug.Log("QUIT CLICKED");
        Application.Quit();
    }
}