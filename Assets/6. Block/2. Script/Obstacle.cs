using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private float Width, Height, Left, Right, Top, Bottom;
    private Vector3 CircleCenter;

    private void Start()
    {
        Width = transform.localScale.x;
        Height = transform.localScale.y;
        Left = transform.position.x - Width / 2;
        Right = transform.position.x + Width / 2;
        Top = transform.position.y + Height / 2;
        Bottom = transform.position.y - Height / 2;
    }

    public Vector3 GetCircleCenter()
    {
        return CircleCenter;
    }

    public bool IsOverlappedWith(Vector3 circleCenter, float radius)
    {
        float closestX = Mathf.Clamp(circleCenter.x, Left, Right);
        float closestY = Mathf.Clamp(circleCenter.y, Bottom, Top);

        float distanceX = circleCenter.x - closestX;
        float distanceY = circleCenter.y - closestY;
        float distance = distanceX * distanceX + distanceY * distanceY;

        if (distance < radius * radius)
        {
            float depth = radius - distance + 0.01f;
            Vector3 normal = new(distanceX, distanceY);
            circleCenter += normal * depth;
            CircleCenter = circleCenter;

            return true;
        }

        return false;
    }

    public Vector3 GetReflectDirection(Vector3 circleCenter, Vector3 direction)
    {
        if (circleCenter.x > Right && circleCenter.y < Bottom ||
            circleCenter.x < Left && circleCenter.y < Bottom ||
            circleCenter.x > Right && circleCenter.y > Top ||
            circleCenter.x < Left && circleCenter.y > Top)
        {
            return Vector2.Reflect(direction, (circleCenter - transform.position).normalized);
        }

        if (circleCenter.x < Left || circleCenter.x > Right)
        {
            direction.x = -direction.x;
            return direction;
        }

        if (circleCenter.y < Bottom || circleCenter.y > Top)
        {
            direction.y = -direction.y;
            return direction;
        }

        return Vector3.zero;
    }
}
