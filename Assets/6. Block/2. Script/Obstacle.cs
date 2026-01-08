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

    public bool IsCollidedWith(Vector3 position, float radius, Vector3 direction)
    {
        if (position.x >= Left - radius && position.x <= Right + radius &&
            position.y <= Up + radius && position.y >= Down - radius)
        {
            return true;
        }
        return false;
    }

    public Vector3 HitNormal(Vector3 position, float radius)
    {
        if (position.x >= Left - radius)
        {
            return Vector3.left;
        }

        if (position.x <= Right + radius)
        {
            return Vector3.right;
        }

        if (position.y <= Up + radius)
        {
            return Vector3.up;
        }

        if (position.y >= Down - radius)
        {
            return Vector3.down;
        }

        return Vector3.zero;
    }
}
