using UnityEngine;
using System.Collections;

public class LinearTrajectory : Trajectory {

    public Vector3 pointA;
    public Vector3 pointB;
    protected Vector3 derivate;

    public void Awake()
    {
        derivate = (pointB - pointA).normalized;
        length = (pointB - pointA).magnitude;
    }

    override public Vector3 GetDerivate(float distance)
    {
        return derivate;
    }

    public override Vector3 GetPosition(float distance)
    {
        distance = Mathf.Clamp(distance, 0f, length);
        return pointA + derivate * distance;
    }

    public override void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawLine(pointA, pointB);
    }
}
