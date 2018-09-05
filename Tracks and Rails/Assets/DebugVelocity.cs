using UnityEngine;
using System.Collections;

public class DebugVelocity : MonoBehaviour {
    protected Rigidbody rigid;
    public bool applyForce = false;
    public Vector3 force;
	// Use this for initialization
	void Start () {
        rigid = GetComponent<Rigidbody>();
        if (applyForce)
            rigid.velocity = force;
	}
	
	// Update is called once per frame
	void Update () {
        Debug.Log(name + " " + rigid.velocity +" magnitude" + rigid.velocity.magnitude);
	}
}
