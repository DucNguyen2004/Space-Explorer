using TMPro;
using UnityEngine;

public class ScoreView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;

    private void Start()
    {
        GameController.Instance.OnScoreUpdate += UpdateScore;
    }

    private void UpdateScore(int score)
    {
        scoreText.text = $"Score: {score}";
    }
}
