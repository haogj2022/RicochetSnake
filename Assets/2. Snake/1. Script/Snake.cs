using UnityEngine;

public class Snake : MonoBehaviour
{
    [SerializeField] private float MoveSpeed = 10f;
    private Vector3 MoveDirection;
    private Obstacle[] Obstacles;
    private float BodyRadius;

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
        MoveDirection = Vector3.zero;
    }

    public void MoveTowardsDirection(Vector3 direction)
    {
        MoveDirection = direction.normalized;
    }

    private void Update()
    {
        if (MoveDirection != Vector3.zero)
        {
            transform.position += MoveSpeed * Time.deltaTime * MoveDirection;
        }

        if (Obstacles.Length > 0)
        {
            CheckCollision();
        }
    }

    private void CheckCollision()
    {
        for (int i = 0; i < Obstacles.Length; i++)
        {
            if (Obstacles[i].IsCollidedWith(transform.position, BodyRadius))
            {
                Debug.Log("Collided with Obstacle: " + Obstacles[i].name);
                MoveDirection = Obstacles[i].GetNewDirection(transform.position, BodyRadius, MoveDirection);

                float angle = Mathf.Atan2(MoveDirection.y, MoveDirection.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, 0, angle);
            }
        }
    }
}
