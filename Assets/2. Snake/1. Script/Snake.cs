using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    [SerializeField] private Transform SnakeTailPrefab;
    [SerializeField] private SnakeBody BodyPartPrefab;
    [SerializeField] private float MoveSpeed = 10f;
    [SerializeField] private float SnakeLength = 1f;
    private Vector2 MoveDirection;
    private bool CanMove;

    private List<Transform> SnakeTails = new();
    private List<SnakeBody> BodyParts = new();
    private List<Vector2> HeadPositions = new();
    private Vector2 LastHeadPosition;
    private Rigidbody2D PlayerBody;

    private void Start()
    {
        GameManager.Instance.OnMoveComplete += OnMoveComplete;
        GameManager.Instance.OnGameOver += OnGameOver;

        SpawnNewPart();
        LastHeadPosition = transform.position;
        PlayerBody = GetComponent<Rigidbody2D>();
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnMoveComplete -= OnMoveComplete;
        GameManager.Instance.OnGameOver -= OnGameOver;
    }

    private void OnMoveComplete()
    {
        CanMove = false;
    }

    private void OnGameOver()
    {
        CanMove = false;
    }

    public void MoveTowardsDirection(Vector2 direction)
    {
        MoveDirection = direction.normalized;
        CanMove = true;
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
        UpdateHeadPositions();
        UpdateSnakeTails();
        UpdateBodyParts();
    }

    #region Draw Snake
    private void UpdateHeadPositions()
    {
        if (Vector2.Distance(transform.position, LastHeadPosition) > SnakeLength)
        {
            LastHeadPosition = transform.position;
            HeadPositions.Add(LastHeadPosition);
        }

        if (HeadPositions.Count > 0)
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
        if (BodyParts.Count < 2)
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
        SpawnNewPart();

        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Vector2 normal = collision.contacts[0].normal;
            MoveDirection = Vector2.Reflect(MoveDirection, normal);
            float angle = Mathf.Atan2(MoveDirection.y, MoveDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
            GameManager.Instance.DecreaseBounceCount();
        }

        if (collision.gameObject.CompareTag("Finish"))
        {
            GameManager.Instance.ResetBounceCount();
            GameManager.Instance.OnMoveComplete();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Apple"))
        {
            collision.gameObject.SetActive(false);
            GameManager.Instance.TryToIncreaseBounceCount();
        }
    }

    private void SpawnNewPart()
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
}
