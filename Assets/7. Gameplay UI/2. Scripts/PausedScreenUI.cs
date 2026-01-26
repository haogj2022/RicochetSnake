using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PausedScreenUI : MonoBehaviour
{
    [SerializeField] private GameObject PausedPanel;
    [SerializeField] private Button ContinueLevel;
    [SerializeField] private Button ReturnToMainMenu;

    private void Start()
    {
        GameManager.Instance.OnLevelPaused += OnLevelPaused;
        ContinueLevel.onClick.AddListener(UnpauseLevel);
        ReturnToMainMenu.onClick.AddListener(LoadMainMenu);
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnLevelPaused -= OnLevelPaused;
        ContinueLevel.onClick.RemoveListener(UnpauseLevel);
        ReturnToMainMenu.onClick.RemoveListener(LoadMainMenu);
    }

    private void OnLevelPaused()
    {
        PausedPanel.transform.DOScale(1, 0.5f).SetEase(Ease.OutBounce);
    }

    private void UnpauseLevel()
    {
        PausedPanel.transform.DOScale(0, 0.5f).SetEase(Ease.InBounce);
        GameManager.Instance.OnLevelUnpaused();
    }

    private void LoadMainMenu()
    {
        LevelLoader.Instance.LoadNextLevel("MainMenu");
    }
}
