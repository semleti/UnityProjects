using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class PolynomialTrajectory : PrecalculatedTrajectory {

    [SerializeField]
    protected float[] x;
    [SerializeField]
    protected float[] y;
    [SerializeField]
    protected float[] z;
    [SerializeField]
    protected float[] w;

#if UNITY_EDITOR
    public bool update = false;
    public bool showDerivs;
#endif
    
    public void Update()
    {
#if UNITY_EDITOR
        if(update)
        {
            update = false;
            CalculatePoints();
        }
        if(showDerivs)
        {
            for(int i =0; i<points.Count; i++)
            {
                Debug.Log(points[i].t + " " + GetDerivateT(points[i].t));
            }
        }
#endif
    }

    protected bool CheckChanged()
    {
        return false;
    }

    protected void CalculateLength()
    {
        
    }

    public override Vector3 GetPosition(float distance)
    {
        return base.GetPosition(distance);
    }

    public override Vector3 GetPoint(float t)//returns a point for a value of t
    {
        return new Vector3(GetAxis(t, x),GetAxis(t, y),GetAxis(t, z));
    }

    public override float GetWT(float t)//returns the rotation for a value of t
    {
        return GetAxis(t, w);
    }

    public float GetAxis(float t, float[] coeffs)//returns one component(x,y or z) of a point for a value of t
    {
        if (coeffs.Length < 1)//if there are no coeffs, assumes all ar 0
            return 0f;

        float value = coeffs[0];
        float T = 1;
        for (int power = 1; power < coeffs.Length; power++)
        {
            T *= t;//increments the power of T (pow(t,coeff))
            value += T * coeffs[power];//add the coeff * t^power
        }
        return value;
    }

    public override Vector3 GetDerivateT(float t)//Returns the derivate for a valuie of t
    {
        return new Vector3(GetDerivateAxis(t, x),GetDerivateAxis(t, y),GetDerivateAxis(t, z)).normalized;
    }

    public float GetDerivateAxis(float t, float[] coeffs)//returns one component (x,y or z) of the derivate for a value of t
    {
        if (coeffs.Length < 2)//if there are is 1 coeff, the derivative is 0, if there are no coeffs, assumes all are 0
            return 0;

        float value = coeffs[1];
        float T = 1;
        for(int power = 2; power < coeffs.Length; power++)
        {
            T *= t;//increment the power of T (pow(t,power-1))
            value += coeffs[power] * power * T;//add the coeff * power * t^(power-1)
        }
        return value;
    }

    /*public override void OnDrawGizmos()
    {
        nbrOfPoints = Mathf.Max(nbrOfPoints, 2);
        Vector3 pointa = GetPoint(minT);
        Vector3 pointb;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(pointa, pointa + GetDerivateT(minT));

        float dt = (maxT - minT) / (nbrOfPoints - 1);
        for (int i = 1; i< nbrOfPoints; i++)
        {
            float t = minT + dt * i;

            pointb = GetPoint(t);
            Gizmos.color = Color.black;
            Gizmos.DrawLine(pointa, pointb);

            Gizmos.color = Color.red;
            Gizmos.DrawLine(pointb, pointb + GetDerivateT(t));

            pointa = pointb;
        }
    }*/

}
