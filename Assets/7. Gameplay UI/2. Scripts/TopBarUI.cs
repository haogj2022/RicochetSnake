using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TopBarUI : MonoBehaviour
{
    [SerializeField] private Button PauseButton;
    [SerializeField] private TMP_Text CurrentLevelText;
    [SerializeField] private TMP_Text TotalGoldText;

    private void Start()
    {
        GameManager.Instance.OnFoodEaten += OnFoodEaten;
        PauseButton.onClick.AddListener(PauseLevel);
        CurrentLevelText.text = GameManager.Instance.GetCurrentLevel().ToString();
        TotalGoldText.text = GameManager.Instance.GetTotalGold().ToString();
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnFoodEaten -= OnFoodEaten;
        PauseButton.onClick.RemoveListener(PauseLevel);
    }

    private void PauseLevel()
    {
        Time.timeScale = 0;
    }

    private void OnFoodEaten()
    {
        TotalGoldText.text = GameManager.Instance.GetTotalGold().ToString();
    }
}
