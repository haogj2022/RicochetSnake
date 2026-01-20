using System;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private TMP_Text BounceCountText;
    [SerializeField] private int BounceCount = 12;
    [SerializeField] private GameObject Player;
    [SerializeField] private float RecoverBounceCountChance = 0.1f;
    [SerializeField] private int AmmoCount = 3;
    public Action OnSnakeShot;
    public Action OnMoveComplete;
    public Action OnGameOver;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        BounceCountText.text = BounceCount.ToString();
        BounceCountText.transform.position = Player.transform.position;
    }

    public void DecreaseBounceCount()
    {
        BounceCount--;

        if (BounceCount < 3)
        {
            BounceCountText.color = Color.red;

            if (BounceCount <= 0)
            {
                BounceCount = 0;
                OnGameOver();
            }
        }
    }

    public void TryToIncreaseBounceCount()
    {
        float randomValue = UnityEngine.Random.value;

        if (randomValue < RecoverBounceCountChance)
        {
            Debug.Log("recover +1 bounce count from food");
            BounceCount++;
        }
        else
        {
            Debug.Log("no bounce count recover from food");
        }
    }

    public void DecreaseAmmoCount()
    {
        AmmoCount--;
    }

    public int GetAmmoCount()
    {
        return AmmoCount;
    }
}
