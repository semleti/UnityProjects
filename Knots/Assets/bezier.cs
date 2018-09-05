using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(bezierPoint))]
public class bezier : Editor {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnSceneGUI()
	{
		float width = 1f;
		/*Handles.DrawBezier(new Vector3(1,1,1), 
		                   Vector3.zero, 
		                   Vector3.up, 
		                   -Vector3.up,
		                   Color.red, 
		                   null,
		                   width);*/
		Debug.Log ("Update");
		if (Event.current.type == EventType.scrollWheel) {
			Event.current.Use ();
		}
	}
}
