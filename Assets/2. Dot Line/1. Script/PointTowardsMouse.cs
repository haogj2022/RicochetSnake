using UnityEngine;

public class PointTowardsMouse : MonoBehaviour
{
    [SerializeField] private float MinAngle = 0f;
    [SerializeField] private float MaxAngle = 180f;

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            PointAtMouse();
        }
    }

    private void PointAtMouse()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePos - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        if (angle < MinAngle)
        {
            if (mousePos.x > transform.position.x)
            {
                angle = MinAngle;
            }
            else
            {
                angle = MaxAngle;
            }
        }
        else if (angle > MaxAngle)
        {
            angle = MaxAngle;
        }

        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}
