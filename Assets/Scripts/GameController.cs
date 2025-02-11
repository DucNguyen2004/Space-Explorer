using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    private static GameController _instance;
    public int score = 0;
    public static GameController Instance => _instance ??= FindAnyObjectByType<GameController>();

    public event Action<int> OnScoreUpdate;
    public event Action OnSpeedUp;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Gameplay");
    }
    public void EndGame()
    {
        SceneManager.LoadScene("EndGameScene");
    }
    public void BackMainMenu()
    {
        score = 0;
        SceneManager.LoadScene("MainMenu");
    }
    public void QuitGame()
    {
        UnityEditor.EditorApplication.isPlaying = false;
    }

    public void AddScore(int amount)
    {
        score += amount;

        OnScoreUpdate?.Invoke(score);

        if (score % 10 == 0)
        {
            SpeedUpGame();
        }
    }

    void SpeedUpGame()
    {
        OnSpeedUp?.Invoke();
    }
}
