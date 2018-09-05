using UnityEngine;
using System.Collections;

public class tempDebug : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter(Collision coll)
    {
        Debug.Log(coll.rigidbody.velocity);
        Debug.Log(coll.relativeVelocity);
    }
}
