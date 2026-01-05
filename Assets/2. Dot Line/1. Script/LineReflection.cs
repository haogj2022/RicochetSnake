using System.Collections.Generic;
using UnityEngine;

public class LineReflection : MonoBehaviour
{
    [SerializeField] private LineRenderer DotLine;
    [SerializeField] private int MaxReflections = 2;
    private float MaxDistance = 100f;
    private List<Vector3> LinePoints;

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            DotLine.enabled = true;
            ReflectLine();
        }

        if (Input.GetMouseButtonUp(0))
        {
            DotLine.enabled = false;
        }
    }

    private void ReflectLine()
    {
        LinePoints = new() { transform.position };

        Vector2 origin = transform.position;
        Vector2 direction = transform.right;

        for (int i = 0; i <= MaxReflections; i++)
        {
            RaycastHit2D hit = Physics2D.Raycast(origin, direction, MaxDistance);

            if (hit.collider != null)
            {
                LinePoints.Add(hit.point);
                direction = Vector2.Reflect(direction, hit.normal);
                origin = hit.point + (direction * 0.01f);
            }
            else
            {
                LinePoints.Add(origin + (direction * MaxDistance));
                break;
            }
        }

        DotLine.positionCount = LinePoints.Count;
        DotLine.SetPositions(LinePoints.ToArray());
    }
}
