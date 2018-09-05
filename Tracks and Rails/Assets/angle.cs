using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class angle : MonoBehaviour {
    public Vector3 A;
    public Vector3 B;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Debug.Log(Vector3.Dot(A.normalized, B.normalized));
        Debug.Log(Mathf.Rad2Deg * Mathf.Acos(Vector3.Dot(A.normalized, B.normalized)));
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawLine(Vector3.zero, A);
        Gizmos.DrawLine(Vector3.zero, B);
    }
}
