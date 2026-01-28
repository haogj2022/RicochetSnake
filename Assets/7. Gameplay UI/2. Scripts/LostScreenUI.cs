using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LostScreenUI : MonoBehaviour
{
    [SerializeField] private GameObject NoAmmoLeftPanel;
    [SerializeField] private GameObject BlurPanel;
    [SerializeField] private Button BuyAmmoButton;
    [SerializeField] private TMP_Text BuyAmmoText;
    [SerializeField] private Button ReturnToMainMenu;

    private void Start()
    {
        GameManager.Instance.OnLevelFailed += OnLevelFailed;
        BuyAmmoButton.onClick.AddListener(BuyAmmo);
        ReturnToMainMenu.onClick.AddListener(LoadMainMenu);
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnLevelFailed -= OnLevelFailed;
        BuyAmmoButton.onClick.RemoveListener(BuyAmmo);
        ReturnToMainMenu.onClick.RemoveListener(LoadMainMenu);
    }

    private void OnLevelFailed()
    {
        BlurPanel.SetActive(true);
        NoAmmoLeftPanel.transform.DOScale(1, 0.5f).SetEase(Ease.OutBounce);
        BuyAmmoText.text = $"Buy 3 Ammo for {GameManager.Instance.GetBuyAmmoCurrentCost()}";

        if (GameManager.Instance.GetTotalGold() < GameManager.Instance.GetBuyAmmoCurrentCost())
        {
            BuyAmmoButton.interactable = false;
            BuyAmmoText.color = Color.red;
        }
    }

    private void LoadMainMenu()
    {
        LevelLoader.Instance.LoadNextLevel("MainMenu");
    }

    private void BuyAmmo()
    {
        GameManager.Instance.BuyAmmo();
        BlurPanel.SetActive(false);
        NoAmmoLeftPanel.transform.DOScale(0, 0.5f).SetEase(Ease.OutBounce);
    }
}
