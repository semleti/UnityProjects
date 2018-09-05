using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class QuaternionTest : MonoBehaviour {
    [Range(0,360)]
    public float w;
    public Vector3 up = Vector3.up;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        
        transform.rotation =Quaternion.AngleAxis(w, up);
        Debug.Log(Quaternion.AngleAxis(w, up) * Vector3.up);
    }
}
