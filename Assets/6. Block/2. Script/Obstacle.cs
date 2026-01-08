using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private float Width, Height;

    private void Start()
    {
        Width = transform.localScale.x;
        Height = transform.localScale.y;
    }

    public float CalculatedDistance(Vector3 playerPos, float playerRadius)
    {
        Vector3 distance = playerPos - transform.position;

        float clampedX = Mathf.Clamp(distance.x, -(Width / 2), Width / 2);
        float clampedY = Mathf.Clamp(distance.y, -(Height / 2), Height / 2);
        Vector3 closetPoint = transform.position + new Vector3(clampedX, clampedY, 0);

        float distanceToClosetPoint = Vector3.Distance(playerPos, closetPoint);
        float actualDistance = distanceToClosetPoint - playerRadius;

        return Mathf.Max(0, actualDistance);
    }
}
