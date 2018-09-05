using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class Point : MonoBehaviour {
	public Knot knotParent;
	public int age;
	// Use this for initialization
	void OnEnable () {
		//gameObject.hideFlags = HideFlags.HideInHierarchy;
		gameObject.hideFlags = HideFlags.None;
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.hasChanged) {
			Debug.Log (gameObject.name + " changed");
			transform.hasChanged=false;
		}
	}

	void OnValidate()
	{
		Debug.Log (gameObject.name + " changed");
	}
}
