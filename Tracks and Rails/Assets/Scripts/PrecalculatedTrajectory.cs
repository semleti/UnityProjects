using UnityEngine;
using System.Collections.Generic;

public class PrecalculatedTrajectory : Trajectory {
    [System.Serializable]
    public class distancePoint
    {
        public float distance;
        public float t;
        public Vector3 point;
        public distancePoint(float distance, float t, Vector3 point)
        {
            this.distance = distance;
            this.t = t;
            this.point = point;
        }
    }
    
    protected List<distancePoint> points = new List<distancePoint>();
    [SerializeField]
    protected distancePoint[] dtPoints = new distancePoint[0];

    [SerializeField]
    protected float minT;
    [SerializeField]
    protected float maxT;

#if UNITY_EDITOR
    public bool drawVectors = false;
    public Color lineColor = Color.black;
    public Color vectorColor = Color.red;
#endif
    

    public override void OnDrawGizmos()
    {
        if (dtPoints.Length < 2)
            return;

        for(int i = 0; i < (dtPoints.Length-1); i++)
        {
            Gizmos.color = lineColor;
            Gizmos.DrawLine(transform.TransformPoint(dtPoints[i].point), transform.TransformPoint(dtPoints[i + 1].point));

            #if UNITY_EDITOR
            if (drawVectors)
            {
                DrawVectors(i);
            }
            #endif

        }

        #if UNITY_EDITOR
        if (drawVectors)
        {
            DrawVectors(dtPoints.Length - 1);
        }
        #endif
    }

    protected void DrawVectors(int i)
    {
        Vector3 point = dtPoints[i].point;
        Vector3 origin = transform.TransformPoint(point);
        Vector3 derivate = GetDerivateT(dtPoints[i].t);
        Vector3 binormal = Quaternion.AngleAxis(-GetWT(dtPoints[i].t), derivate) * -Vector3.Cross(derivate, Vector3.up).normalized;
        Vector3 normal = Vector3.Cross(derivate, binormal);
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(origin, transform.TransformPoint(point + derivate));

        Gizmos.color = Color.red;
        Gizmos.DrawLine(origin, transform.TransformPoint(point + binormal));

        Gizmos.color = Color.green;
        Gizmos.DrawLine(origin,
            transform.TransformPoint(point + normal));


        /*Gizmos.color = Color.magenta;
        Gizmos.DrawLine(origin, transform.TransformPoint(
            dtPoints[i].point + Quaternion.AngleAxis(W2, GetDerivateT(dtPoints[i].t)) * Vector3.Cross(GetDerivateT(dtPoints[i].t), 
            -Vector3.Cross(GetDerivateT(dtPoints[i].t), Vector3.up).normalized)));*/
    }

    public virtual void CalculatePoints()
    {
        points.Clear();
        switch (quality)
        {
            case Quality.points:
                CalculatePointsPoints();
            break;
            case Quality.angles:
                CalculatePointsAngle();
            break;
            case Quality.precision:
                CalculatePointsPrecision();
            break;
            case Quality.distanceLinePoint:
                CalculatePointsDistance();
            break;
        }
        dtPoints = points.ToArray();
        length = dtPoints[dtPoints.Length - 1].distance;
        Debug.Log("distance " + length);
        Debug.Log("points " + dtPoints.Length);
    }

    protected virtual void CalculatePointsPoints()
    {
        float dt = (maxT - minT) / (nbrOfPoints - 1);

        points.Add(new distancePoint(0, minT, GetPoint(minT)));

        for (int i = 1; i < nbrOfPoints; i++)
        {
            float t = minT + dt * i;
            Vector3 newPoint = GetPoint(t);
            float dist = points[i - 1].distance;
            dist += (newPoint - points[i - 1].point).magnitude;
            points.Add(new distancePoint(dist, t, newPoint));
        }
    }

    protected virtual void CalculatePointsAngle()//uses recurence, does the first step to launch recurence
    {
        Vector3 pointA = GetPoint(minT);
        Vector3 pointB = GetPoint(maxT);

        points.Add(new distancePoint(0, minT, pointA));
        CalculatePointsAngleRecurence(pointA, pointB, GetDerivateT(minT), GetDerivateT(maxT), minT, maxT);
        AddPoint(pointB, maxT);
    }

