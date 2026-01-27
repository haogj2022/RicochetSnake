using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Snake : MonoBehaviour
{
    #region MoveSnakeHead
    [SerializeField] private float MoveSpeed = 10f;
    private Vector2 MoveDirection;
    private Rigidbody2D PlayerBody;
    private bool CanMove;

    private void Start()
    {
        GameManager.Instance.OnSnakeShot += OnSnakeShot;
        GameManager.Instance.OnLevelCompleted += OnLevelCompleted;
        GameManager.Instance.OnLevelPaused += OnLevelPaused;
        GameManager.Instance.OnLevelUnpaused += OnLevelUnpaused;

        PlayerBody = GetComponent<Rigidbody2D>();
        ResetSnake();
    }

    private void OnEnable()
    {
        if (DeadStatus.activeSelf)
        {
            ResetSnake();
            GameManager.Instance.OnMoveCompleted();
        }
    }

    private void OnDisable()
    {
        DespawnOldParts();
    }

    private void ResetSnake()
    {
        CanMove = false;
        AliveStatus.SetActive(true);
        DeadStatus.SetActive(false);
        SpawnNewParts();
        MaxBounce = GameManager.Instance.GetMaxBounce();
        RecoveryRate = GameManager.Instance.GetRecoveryRate();
        CurrentBounceCount = MaxBounce;
        BounceCountText.color = Color.white;
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnSnakeShot -= OnSnakeShot;
        GameManager.Instance.OnLevelCompleted -= OnLevelCompleted;
        GameManager.Instance.OnLevelPaused -= OnLevelPaused;
        GameManager.Instance.OnLevelUnpaused -= OnLevelUnpaused;
    }

    private void OnSnakeShot()
    {
        MoveDirection = transform.right;
        CanMove = true;
    }

    private void OnLevelPaused()
    {
        CanMove = false;
    }

    private void OnLevelUnpaused()
    {
        CanMove = true;
    }

    private void OnLevelCompleted()
    {
        CanMove = false;
    }

    private void FixedUpdate()
    {
        if (CanMove)
        {
            PlayerBody.velocity = MoveDirection * MoveSpeed;
        }
        else
        {
            PlayerBody.velocity = Vector2.zero;
        }
    }
    #endregion MoveSnakeHead

    #region DrawSnake
    [SerializeField] private Transform SnakeTailPrefab;
    [SerializeField] private SnakeBody BodyPartPrefab;
    [SerializeField] private float SnakeLength = 1f;
    private List<Transform> SnakeTails = new();
    private List<SnakeBody> BodyParts = new();
    private List<Vector2> HeadPositions = new();
    private Vector2 LastHeadPosition;

    private void Update()
    {
        UpdateBounceCountText();
        UpdateHeadPositions();
        UpdateSnakeTails();
        UpdateBodyParts();
    }

    private void UpdateHeadPositions()
    {
        if (Vector2.Distance(transform.position, LastHeadPosition) > SnakeLength)
        {
            LastHeadPosition = transform.position;
            HeadPositions.Add(LastHeadPosition);
        }

        if (SnakeTails.Count > 0 && HeadPositions.Count > 0)
        {
            SnakeTails[0].position = Vector2.MoveTowards(
                SnakeTails[0].position, HeadPositions[0], MoveSpeed * Time.deltaTime);

            if (Vector2.Distance(SnakeTails[0].position, HeadPositions[0]) < 0.1f)
            {
                SnakeTails[0].position = HeadPositions[0];
                HeadPositions.RemoveAt(0);
            }
        }
    }

    private void UpdateSnakeTails()
    {
        if (SnakeTails.Count > 1)
        {
            if (SnakeTails[0].position == SnakeTails[1].position)
            {
                PoolingSystem.Despawn(SnakeTailPrefab.gameObject, SnakeTails[1].gameObject);
                SnakeTails.RemoveAt(1);

                PoolingSystem.Despawn(BodyPartPrefab.gameObject, BodyParts[1].gameObject);
                BodyParts.RemoveAt(1);
            }
        }
    }

    private void UpdateBodyParts()
    {
        if (BodyParts.Count == 1)
        {
            BodyParts[0].SetPosition(SnakeTails[0].position, transform.position);
        }

        for (int i = 1; i < BodyParts.Count; i++)
        {
            BodyParts[i - 1].SetPosition(SnakeTails[i - 1].position, SnakeTails[i].position);

            if (i == BodyParts.Count - 1)
            {
                BodyParts[i].SetPosition(SnakeTails[i].position, transform.position);
            }
        }
    }

    private void SpawnNewParts()
    {
        Transform newTail = PoolingSystem.Spawn<Transform>(
            SnakeTailPrefab.gameObject,
            transform.parent,
            SnakeTailPrefab.transform.localScale,
            transform.position,
            Quaternion.identity);
        SnakeTails.Add(newTail);

        SnakeBody newBodyPart = PoolingSystem.Spawn<SnakeBody>(
            BodyPartPrefab.gameObject,
            transform.parent,
            BodyPartPrefab.transform.localScale,
            transform.position,
            Quaternion.identity);
        BodyParts.Add(newBodyPart);
    }

    private void DespawnOldParts()
    {
        for (int i = 0; i < SnakeTails.Count; i++)
        {
            PoolingSystem.Despawn(SnakeTailPrefab.gameObject, SnakeTails[i].gameObject);
        }

        for (int i = 0; i < BodyParts.Count; i++)
        {
            PoolingSystem.Despawn(BodyPartPrefab.gameObject, BodyParts[i].gameObject);
        }

        SnakeTails.Clear();
        BodyParts.Clear();
    }
    #endregion DrawSnake

    #region HandleCollision
    [SerializeField] private GameObject AliveStatus;
    [SerializeField] private GameObject DeadStatus;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        LastHeadPosition = transform.position;
        HeadPositions.Add(transform.position);
        SpawnNewParts();

        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Vector2 normal = collision.contacts[0].normal;
            MoveDirection = Vector2.Reflect(MoveDirection, normal);
            float angle = Mathf.Atan2(MoveDirection.y, MoveDirection.x) * Mathf.Rad2Deg;

            if (CurrentBounceCount > 0)
            {
                transform.rotation = Quaternion.Euler(0, 0, angle);
            }
        }

        if (collision.gameObject.CompareTag("Finish"))
        {
            CanMove = false;
            ResetBounceCount();

            if (GameManager.Instance.GetAmmoCount() > 0)
            {
                GameManager.Instance.OnMoveCompleted();
            }
            else
            {
                DespawnSelf();
            }
        }

        if (collision.gameObject.CompareTag("Food"))
        {
            IncreaseBounceCount();
        }
    }

    private void DespawnSelf()
    {
        AliveStatus.SetActive(false);
        DeadStatus.SetActive(true);
        GameManager.Instance.OnZeroBounceCount();
    }
    #endregion HandleCollision

    #region UpdateBounceCount
    [SerializeField] private TMP_Text BounceCountText;
    private int MaxBounce;
    private float RecoveryRate;
    private int CurrentBounceCount;
    private int MinBounceCount = 3;

    public int GetCurrentBounceCount()
    {
        return CurrentBounceCount;
    }

    private void UpdateBounceCountText()
    {
        BounceCountText.text = CurrentBounceCount.ToString();
        BounceCountText.transform.rotation = Quaternion.identity;
    }

    private void IncreaseBounceCount()
    {
        float randomValue = Random.value;
        float recoverChance = RecoveryRate / 100;

        if (recoverChance < randomValue)
        {
            CurrentBounceCount++;
        }
    }

    public void DecreaseBounceCount(int amount)
    {
        CurrentBounceCount -= amount;

        if (CurrentBounceCount < MinBounceCount)
        {
            BounceCountText.color = Color.red;

            if (CurrentBounceCount <= 0)
            {
                CurrentBounceCount = 0;
                CanMove = false;
                DespawnSelf();
            }
        }
    }

    private void ResetBounceCount()
    {
        CurrentBounceCount = MaxBounce;
        BounceCountText.color = Color.white;
    }
    #endregion UpdateBounceCount
}
