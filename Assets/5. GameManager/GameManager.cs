using System;
using System.Xml;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public PlayerData Data;
    [SerializeField] private int MaxAmmoCount = 3;
    public Action<Vector2> OnSnakeShot;
    public Action OnZeroBounceCount;
    public Action OnMoveCompleted;
    public Action OnLevelFailed;
    public Action OnLevelCompleted;
    private int CurrentAmmoCount;
    private int FoodAmount;

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
    }

    private void OnDestroy()
    {
        PlayerDataManager.SavePlayerData(Data);
    }

    private void Start()
    {
        CurrentAmmoCount = MaxAmmoCount;
        FoodAmount = GameObject.FindGameObjectsWithTag("Apple").Length + 
            GameObject.FindGameObjectsWithTag("Gold Apple").Length;
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
            Debug.Log("Level Completed");
        }
    }

    public int GetAmmoCount()
    {
        return CurrentAmmoCount;
    }
}
