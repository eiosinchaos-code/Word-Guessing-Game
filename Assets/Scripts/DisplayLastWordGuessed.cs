using UnityEngine;
using TMPro;

public class Display : MonoBehaviour
{
    void Start()
    {
        string word = PlayerPrefs.GetString("lastWordGuessed", "No word guessed");

        GetComponent<TextMeshProUGUI>().text = "The word was: " + word;
    }
}