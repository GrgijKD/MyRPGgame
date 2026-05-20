using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartNewGame()
    {
        PlayerPrefs.SetInt("LoadFromSave", 0);
        SceneManager.LoadScene(1);
    }

    public void LoadGame()
    {
        if (PlayerPrefs.HasKey("SaveDateTime"))
        {
            PlayerPrefs.SetInt("LoadFromSave", 1);
            SceneManager.LoadScene(1);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}