using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Snake : MonoBehaviour
{
    [SerializeField] private Transform SnakeTailPrefab;
    [SerializeField] private SnakeBody BodyPartPrefab;
    [SerializeField] private float MoveSpeed = 10f;
    [SerializeField] private float SnakeLength = 1f;
    [SerializeField] private TMP_Text BounceCountText;
    [SerializeField] private int MaxBounceCount = 5;
    [SerializeField] private float RecoverBounceCountPercentage = 10;
    [SerializeField] private GameObject AliveStatus;
    [SerializeField] private GameObject DeadStatus;
    private Vector2 MoveDirection;
    private bool CanMove;
    private int CurrentBounceCount;
    private int MinBounceCount = 3;

    private List<Transform> SnakeTails = new();
    private List<SnakeBody> BodyParts = new();
    private List<Vector2> HeadPositions = new();
    private Vector2 LastHeadPosition;
    private Rigidbody2D PlayerBody;

    private void Start()
    {
        GameManager.Instance.OnSnakeShot += OnSnakeShot;
        GameManager.Instance.OnLevelCompleted += OnLevelCompleted;

        SpawnNewParts();
        LastHeadPosition = transform.position;
        PlayerBody = GetComponent<Rigidbody2D>();
        CurrentBounceCount = MaxBounceCount;
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnSnakeShot -= OnSnakeShot;
        GameManager.Instance.OnLevelCompleted -= OnLevelCompleted;
    }

    private void OnSnakeShot(Vector2 moveDirection)
    {
        MoveDirection = moveDirection.normalized;
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

    private void Update()
    {
        UpdateBounceCountText();
        UpdateHeadPositions();
        UpdateSnakeTails();
        UpdateBodyParts();
    }

    private void UpdateBounceCountText()
    {
        BounceCountText.text = CurrentBounceCount.ToString();
        BounceCountText.transform.rotation = Quaternion.identity;
    }

    #region Draw Snake
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
        if (BodyParts.Count > 0 && BodyParts.Count < 2)
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
    #endregion Draw Snake

    private void OnCollisionEnter2D(Collision2D collision)
    {
        LastHeadPosition = transform.position;
        HeadPositions.Add(transform.position);
        SpawnNewParts();

        if (collision.gameObject.CompareTag("Obstacle"))
        {
            DecreaseBounceCount(1);
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
            GameManager.Instance.OnMoveCompleted();

            if (GameManager.Instance.GetAmmoCount() > 0)
            {
                ResetBounceCount();
            }
            else
            {
                DespawnSelf();
            }
        }

        if (collision.gameObject.CompareTag("Apple"))
        {
            collision.gameObject.SetActive(false);
            IncreaseBounceCount();
            GameManager.Instance.DecreaseFoodAmount();
        }
    }

    private void IncreaseBounceCount()
    {
        float randomValue = Random.value;
        float recoverChance = RecoverBounceCountPercentage / 100;

        if (recoverChance < randomValue)
        {
            CurrentBounceCount++;
        }
    }

    private void DecreaseBounceCount(int amount)
    {
        CurrentBounceCount -= amount;

        if (CurrentBounceCount < MinBounceCount)
        {
            BounceCountText.color = Color.red;

            if (CurrentBounceCount <= 0)
            {
                CurrentBounceCount = 0;
                DespawnSelf();
            }
        }
    }

    private void ResetBounceCount()
    {
        CurrentBounceCount = MaxBounceCount;
        BounceCountText.color = Color.white;
    }

    private void SpawnNewParts()
    {
        Transform newTail = PoolingSystem.Spawn<Transform>(
            SnakeTailPrefab.gameObject,
            transform.parent,
            transform.localScale,
            transform.position,
            Quaternion.identity);
        SnakeTails.Add(newTail);

        SnakeBody newBodyPart = PoolingSystem.Spawn<SnakeBody>(
            BodyPartPrefab.gameObject,
            transform.parent,
            transform.localScale,
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

    private void DespawnSelf()
    {
        CanMove = false;
        AliveStatus.SetActive(false);
        DeadStatus.SetActive(true);
        DespawnOldParts();
        GameManager.Instance.OnZeroBounceCount();
    }
}
