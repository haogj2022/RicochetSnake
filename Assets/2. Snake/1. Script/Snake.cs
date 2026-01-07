using UnityEngine;

public class Snake : MonoBehaviour
{
    [SerializeField] private Rigidbody2D SnakeHead;
    [SerializeField] private float MoveSpeed = 5f;
    private Vector2 MoveDirection;

    public void MoveTowardsDirection(Vector2 direction)
    {
        MoveDirection = direction.normalized;
        SnakeHead.velocity = MoveDirection * MoveSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Obstacle"))
        {
            BounceOff(collision);
            GameManager.Instance.DecreaseBounceCount(1);
        }

        if (collision.collider.CompareTag("Stop"))
        {
            SnakeHead.velocity = Vector2.zero;
            GameManager.Instance.OnMoveComplete();
        }
    }

    private void BounceOff(Collision2D collision)
    {
        ContactPoint2D contact = collision.GetContact(0);
        MoveDirection = Vector2.Reflect(MoveDirection, contact.normal).normalized;

        SnakeHead.velocity = MoveDirection * MoveSpeed;

        float angle = Mathf.Atan2(MoveDirection.y, MoveDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Food"))
        {
            collision.gameObject.SetActive(false);
        }
    }
}
