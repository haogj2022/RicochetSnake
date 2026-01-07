using System;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private TMP_Text BounceCountText;
    [SerializeField] private int BounceCount = 12;
    [SerializeField] private GameObject Player;
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

    private void Start()
    {
        OnMoveComplete += CheckBounceCount;
    }

    private void OnDestroy()
    {
        OnMoveComplete -= CheckBounceCount;
    }

    private void CheckBounceCount()
    {
        if (BounceCount <= 0)
        {
            OnGameOver();
        }
    }

    private void Update()
    {
        BounceCountText.text = BounceCount.ToString();
        BounceCountText.transform.position = Player.transform.position;
    }

    public void DecreaseBounceCount(int amount)
    {
        BounceCount -= amount;

        if (BounceCount < 4)
        {
            BounceCountText.color = Color.red;

            if (BounceCount <= 0)
            {
                BounceCount = 0;
                OnGameOver();
            }
        }
    }
}
