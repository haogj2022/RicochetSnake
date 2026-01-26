using System.Collections.Generic;
using UnityEngine;

public class SnakeSpawner : MonoBehaviour
{
    [SerializeField] private Snake PlayerPrefab;
    [SerializeField] private Vector3 PlayerSpawn = new(0, -2.5f, 0);
    private int AmmoCount;
    private List<Snake> Snakes = new();

    private void Start()
    {
        GameManager.Instance.OnZeroBounceCount += OnZeroBounceCount;
        SpawnNewPlayer();
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnZeroBounceCount -= OnZeroBounceCount;
    }

    private void OnZeroBounceCount()
    {
        Invoke(nameof(DespawnOldPlayer), 1);

        AmmoCount = GameManager.Instance.GetAmmoCount();

        if (AmmoCount > 0)
        {
            SpawnNewPlayer();
        }
        else
        {
            Snakes.Clear();
            GameManager.Instance.OnLevelFailed();
        }
    }

    private void SpawnNewPlayer()
    {
        Snake newSnake = Instantiate(PlayerPrefab, PlayerSpawn, Quaternion.identity, transform);
        Snakes.Add(newSnake);
    }

    private void DespawnOldPlayer()
    {
        if (Snakes.Count > 0)
        {
            Snakes[0].gameObject.SetActive(false);
            Snakes.RemoveAt(0);
        }
    }
}
