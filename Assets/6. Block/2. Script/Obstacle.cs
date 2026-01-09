using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private float Width, Height, Left, Right, Up, Down;

    private void Start()
    {
        Width = transform.localScale.x;
        Height = transform.localScale.y;
        Left = transform.position.x - (Width / 2);
        Right = transform.position.x + (Width / 2);
        Up = transform.position.y + (Height / 2);
        Down = transform.position.y - (Height / 2);
    }

    public bool IsCollidedWith(Vector3 position, float radius)
    {
        if (position.x >= Left - radius && position.x <= Right + radius &&
            position.y <= Up + radius && position.y >= Down - radius)
        {
            return true;
        }
        return false;
    }

    public Vector3 GetNewDirection(Vector3 position, float radius, Vector3 direction)
    {
        if (position.x >= Left && position.x <= Right &&
            position.y <= Up + radius && position.y > Down)
        {
            return Vector3.Reflect(direction, Vector3.up);
        }

        if (position.x >= Left && position.x <= Right &&
            position.y >= Down - radius && position.y < Up)
        {
            return Vector3.Reflect(direction, Vector3.down);
        }

        if (position.x >= Left - radius && position.x < Right &&
            position.y >= Down && position.y <= Up)
        {
            return Vector3.Reflect(direction, Vector3.left);
        }

        if (position.x <= Right + radius && position.x > Left &&
            position.y >= Down && position.y <= Up)
        {
            return Vector3.Reflect(direction, Vector3.right);
        }

        return Vector3.Reflect(direction, direction);
    }
}
