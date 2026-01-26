using UnityEngine;
using UnityEngine.UI;

public class LostScreenUI : MonoBehaviour
{
    [SerializeField] private GameObject NoAmmoLeftPanel;
    [SerializeField] private Button RestartLevel;
    [SerializeField] private Button ReturnToMainMenu;

    private void Start()
    {
        GameManager.Instance.OnLevelFailed += OnLevelFailed;
        ReturnToMainMenu.onClick.AddListener(LoadMainMenu);
        RestartLevel.onClick.AddListener(ReloadLevel);
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnLevelFailed -= OnLevelFailed;
        ReturnToMainMenu.onClick.RemoveListener(LoadMainMenu);
        RestartLevel.onClick.RemoveListener(ReloadLevel);
    }

    private void OnLevelFailed()
    {
        NoAmmoLeftPanel.SetActive(true);
    }

    private void LoadMainMenu()
    {
        LevelLoader.Instance.LoadNextLevel("MainMenu");
    }

    private void ReloadLevel()
    {
        LevelLoader.Instance.LoadNextLevel("Gameplay");
    }
}
