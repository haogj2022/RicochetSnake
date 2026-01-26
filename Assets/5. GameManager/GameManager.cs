using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Action OnSnakeShot;
    public Action OnFoodEaten;
    public Action OnZeroBounceCount;
    public Action OnMoveCompleted;
    public Action OnLevelFailed;
    public Action OnLevelCompleted;
    public Action OnLevelPaused;
    public Action OnLevelUnpaused;
    public Action OnAmmoPurchased;
    public Action OnGoldDoubled;

    private int MaxAmmoCount = 3;
    private int CurrentAmmoCount;
    private int FoodAmount;
    private int BuyAmmoCount = 3;
    private int BuyAmmoStartCost = 50;
    private int BuyAmmoCurrentCost;

    private PlayerData Data;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        Data = PlayerDataManager.LoadPlayerData();

        if (Data == null)
        {
            Data = new PlayerData();
        }
        ResetLevel();
    }

    private void OnApplicationQuit()
    {
        PlayerDataManager.SavePlayerData(Data);
    }

    public void ResetLevel()
    {
        CurrentAmmoCount = MaxAmmoCount;
        BuyAmmoCurrentCost = BuyAmmoStartCost;
        FoodAmount = GameObject.FindGameObjectsWithTag("Food").Length;
    }

    public void BuyAmmo()
    {
        if (Data.TotalGold >= BuyAmmoCurrentCost)
        {
            Data.TotalGold -= BuyAmmoCurrentCost;
            CurrentAmmoCount += BuyAmmoCount;
            BuyAmmoCurrentCost *= 2;
            OnAmmoPurchased();
        }
    }

    public int GetBuyAmmoCurrentCost()
    {
        return BuyAmmoCurrentCost;
    }

    public void DecreaseAmmoCount()
    {
        CurrentAmmoCount--;
    }

    public void DecreaseFoodAmount()
    {
        FoodAmount--;

        if (FoodAmount <= 0)
        {
            FoodAmount = 0;
            OnLevelCompleted();
            Data.CurrentLevel++;
        }
    }

    public int GetAmmoCount()
    {
        return CurrentAmmoCount;
    }

    public void IncreaseGold(int amount)
    {
        Data.TotalGold += amount;
    }

    public int GetMaxBounce()
    {
        return Data.MaxBounce;
    }

    public float GetRecoveryRate()
    {
        return Data.RecoveryRate;
    }

    public int GetCurrentLevel()
    {
        return Data.CurrentLevel;
    }

    public int GetTotalGold()
    {
        return Data.TotalGold;
    }
}
