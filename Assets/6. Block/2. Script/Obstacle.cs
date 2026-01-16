using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private float Width, Height, Left, Right, Top, Bottom;
    private Vector2 CircleCenter;
    private float ClosestDistance;

    private void Start()
    {
        Width = transform.localScale.x;
        Height = transform.localScale.y;
        Left = transform.position.x - Width / 2;
        Right = transform.position.x + Width / 2;
        Top = transform.position.y + Height / 2;
        Bottom = transform.position.y - Height / 2;
    }

    public Vector2 GetCircleCenter()
    {
        return CircleCenter;
    }

    public bool IsOverlappedWith(Vector2 circleCenter, float radius, Vector2 moveDirection)
    {
        CircleCenter = circleCenter;

        float closestX = Mathf.Clamp(circleCenter.x, Left, Right);
        float closestY = Mathf.Clamp(circleCenter.y, Bottom, Top);

        float distanceX = circleCenter.x - closestX;
        float distanceY = circleCenter.y - closestY;

        ClosestDistance = distanceX * distanceX + distanceY * distanceY;

        if (ClosestDistance < radius * radius)
        {
            if (ClosestDistance == 0)
            {
                Debug.Log("overlap");
                distanceX = -moveDirection.x - 0.1f;
                distanceY = -moveDirection.y - 0.1f;
            }
            else
            {
                Debug.Log("bounce");
                GameManager.Instance.DecreaseBounceCount(1);
            }

            float depth = radius - ClosestDistance;
            Vector2 normal = new(distanceX, distanceY);
            circleCenter += normal * depth;
            CircleCenter = circleCenter;
            
            return true;
        }

        return false;
    }

    public Vector2 GetReflectDirection(Vector2 circleCenter, Vector2 direction)
    {
        if (circleCenter.x > Right && circleCenter.y < Bottom ||
            circleCenter.x < Left && circleCenter.y < Bottom ||
            circleCenter.x > Right && circleCenter.y > Top ||
            circleCenter.x < Left && circleCenter.y > Top)
        {
            return Vector2.Reflect(direction, circleCenter - (Vector2)transform.position).normalized;
        }

        if (circleCenter.x < Left || circleCenter.x > Right)
        {
            direction.x = -direction.x;
            return direction.normalized;
        }

        if (circleCenter.y < Bottom || circleCenter.y > Top)
        {
            direction.y = -direction.y;
            return direction.normalized;
        }

        return Vector2.zero;
    }
}
