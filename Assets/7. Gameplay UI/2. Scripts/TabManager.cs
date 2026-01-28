using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TabManager : MonoBehaviour
{
    [SerializeField] private Image UpgradesIcon;
    [SerializeField] private Image HomeIcon;
    [SerializeField] private Image SettingsIcon;
    [SerializeField] private TMP_Text UpgradesText;
    [SerializeField] private TMP_Text HomeText;
    [SerializeField] private TMP_Text SettingsText;
    [SerializeField] private Button UpgradesButton;
    [SerializeField] private Button HomeButton;
    [SerializeField] private Button SettingsButton;
    [SerializeField] private LayoutElement UpgradesLayout;
    [SerializeField] private LayoutElement HomeLayout;
    [SerializeField] private LayoutElement SettingsLayout;
    private Color TransparentColor = new Color(1, 1, 1, 0.1f);
    private Vector3 UpgradesIconStartPos;
    private Vector3 HomeIconStartPos;
    private Vector3 SettingsIconStartPos;

    private void Start()
    {
        UpgradesButton.onClick.AddListener(OpenUpgrades);
        HomeButton.onClick.AddListener(OpenHome);
        SettingsButton.onClick.AddListener(OpenSettings);

        UpgradesIconStartPos = UpgradesIcon.transform.localPosition;
        HomeIconStartPos = HomeIcon.transform.localPosition;
        SettingsIconStartPos = SettingsIcon.transform.localPosition;
    }

    private void OnDestroy()
    {
        UpgradesButton.onClick.RemoveListener(OpenUpgrades);
        HomeButton.onClick.RemoveListener(OpenHome);
        SettingsButton.onClick.RemoveListener(OpenSettings);
    }

    private void OpenUpgrades()
    {
        UpgradesIcon.transform.DOLocalJump(UpgradesIconStartPos, 20, 1, 0.5f);

        UpgradesLayout.flexibleWidth = 1.5f;
        HomeLayout.flexibleWidth = 1;
        SettingsLayout.flexibleWidth = 1;

        UpgradesIcon.color = Color.white;
        HomeIcon.color = TransparentColor;
        SettingsIcon.color = TransparentColor;

        UpgradesText.text = "Upgrades";
        HomeText.text = string.Empty;
        SettingsText.text = string.Empty;
    }

    private void OpenHome()
    {
        HomeIcon.transform.DOLocalJump(HomeIconStartPos, 20, 1, 0.5f);

        UpgradesLayout.flexibleWidth = 1;
        HomeLayout.flexibleWidth = 1.5f;
        SettingsLayout.flexibleWidth = 1;

        UpgradesIcon.color = TransparentColor;
        HomeIcon.color = Color.white;
        SettingsIcon.color = TransparentColor;

        UpgradesText.text = string.Empty;
        HomeText.text = "Home";
        SettingsText.text = string.Empty;
    }

    private void OpenSettings()
    {
        SettingsIcon.transform.DOLocalJump(SettingsIconStartPos, 20, 1, 0.5f);

        UpgradesLayout.flexibleWidth = 1;
        HomeLayout.flexibleWidth = 1;
        SettingsLayout.flexibleWidth = 1.5f;

        UpgradesIcon.color = TransparentColor;
        HomeIcon.color = TransparentColor;
        SettingsIcon.color = Color.white;

        UpgradesText.text = string.Empty;
        HomeText.text = string.Empty;
        SettingsText.text = "Settings";
    }
}
