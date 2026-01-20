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
        if (GameManager.Instance.GetAmmoCount() > 0)
        {
            CanShoot = true;
        }
        else
        {
            GameManager.Instance.OnGameOver();
        }
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
            GameManager.Instance.DecreaseAmmoCount();
        }
    }

    private void MoveSnake()
    {
        SnakeHead.MoveTowardsDirection(transform.right);
        CanShoot = false;
    }
}
