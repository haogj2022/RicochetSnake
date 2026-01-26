using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VictoryScreenUI : MonoBehaviour
{
    [SerializeField] private GameObject LevelCompletePanel;
    [SerializeField] private Button DoubleGoldButton;
    [SerializeField] private Button ContinueButton;
    [SerializeField] private TMP_Text AppleRewardText;
    [SerializeField] private TMP_Text GoldAppleRewardText;
    [SerializeField] private TMP_Text AmmoRewardText;
    [SerializeField] private TMP_Text BounceRewardText;
    [SerializeField] private TMP_Text LevelBonusRewardText;
    [SerializeField] private TMP_Text TotalRewardText;
    private Snake Player;
    private Food[] AllFood;

    private int BaseAppleReward = 10;
    private int BaseGoldAppleReward = 100;
    private int BaseAmmoReward = 50;
    private int BaseBounceReward = 5;
    private int BaseLevelBonusReward = 50;

    private int AppleCount;
    private int GoldAppleCount;
    private int AmmoCount;
    private int BounceCount;
    private int CurrentLevel;

    private int AppleReward;
    private int GoldAppleReward;
    private int AmmoReward;
    private int BounceReward;
    private int LevelBonusReward;
    private int TotalReward;

    private void Start()
    {
        GameManager.Instance.OnLevelCompleted += OnLevelCompleted;
        DoubleGoldButton.onClick.AddListener(DoubleGold);
        ContinueButton.onClick.AddListener(LoadNextLevel);
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnLevelCompleted -= OnLevelCompleted;
        DoubleGoldButton.onClick.RemoveListener(DoubleGold);
        ContinueButton.onClick.RemoveListener(LoadNextLevel);
    }

    private void OnLevelCompleted()
    {
        SetStats();
        LevelCompletePanel.transform.DOScale(1, 0.5f).SetEase(Ease.OutBounce);
    }

    private void SetStats()
    {
        Player = FindObjectOfType<Snake>();
        BounceCount = Player.GetCurrentBounceCount();
        AmmoCount = GameManager.Instance.GetAmmoCount();
        CurrentLevel = GameManager.Instance.GetCurrentLevel();

        AllFood = FindObjectsByType<Food>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        for (int i = 0; i < AllFood.Length; i++)
        {
            if (AllFood[i].GetGoldReward() == BaseAppleReward)
            {
                AppleCount++;
            }

            if (AllFood[i].GetGoldReward() == BaseGoldAppleReward)
            {
                GoldAppleCount++;
            }
        }

        AppleReward = BaseAppleReward * AppleCount;
        GoldAppleReward = BaseGoldAppleReward * GoldAppleCount;
        AmmoReward = BaseAmmoReward * AmmoCount;
        BounceReward = BaseBounceReward * BounceCount;
        LevelBonusReward = BaseLevelBonusReward + (CurrentLevel * AmmoCount);
        TotalReward = AppleReward + GoldAppleReward + AmmoReward + BounceReward + LevelBonusReward;
        GameManager.Instance.IncreaseGold(TotalReward);

        AppleRewardText.text = $"{BaseAppleReward} x {AppleCount} = {AppleReward}";
        GoldAppleRewardText.text = $"{BaseGoldAppleReward} x {GoldAppleCount} = {GoldAppleReward}";
        AmmoRewardText.text = $"{BaseAmmoReward} x {AmmoCount} = {AmmoReward}";
        BounceRewardText.text = $"{BaseBounceReward} x {BounceCount} = {BounceReward}";
        LevelBonusRewardText.text = $"{BaseLevelBonusReward} + ({CurrentLevel} x {AmmoCount}) = {LevelBonusReward}";
        TotalRewardText.text = TotalReward.ToString();
    }

    private void DoubleGold()
    {
        DoubleGoldButton.interactable = false;
        TotalRewardText.text = $"{TotalReward} x 2 = {TotalReward * 2}";
        GameManager.Instance.IncreaseGold(TotalReward);
        GameManager.Instance.OnGoldDoubled();
    }

    private void LoadNextLevel()
    {
        LevelLoader.Instance.LoadNextLevel("Gameplay");
    }
}
