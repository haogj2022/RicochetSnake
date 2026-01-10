using UnityEngine;

public class Snake : MonoBehaviour
{
    [SerializeField] private float MoveSpeed = 10f;
    private Vector3 MoveDirection;
    private Obstacle[] Obstacles;
    private float BodyRadius;
    private bool CanMove = true;

    private void Start()
    {
        GameManager.Instance.OnGameOver += OnGameOver;
        Obstacles = FindObjectsOfType<Obstacle>();
        BodyRadius = Mathf.Max(transform.localScale.x, transform.localScale.y) / 2;
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnGameOver -= OnGameOver;
    }

    private void OnGameOver()
    {
        CanMove = false;
    }

    public void MoveTowardsDirection(Vector3 direction)
    {
        MoveDirection = direction.normalized;
        CanMove = true;
    }

    private void Update()
    {
        if (CanMove)
        {
            transform.position += MoveSpeed * Time.deltaTime * MoveDirection;
        }

        CheckCollision();
    }

    private void CheckCollision()
    {
        for (int i = 0; i < Obstacles.Length; i++)
        {
            if (Obstacles[i].IsOverlappedWith(transform.position, BodyRadius))
            {
                transform.position = Obstacles[i].GetCircleCenter();
                MoveDirection = Obstacles[i].GetReflectDirection(transform.position, BodyRadius, MoveDirection);

                float angle = Mathf.Atan2(MoveDirection.y, MoveDirection.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, 0, angle);
                GameManager.Instance.DecreaseBounceCount(1);
            }
        }
    }
}
