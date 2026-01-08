using UnityEngine;

public class SnakeShooter : MonoBehaviour
{
    [SerializeField] private Snake SnakeHead;
    private bool CanShoot = true;


    private void Start()
    {
        GameManager.Instance.OnMoveComplete += OnMoveComplete;
        GameManager.Instance.OnGameOver += OnGameOver;
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnMoveComplete -= OnMoveComplete;
        GameManager.Instance.OnGameOver -= OnGameOver;
    }

    void OnMoveComplete()
    {
        CanShoot = true;
    }

    void OnGameOver()
    {
        CanShoot = false;
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0) && CanShoot)
        {
            MoveSnake();
        }
    }

    private void MoveSnake()
    {
        SnakeHead.MoveTowardsDirection(transform.right);
        CanShoot = false;
    }
}
