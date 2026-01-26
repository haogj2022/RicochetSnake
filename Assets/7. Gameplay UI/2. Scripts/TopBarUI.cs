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
        GameManager.Instance.OnFoodEaten += UpdateTotalGold;
        GameManager.Instance.OnLevelUnpaused += EnablePauseButton;
        GameManager.Instance.OnLevelCompleted += DisablePauseButton;
        GameManager.Instance.OnLevelFailed += DisablePauseButton;
        GameManager.Instance.OnAmmoPurchased += UpdateTotalGold;
        GameManager.Instance.OnGoldDoubled += UpdateTotalGold;

        PauseButton.onClick.AddListener(PauseLevel);
        CurrentLevelText.text = GameManager.Instance.GetCurrentLevel().ToString();
        TotalGoldText.text = GameManager.Instance.GetTotalGold().ToString();
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnFoodEaten -= UpdateTotalGold;
        GameManager.Instance.OnLevelUnpaused -= EnablePauseButton;
        GameManager.Instance.OnLevelCompleted -= DisablePauseButton;
        GameManager.Instance.OnLevelFailed -= DisablePauseButton;
        GameManager.Instance.OnAmmoPurchased -= UpdateTotalGold;
        GameManager.Instance.OnGoldDoubled -= UpdateTotalGold;

        PauseButton.onClick.RemoveListener(PauseLevel);
    }

    private void DisablePauseButton()
    {
        PauseButton.interactable = false;
    }

    private void UpdateTotalGold()
    {
        TotalGoldText.text = GameManager.Instance.GetTotalGold().ToString();
    }

    private void PauseLevel()
    {
        GameManager.Instance.OnLevelPaused();
        PauseButton.interactable = false;
    }

    private void EnablePauseButton()
    {
        PauseButton.interactable = true;
    }
}
