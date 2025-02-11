using UnityEngine;
using UnityEngine.UI;


public class MainMenuView : MonoBehaviour
{
    [SerializeField] private Button startBtn;
    [SerializeField] private Button instructionBtn;

    private void Start()
    {
        startBtn.onClick.AddListener(GameController.Instance.StartGame);
    }

    private void OnDestroy()
    {
        startBtn.onClick.RemoveAllListeners();
    }
}

