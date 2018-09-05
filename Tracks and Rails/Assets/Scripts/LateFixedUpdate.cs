using UnityEngine;
using System.Collections;

public class LateFixedUpdate : MonoBehaviour {
    public MaintainInPosition maintain;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionStay()
    {
        /*Debug.Log("latefixedupdate");
        maintain.LateFixedUpdate();*/
    }
}
