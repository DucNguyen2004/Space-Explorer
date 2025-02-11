using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndGameView : MonoBehaviour
{
    [SerializeField] private Button mainMenuBtn;
    [SerializeField] private Button exitMenuBtn;
    [SerializeField] private TextMeshProUGUI scorePoint;

    private void Start()
    {
        mainMenuBtn.onClick.AddListener(GameController.Instance.BackMainMenu);
        exitMenuBtn.onClick.AddListener(GameController.Instance.QuitGame);

        scorePoint.text = $"Your score: {GameController.Instance.score}";
    }

    private void OnDisable()
    {
        mainMenuBtn.onClick.RemoveAllListeners();
        exitMenuBtn.onClick.RemoveAllListeners();
    }
}

