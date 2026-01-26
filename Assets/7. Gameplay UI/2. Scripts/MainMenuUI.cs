using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private TMP_Text CurrentLevelText;
    [SerializeField] private Button StartButton;

    private void Start()
    {
        StartButton.onClick.AddListener(LoadLevel);

        if (CurrentLevelText != null)
        {
            CurrentLevelText.text = $"Current Level: " + GameManager.Instance.GetCurrentLevel().ToString();
        }
    }

    private void OnDestroy()
    {
        StartButton.onClick.RemoveListener(LoadLevel);
    }

    private void LoadLevel()
    {
        LevelLoader.Instance.LoadNextLevel("Gameplay");
    }
}
