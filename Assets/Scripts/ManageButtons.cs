using UnityEngine;
using UnityEngine.SceneManagement;

public class ManageButtons : MonoBehaviour
{
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "wordGameStart")
            PlayerPrefs.SetInt("score", 0);
    }

    public void StartWordGame()
    {
        SceneManager.LoadScene("wordGame");
    }

    public void GoToPreferences()
    {
        SceneManager.LoadScene("Preferences");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}