    protected virtual void CalculatePointsAngleRecurence(Vector3 pointA, Vector3 pointB, Vector3 derivA, Vector3 derivB, float t, float T)//given two points, calculates middle point, if necessary calls itself with pointA and middle, and middle and pointB
    {
        float at = (t + T) / 2f;
        Vector3 pointC = GetPoint(at);
        Vector3 derivC = GetDerivateT(at);
        if (GetAngle(derivA, derivC) > angle)
        {
            CalculatePointsAngleRecurence(pointA, pointC, derivA, derivC, t, at);
        }
        AddPoint(pointC, at);
        if(GetAngle(derivC, derivB) > angle)
        {
            CalculatePointsAngleRecurence(pointC, pointB, derivC, derivB, at, T);
        }
    }

    public float GetAngle(Vector3 A, Vector3 B)
    {
        return Mathf.Rad2Deg * Mathf.Acos(Vector3.Dot(A, B));
    }

    protected virtual void CalculatePointsPrecision()//uses recurence, does the first step to launch recurence
    {
        Vector3 pointA = GetPoint(minT);
        Vector3 pointB = GetPoint(maxT);

        points.Add(new distancePoint(0, minT, pointA));
        CalculatePointsPrecisionRecurence(pointA, pointB, (pointB - pointA).magnitude, minT, maxT);
        AddPoint(pointB, maxT);
    }

    protected virtual void CalculatePointsPrecisionRecurence(Vector3 pointA, Vector3 pointB, float AB, float t, float T)
    {
        float at = (t + T) / 2f;
        Vector3 pointC = GetPoint(at);
        float AC = (pointC - pointA).magnitude;
        float CB = (pointB - pointC).magnitude;
        if (AC + CB - AB > precision)
        {
            CalculatePointsPrecisionRecurence(pointA, pointC, AC, t, at);
            AddPoint(pointC, at);
            CalculatePointsPrecisionRecurence(pointC, pointB, CB, at, T);
        }
        else
            AddPoint(pointC, T);
    }

    protected virtual void CalculatePointsDistance()
    {
        Vector3 pointA = GetPoint(minT);
        Vector3 pointB = GetPoint(maxT);

        points.Add(new distancePoint(0, minT, pointA));
        CalculatePointsDistanceRecurence(pointA, pointB, minT, maxT);
        AddPoint(pointB, maxT);
    }

    protected virtual void CalculatePointsDistanceRecurence(Vector3 pointA, Vector3 pointB, float t, float T)
    {
        float at = (t + T) / 2f;
        Vector3 pointC = GetPoint(at);
        Vector3 AB = (pointB - pointA);
        Vector3 AC = (pointC - pointA);
        float dist = (AC - Vector3.Dot(AC, AB) * AB / AB.sqrMagnitude).magnitude;
        if (dist > distance)
        {
            CalculatePointsDistanceRecurence(pointA, pointC, t, at);
            AddPoint(pointC, at);
            CalculatePointsDistanceRecurence(pointC, pointB, at, T);
        }
        else
            AddPoint(pointC, T);
    }

    protected void AddPoint(Vector3 point, float t)//adds a point at the end
    {
        float dist = points[points.Count - 1].distance;
        dist += (point - points[points.Count - 1].point).magnitude;
        points.Add(new distancePoint(dist, t, point));
    }

    public virtual Vector3 GetPoint(float t)
    {
        return Vector3.zero;
    }

    public virtual Vector3 GetDerivateT(float t)
    {
        return Vector3.zero;
    }

    public virtual float GetWT(float t)
    {
        return 0f;
    }

    /*public virtual float GetT(float distance)//not finished
    {
        int i = 0;
        while (i < points.Count && points[i].t < t)
        {
            i++;
        }
        if (points[i].t == t)
            return points[i].point;
        else
        {
            float g = (points[i].t - points[i - 1].t) / (t - points[i - 1].t);
            return Vector3.zero;
        }
    }

    public virtual float GetDistance(float t)
    {
        return 0f;
    }*/

    public float DistanceToT(float distance)
    {
        int count = dtPoints.Length;
        if (distance <= 0)
            return dtPoints[0].t;
        if (distance >= length)
            return dtPoints[count - 1].t;

        int index = DistanceToTRecurence(distance, 0, count-1);

        return Mathf.Lerp(dtPoints[index].t, dtPoints[index +1].t, (distance - dtPoints[index].distance)/(dtPoints[index+1].distance - dtPoints[index].distance));
    }

    protected int DistanceToTRecurence(float distance, int i, int j)
    {
        int index = (i + j) / 2;
        if (dtPoints[index].distance > distance)
            return DistanceToTRecurence(distance, i, index);
        else if (dtPoints[index + 1].distance < distance)
            return DistanceToTRecurence(distance, index, j);
        return index;
    }
}
