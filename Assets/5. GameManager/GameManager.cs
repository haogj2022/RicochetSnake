using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private int MaxAmmoCount = 3;
    public Action OnSnakeShot;
    public Action OnFoodEaten;
    public Action OnZeroBounceCount;
    public Action OnMoveCompleted;
    public Action OnLevelFailed;
    public Action OnLevelCompleted;
    private int CurrentAmmoCount;
    private int FoodAmount;
    private int CurrentLevelGold;
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
    }

    private void OnApplicationQuit()
    {
        PlayerDataManager.SavePlayerData(Data);
    }

    public void ResetLevel()
    {
        CurrentAmmoCount = MaxAmmoCount;
        FoodAmount = GameObject.FindGameObjectsWithTag("Food").Length;
        CurrentLevelGold = 0;
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
        CurrentLevelGold += amount;
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
