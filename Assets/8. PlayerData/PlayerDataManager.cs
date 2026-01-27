using Newtonsoft.Json;
using System.IO;
using UnityEngine;

public static class PlayerDataManager
{
    public static void SavePlayerData(PlayerData data)
    {
        string fileData = JsonConvert.SerializeObject(data, Formatting.Indented);
        string filePath = Path.Combine(Application.dataPath, "Resources", "PlayerData.json");
        File.WriteAllText(filePath, fileData);
        Debug.Log($"Player data has been saved to {filePath}");
    }

    public static PlayerData LoadPlayerData()
    {
        string filePath = Path.Combine(Application.dataPath, "Resources", "PlayerData.json");

        if (File.Exists(filePath) == false)
        {
            Debug.LogWarning($"Player data was not found. Create new player data");
            return new PlayerData();
        }

        string fileData = File.ReadAllText(filePath);
        PlayerData data = JsonConvert.DeserializeObject<PlayerData>(fileData);
        Debug.Log($"Player data has been loaded from {filePath}");
        return data;
    }
}

public class PlayerData
{
    public int CurrentLevel;
    public int TotalGold;
    public int MaxBounce;
    public float RecoveryRate;
    public float MagnetRange;
    public float FreeBounce;

    public PlayerData()
    {
        CurrentLevel = 1;
        TotalGold = 0;
        MaxBounce = 5;
        RecoveryRate = 0;
        MagnetRange = 0;
        FreeBounce = 0;
    }
}
