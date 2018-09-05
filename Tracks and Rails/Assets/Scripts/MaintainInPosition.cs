using UnityEngine;
using System.Collections;

public class MaintainInPosition : MonoBehaviour {
    protected Rigidbody rigid;
    protected Collider coll;
    public Vector3 velocity;
    public float distance = 0f;
    protected Vector3 previousPosition;
    public bool useGravity = false;
    protected Vector3 velocityToAdd;
    public Trajectory traj;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        coll = GetComponent<Collider>();
        rigid.position = GetPosition(distance);
        previousPosition = rigid.position;
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

    }

    static public float BounceCombine(float bounciness1, float bounciness2, PhysicMaterialCombine mat1, PhysicMaterialCombine mat2)
    {
        if (mat1 == PhysicMaterialCombine.Maximum || mat2 == PhysicMaterialCombine.Maximum)
            return Mathf.Max(bounciness1, bounciness2);
        else if (mat1 == PhysicMaterialCombine.Multiply || mat2 == PhysicMaterialCombine.Multiply)
            return bounciness1 * bounciness2;
        else if (mat1 == PhysicMaterialCombine.Minimum || mat2 == PhysicMaterialCombine.Minimum)
            return Mathf.Min(bounciness1, bounciness2);
        else
            return (bounciness1 + bounciness2) / 2f;
    }

    static public float BounceCombine(PhysicMaterial mat1, PhysicMaterial mat2)
    {
        /*if (mat1.bounceCombine == PhysicMaterialCombine.Maximum || mat2.bounceCombine == PhysicMaterialCombine.Maximum)
            return Mathf.Max(mat1.bounciness, mat2.bounciness);
        else if (mat1.bounceCombine == PhysicMaterialCombine.Multiply || mat2.bounceCombine == PhysicMaterialCombine.Multiply)
            return mat1.bounciness * mat2.bounciness;
        else if (mat1.bounceCombine == PhysicMaterialCombine.Minimum || mat2.bounceCombine == PhysicMaterialCombine.Minimum)
            return Mathf.Min(mat1.bounciness, mat2.bounciness);
        else
            return (mat1.bounciness + mat2.bounciness) / 2f;*/
        return BounceCombine(mat1.bounciness, mat2.bounciness, mat1.bounceCombine, mat2.bounceCombine);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (rigid.isKinematic)
        {
            Vector3 va;
            Vector3 vb;
            /*Debug.Log(rigid.velocity);
            Debug.Log(collision.rigidbody.velocity);*/
            float bounciness = BounceCombine(coll.sharedMaterial, collision.collider.sharedMaterial);
            //Debug.Log("bounciness " + bounciness);
            InelasticCollision(bounciness, rigid.mass, collision.rigidbody.mass, rigid.velocity, rigid.velocity + collision.relativeVelocity, out va, out vb);
            /*Debug.Log("va " + va);
            Debug.Log("vb " + vb);*/
            velocityToAdd += va - rigid.velocity;
            collision.rigidbody.AddForce(vb - collision.rigidbody.velocity, ForceMode.VelocityChange);
            //Debug.Log("impulse " + collision.impulse);
            /*rigid.isKinematic = false;
            rigid.isKinematic = true;*/
            //rigid.AddForce(collision.impulse);
            //velocity += collision.relativeVelocity;
            //Debug.Log("previousPosition1 " + previousPosition);
            //previousPosition -= collision.relativeVelocity * Time.fixedDeltaTime;
            //Debug.Log("previousPosition2 " + previousPosition);
            //velocity = rigid.velocity;
            //collision.contacts[0].
            //rigid.velocity = collision.impulse;
            /*collision.rigidbody.useGravity = false;
            collision.rigidbody.velocity = Vector3.zero;
            collision.rigidbody.angularVelocity = Vector3.zero;*/
        }
    }

    public void InelasticCollision(float bounciness, float ma, float mb, Vector3 va1, Vector3 vb1, out Vector3 va2, out Vector3 vb2)
    {
        bounciness = Mathf.Clamp01(bounciness);
        va2 = (bounciness * mb * (vb1 - va1) + ma * va1 + mb * vb1) / ( ma + mb) /*/ Time.fixedDeltaTime*/;
        vb2 = (bounciness * ma * (va1 - vb1) + ma * va1 + mb * vb1) / (ma + mb) /*/ Time.fixedDeltaTime*/;
    }

    public void FixedUpdate()
    {
        //Debug.Log("previousPosition " + previousPosition);
        ApplyForce((rigid.position - previousPosition)/Time.fixedDeltaTime);
        previousPosition = rigid.position;
    }

    void ApplyForce(Vector3 force)
    {
        if (!rigid.isKinematic)
        {
            bool moveOrVel = true;
            //Debug.Log("force " + force);
            //Debug.Log("velocity1 " + rigid.velocity);
            if (useGravity)
                force += Physics.gravity;

            Vector3 vel = Vector3.Project(force, GetDerivate(distance));


            if (moveOrVel)
                rigid.velocity = vel;
            //Debug.Log("velocity2 " + rigid.velocity);
            Debug.Log(Mathf.Sign(Vector3.Dot(vel.normalized, GetDerivate(distance))));
            //distance += vel.magnitude * Vector3.Dot(vel.normalized, GetDerivate(distance).normalized) * Time.fixedDeltaTime;
            distance += vel.magnitude * Mathf.Sign(Vector3.Dot(vel, GetDerivate(distance))) * Time.fixedDeltaTime;
            Debug.Log("new position " + GetPosition(distance));
            //rigid.isKinematic = true;
            if (!moveOrVel)
                rigid.MovePosition(GetPosition(distance));
            //rigid.isKinematic = false;
            //rigid.velocity = (GetPosition(distance) - previousPosition) / Time.fixedDeltaTime;

            //Debug.Log("velocity3 " + rigid.velocity);

            /*if (useGravity)
                rigid.AddForce(Physics.gravity);*/
        }
        else
        {
            Debug.Log("velocityToAdd " + velocityToAdd);
            velocity += velocityToAdd;
            if (useGravity)
                velocity += Physics.gravity * Time.fixedDeltaTime;
            Vector3 vel = Vector3.Project(velocity, GetDerivate(distance));
            velocity = vel;
            distance += vel.magnitude * Mathf.Sign(Vector3.Dot(vel, GetDerivate(distance))) * Time.fixedDeltaTime;
            rigid.MovePosition(GetPosition(distance));
        }
        velocityToAdd = Vector3.zero;
    }

    public Vector3 GetDerivate(float distance)
    {
        return traj.GetDerivate(distance);
    }

    public Vector3 GetPosition(float distance)
    {
        return traj.GetPosition(distance);
    }
}
