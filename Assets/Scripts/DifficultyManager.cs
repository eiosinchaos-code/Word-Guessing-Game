using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    public enum Difficulty
    {
        Easy,
        Medium,
        Hard
    }

    public Difficulty currentDifficulty;

    void Awake()
    {
        LoadDifficulty();
    }

    public void SetDifficulty(int difficultyIndex)
    {
        currentDifficulty = (Difficulty)difficultyIndex;
        PlayerPrefs.SetInt("difficulty", difficultyIndex);
        PlayerPrefs.Save();
    }

    public void LoadDifficulty()
    {
        int savedDifficulty = PlayerPrefs.GetInt("difficulty", 0);
        currentDifficulty = (Difficulty)savedDifficulty;
    }

    public int GetMinWordLength()
    {
        return currentDifficulty switch
        {
            Difficulty.Easy => 4,
            Difficulty.Medium => 6,
            Difficulty.Hard => 9,
            _ => 4
        };
    }

    public int GetMaxWordLength()
    {
        return currentDifficulty switch
        {
            Difficulty.Easy => 5,
            Difficulty.Medium => 8,
            Difficulty.Hard => 999,
            _ => 5
        };
    }

    public float GetTimeLimit()
    {
        return currentDifficulty switch
        {
            Difficulty.Easy => 150f,
            Difficulty.Medium => 120f,
            Difficulty.Hard => 60f,
            _ => 60f
        };
    }
}