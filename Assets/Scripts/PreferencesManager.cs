using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PreferencesManager : MonoBehaviour
{
    public TMP_Dropdown difficultyDropdown;
    public TMP_InputField nameInputField; // 1. ADD THIS FIELD

    void Start()
    {
        int savedDifficulty = PlayerPrefs.GetInt("difficulty", 0);

        if (difficultyDropdown != null)
        {
            difficultyDropdown.value = savedDifficulty;
            difficultyDropdown.RefreshShownValue();
        }

        // 2. ADD THIS TO LOAD SAVED NAME
        if (nameInputField != null)
        {
            nameInputField.text = PlayerPrefs.GetString("playerName", "Player");
        }
    }

    public void StartGame()
    {
        if (difficultyDropdown != null)
        {
            PlayerPrefs.SetInt("difficulty", difficultyDropdown.value);
        }

        // 3. ADD THIS TO SAVE NAME
        if (nameInputField != null && !string.IsNullOrWhiteSpace(nameInputField.text))
        {
            PlayerPrefs.SetString("playerName", nameInputField.text.Trim());
        }
        else
        {
            PlayerPrefs.SetString("playerName", "Player");
        }

        PlayerPrefs.Save();

        SceneManager.LoadScene("wordGame");
    }
}