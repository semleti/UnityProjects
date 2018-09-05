using UnityEngine;
using System.Collections;
using System;

public class SquareTrajectory : Trajectory {
    [SerializeField]
    protected float a;
    [SerializeField]
    protected float b;
    [SerializeField]
    protected float c;
    [SerializeField]
    protected float xmin;
    [SerializeField]
    protected float xmax;

    protected float a2;
    protected float b2;
    protected float c2;

    protected int nbrOfPoints = 10;

    public int NbrOfPoints
    {
        get { return nbrOfPoints; }
        set
        {
            nbrOfPoints = value;
            GizmoPoints = new Vector3[nbrOfPoints];
            CalculateGizmoPoints();
        }
    }

    public float A
    {
        get { return a; }
        set { a = value; }
    }

    protected Vector3[] GizmoPoints;
    // Use this for initialization
    public void Awake()
    {
        a2 = a + 1;
        b2 = b + 1;
        c2 = c + 1;

        GizmoPoints = new Vector3[10];
        CalculateLength();
        CalculateGizmoPoints();
    }

    public override Vector3 GetPosition(float distance)
    {
        float x = GetX(distance);
        float y = GetY(x);
        return new Vector3(x, y, 0f);
    }

    public override Vector3 GetDerivate(float distance)
    {
        float x = GetX(distance);
        float y = 2f*a*x + b;
        return new Vector3(x, y, 0f);
    }

    protected float GetX(float distance)
    {
        return 0f;
    }

    protected float GetY(float x)
    {
        return a * x * x + b * x + c;
    }

    public void CalculateGizmoPoints()
    {
        for(int i =0; i< 10; i++)
        {
            float x = GetX(length / 10 * i);
            GizmoPoints[i] = new Vector3(x, GetY(x), 0f);
        }
    }

    public override void OnDrawGizmos()
    {

        Gizmos.color = Color.black;
        for(int i =0; i< GizmoPoints.Length-1; i++)
        {
            Gizmos.DrawLine(GizmoPoints[i], GizmoPoints[i + 1]);
        }
    }

    protected void UpdateIfNecessary()
    {
        if (CoeffsSame())
            return;
        CalculateLength();
        CalculateGizmoPoints();
        a2 = a;
        b2 = b;
        c2 = c;
    }

    protected void CalculateLength()
    {
        //((b+2 a x) sqrt(1+(b+2 a x)^2)+sinh^(-1)(b+2 a x))/(4 a)
        if (a == a2 && b == b2)
            return;
        length = 0f;
        length = ((b + 2f * a * xmax) * Mathf.Sqrt(1 + (b + 2 * a * xmax) * (b + 2 * a * xmax)) + SinhInv(b + 2 * a * xmax)) / (4 * a);//sitll do - xmin
    }

    protected bool CoeffsSame()
    {
        return a == a2 && b == b2 && c == c2;
    }

    public double SinhInv(double x)
    {
        return Math.Log(x + Math.Sqrt(1 + x * x));
    }

    public float SinhInv(float x)
    {
        return (float)Math.Log(x + Mathf.Sqrt(1 + x * x));
    }
}
