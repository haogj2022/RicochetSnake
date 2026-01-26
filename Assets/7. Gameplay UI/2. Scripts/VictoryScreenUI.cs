using UnityEngine;
using UnityEngine.UI;

public class VictoryScreenUI : MonoBehaviour
{
    [SerializeField] private GameObject LevelCompletePanel;
    [SerializeField] private Button ContinueButton;

    private void Start()
    {
        GameManager.Instance.OnLevelCompleted += OnLevelCompleted;
        ContinueButton.onClick.AddListener(LoadMainMenu);
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnLevelCompleted -= OnLevelCompleted;
        ContinueButton.onClick.RemoveListener(LoadMainMenu);
    }

    private void OnLevelCompleted()
    {
        LevelCompletePanel.SetActive(true);
    }

    private void LoadMainMenu()
    {
        LevelLoader.Instance.LoadNextLevel("MainMenu");
    }
}
