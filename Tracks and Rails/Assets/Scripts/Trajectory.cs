using UnityEngine;
using System.Collections;

public class Trajectory : MonoBehaviour{
    protected float length = 0;
    [SerializeField]
    [Tooltip("The quality setting used to aproximate the trajectory")]
    protected Quality quality;
    [SerializeField]
    [Tooltip("the number of points used")]
    protected int nbrOfPoints = 2;
    [SerializeField]
    [Range(0,180)]
    [Tooltip("the angle between two segments which doesn't require another point")]
    protected float angle = 160f;
    [SerializeField]
    [Tooltip("the distance gained when inserting a point which doesn't require another point")]
    protected float precision = 0.1f;
    [SerializeField]
    [Tooltip("the distance from the line to the point to insert which does'nt require another point")]
    protected float distance = 0.1f;

    virtual public float Length
    {
        get
        {
            return Length;
        }
    }

    virtual public Vector3 GetDerivate(float distance)
    {
        return Vector3.zero;
    }

    virtual public Vector3 GetPosition(float distance)
    {
        return Vector3.zero;
    }

    virtual public void OnDrawGizmos()
    {

    }

    public enum Quality
    {
        points,
        angles,
        precision,
        distanceLinePoint
    }
}
