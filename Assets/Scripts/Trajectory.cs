using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trajectory : MonoBehaviour
{

    private static LineRenderer _lineRenderer;
    private static int _numPoints = 50;
    private static float _timeBetween = 0.1f;

    public static void SetParams(int numPoints, float timeBetween, LineRenderer lineRenderer)
    {
        _lineRenderer = lineRenderer;
        _numPoints = numPoints;
        _timeBetween = timeBetween;
    }

    public static void SetPoints(Vector3 startPoint, Vector3 startVelocity)
    {
        _lineRenderer.positionCount = _numPoints;

        List<Vector3> points = new List<Vector3>();
        for (float t = 0; t < _numPoints; t += _timeBetween)
        {
            Vector3 newPoint = startPoint + t * startVelocity;
            newPoint.y = startPoint.y + startVelocity.y * t + Physics.gravity.y / 2f * t * t;
            points.Add(newPoint);

        }
        _lineRenderer.SetPositions(points.ToArray());
    }
}
