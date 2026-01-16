using UnityEngine;

public class SnakeBody : MonoBehaviour
{
    private Vector2 BodyPosition;
    private Quaternion BodyRotation;
    private Vector2 BodyScale;

    public void SetPosition(Vector2 start, Vector2 end)
    {
        BodyPosition = (Vector3)(start + end) / 2;

        Vector2 newDirection = (start - end).normalized;
        float angle = Mathf.Atan2(newDirection.y, newDirection.x) * Mathf.Rad2Deg;
        BodyRotation = Quaternion.Euler(0, 0, angle);

        float distance = Vector2.Distance(start, end);
        BodyScale = new Vector2(distance, transform.localScale.y);

        transform.position = BodyPosition;
        transform.rotation = BodyRotation;
        transform.localScale = BodyScale;
    }
}
