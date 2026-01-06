using UnityEngine;

public class Snake : MonoBehaviour
{
    [SerializeField] private Rigidbody2D SnakeBody;
    [SerializeField] private float MoveSpeed = 5f;
    private Vector2 MoveDirection;

    public void MoveTowardsDirection(Vector2 direction)
    {
        MoveDirection = direction.normalized;
        SnakeBody.velocity = MoveDirection * MoveSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var firstContact = collision.contacts[0];
        MoveDirection = Vector2.Reflect(MoveDirection, firstContact.normal).normalized;
        SnakeBody.velocity = MoveDirection * MoveSpeed;
    }

    private void Update()
    {
        float angle = Mathf.Atan2(MoveDirection.y, MoveDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
