using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    int score = 0;
    int nbAttempts, maxNbAttempts;

    public GameObject letter;
    public GameObject cen;

    string wordToGuess = "";
    int lengthOfWordToGuess;

    char[] lettersToGuess;
    bool[] lettersGuessed;
    DifficultyManager difficultyManager;

    float timeRemaining;

    void Start()
    {
        cen = GameObject.Find("centerOfScreen");

        // Ensure DifficultyManager exists on this object so GetComponent doesn't fail
        difficultyManager = GetComponent<DifficultyManager>();
        if (difficultyManager == null)
        {
            difficultyManager = gameObject.AddComponent<DifficultyManager>();
        }

        nbAttempts = 0;

        InitGame();
        InitLetters();
        UpdateNbAttempts();
        UpdateScore();
        UpdatePlayerName();

        timeRemaining = difficultyManager.GetTimeLimit();
    }

    void Update()
    {
        CheckKeyboard2();
        UpdateTimer();
    }

    void InitLetters()
    {
        int nbLetters = lengthOfWordToGuess;

        for (int i = 0; i < nbLetters; i++)
        {
            Vector3 newPosition = new Vector3(
                cen.transform.position.x + ((i - nbLetters / 2.0f) * 100),
                cen.transform.position.y,
                cen.transform.position.z
            );

            GameObject l = Instantiate(letter, newPosition, Quaternion.identity);

            l.name = "letter" + (i + 1);
            l.transform.SetParent(GameObject.Find("Canvas").transform);
        }
    }

    void InitGame()
    {
        wordToGuess = PickAWordFromFile();

        wordToGuess = wordToGuess.Trim();
        wordToGuess = wordToGuess.ToUpper();

        maxNbAttempts = wordToGuess.Length * 2;

        lengthOfWordToGuess = wordToGuess.Length;

        lettersToGuess = wordToGuess.ToCharArray();
        lettersGuessed = new bool[lengthOfWordToGuess];
    }

    void CheckKeyboard2()
    {
        if (Input.inputString.Length > 0)
        {
            char letterPressed = Input.inputString[0];

            letterPressed = System.Char.ToUpper(letterPressed);

            nbAttempts++;
            UpdateNbAttempts();

            for (int i = 0; i < lengthOfWordToGuess; i++)
            {
                if (!lettersGuessed[i] && lettersToGuess[i] == letterPressed)
                {
                    lettersGuessed[i] = true;

                    GameObject.Find("letter" + (i + 1))
                        .GetComponent<TextMeshProUGUI>()
                        .text = letterPressed.ToString();
                }
            }

            CheckIfWordWasFound();

            if (nbAttempts >= maxNbAttempts)
            {
                // Save word so game over screen can display it
                PlayerPrefs.SetString("lastWordGuessed", wordToGuess);
                PlayerPrefs.Save();

                SceneManager.LoadScene("wordGameEnd");
            }
        }
    }

    void UpdateTimer()
    {
        timeRemaining -= Time.deltaTime;

        if (timeRemaining < 0)
        {
            timeRemaining = 0;
        }

        GameObject.Find("timerUI")
            .GetComponent<TextMeshProUGUI>()
            .text = Mathf.Ceil(timeRemaining).ToString();

        if (timeRemaining <= 0)
        {
            // Save word so game over screen can display it
            PlayerPrefs.SetString("lastWordGuessed", wordToGuess);
            PlayerPrefs.Save();

            SceneManager.LoadScene("wordGameEnd");
        }
    }

    void UpdateNbAttempts()
    {
        GameObject.Find("nbAttempts")
            .GetComponent<TextMeshProUGUI>()
            .text = nbAttempts + "/" + maxNbAttempts;
    }

    void UpdateScore()
    {
        GameObject.Find("scoreUI")
            .GetComponent<TextMeshProUGUI>()
            .text = "Score:" + score;
    }

    void CheckIfWordWasFound()
    {
        bool condition = true;

        for (int i = 0; i < lengthOfWordToGuess; i++)
        {
            condition = condition && lettersGuessed[i];
        }

        if (condition)
        {
            PlayerPrefs.SetString("lastWordGuessed", wordToGuess);
            PlayerPrefs.Save();

            score++;
            PlayerPrefs.SetInt("score", score);
            PlayerPrefs.Save();

            SceneManager.LoadScene("wordGameWin");
        }
    }

    string PickAWordFromFile()
    {
        TextAsset t1 = Resources.Load<TextAsset>("words");

        string s = t1.text;

        string[] words = s.Split('\n');

        int minLength = difficultyManager.GetMinWordLength();
        int maxLength = difficultyManager.GetMaxWordLength();

        System.Collections.Generic.List<string> validWords =
            new System.Collections.Generic.List<string>();

        foreach (string word in words)
        {
            string cleanWord = word.Trim();

            if (cleanWord.Length >= minLength &&
                cleanWord.Length <= maxLength)
            {
                validWords.Add(cleanWord);
            }
        }

        if (validWords.Count == 0)
        {
            Debug.LogError("No words found for this difficulty!");
            return "TEST";
        }

        int randomWord = Random.Range(0, validWords.Count);

        return validWords[randomWord];
    }
    void UpdatePlayerName()
    {
        GameObject nameObj = GameObject.Find("playerNameUI");

        if (nameObj != null)
        {
            TextMeshProUGUI nameText = nameObj.GetComponent<TextMeshProUGUI>();
            if (nameText != null)
            {
                string savedName = PlayerPrefs.GetString("playerName", "Player");
                nameText.text = savedName;
                Debug.Log("Displayed player name: " + savedName);
            }
            else
            {
                Debug.LogError("Found 'playerNameUI', but it is missing a TextMeshProUGUI component!");
            }
        }
        else
        {
            Debug.LogError("Could not find GameObject named 'playerNameUI' in the scene!");
        }
    }
}
