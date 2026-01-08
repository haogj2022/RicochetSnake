using UnityEngine;

public class Snake : MonoBehaviour
{
    [SerializeField] private float MoveSpeed = 10f;
    private Vector2 MoveDirection;
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
        MoveDirection = Vector2.zero;
    }

    public void MoveTowardsDirection(Vector2 direction)
    {
        MoveDirection = direction.normalized;
    }

    private void Update()
    {
        if (MoveDirection != Vector2.zero)
        {
            transform.Translate(MoveSpeed * Time.deltaTime * MoveDirection);
        }

        CheckCollision();
    }

    private void CheckCollision()
    {
        for (int i = 0; i < Obstacles.Length; i++)
        {
            if (Obstacles[i].CalculatedDistance(transform.position, BodyRadius) <= 0)
            {
                Debug.Log("Collided with " + Obstacles[i].name);
                MoveDirection = Vector2.zero;
            }
        }
    }
}
