using System;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private TMP_Text BounceCountText;
    [SerializeField] private int MaxBounceCount = 5;
    [SerializeField] private GameObject Player;
    [SerializeField] private Vector3 PlayerStartPos = new(0, -2.5f, 0);
    [SerializeField] private float RecoverBounceCountChance = 0.1f;
    [SerializeField] private int AmmoCount = 3;
    public Action OnSnakeShot;
    public Action OnMoveComplete;
    public Action OnGameOver;
    private int CurrentBounceCount;

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

    private void Start()
    {
        ResetBounceCount();
        ResetPlayerPosition();
    }

    public void ResetBounceCount()
    {
        CurrentBounceCount = MaxBounceCount;
        BounceCountText.color = Color.white;
    }

    private void ResetPlayerPosition()
    {
        Player.transform.position = PlayerStartPos;
    }

    private void Update()
    {
        BounceCountText.text = CurrentBounceCount.ToString();
        BounceCountText.transform.position = Player.transform.position;
    }

    public void DecreaseBounceCount()
    {
        CurrentBounceCount--;

        if (CurrentBounceCount < 3)
        {
            BounceCountText.color = Color.red;

            if (CurrentBounceCount <= 0)
            {
                ResetPlayerPosition();
                ResetBounceCount();
                OnMoveComplete();
            }
        }
    }

    public void TryToIncreaseBounceCount()
    {
        float randomValue = UnityEngine.Random.value;

        if (randomValue < RecoverBounceCountChance)
        {
            Debug.Log("recover +1 bounce count from food");
            CurrentBounceCount++;
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
