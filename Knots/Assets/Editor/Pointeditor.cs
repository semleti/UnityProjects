using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(Point))]

public class PointEditor : Editor {
	
	void OnEnable(){

	}
	
	public override void OnInspectorGUI()
	{
		Point myTarget = (Point)target;
		//base.OnInspectorGUI ();
		KnotEditor.Draw (myTarget.knotParent, myTarget.knotParent.points.IndexOf(myTarget.gameObject));
	}
}