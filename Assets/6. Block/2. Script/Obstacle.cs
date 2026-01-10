using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private float Width, Height;
    private Vector3 BottomLeftCorner;
    private Vector3 CircleCenter;

    private void Start()
    {
        Width = transform.localScale.x;
        Height = transform.localScale.y;

        BottomLeftCorner = new Vector3(
            transform.position.x - (Width / 2),
            transform.position.y - (Height / 2), 0);
    }

    public Vector3 GetCircleCenter()
    {
        return CircleCenter;
    }

    public bool IsOverlappedWith(Vector3 circle, float radius)
    {
        float closestX = Mathf.Clamp(circle.x, BottomLeftCorner.x, BottomLeftCorner.x + Width);
        float closestY = Mathf.Clamp(circle.y, BottomLeftCorner.y, BottomLeftCorner.y + Height);

        float distanceX = circle.x - closestX;
        float distanceY = circle.y - closestY;
        float distance = Mathf.Sqrt(distanceX * distanceX + distanceY * distanceY);

        if (distance < radius && distance != 0)
        {
            float depth = radius - distance + 0.01f;
            Vector3 normal = new(distanceX / distance, distanceY / distance);
            circle.x += normal.x * depth;
            circle.y += normal.y * depth;
            CircleCenter = circle;

            return true;
        }

        return false;
    }

    public Vector3 GetReflectDirection(Vector3 circle, float radius, Vector3 direction)
    {
        if (circle.x < BottomLeftCorner.x || circle.x > BottomLeftCorner.x + Width)
        {
            direction.x = -direction.x;
            return direction;
        }

        if (circle.y < BottomLeftCorner.y || circle.y > BottomLeftCorner.y + Height)
        {
            direction.y = -direction.y;
            return direction;
        }

        return Vector3.zero;
    }
}
