using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class ExtendHierarchy : Editor {

	static Texture2D texture;
	static List<int> markedObjectsID;
	static Dictionary<int,GameObject> markedObjects;
	
	static ExtendHierarchy ()
	{
		// Init
		texture = AssetDatabase.LoadAssetAtPath ("Assets/Images/TestIcon.png", typeof(Texture2D)) as Texture2D;
		EditorApplication.update += UpdateCB;
		EditorApplication.hierarchyWindowItemOnGUI += HierarchyItemCB;
	}
	
	static void UpdateCB ()
	{
		// Check here
		GameObject[] go = Object.FindObjectsOfType (typeof(GameObject)) as GameObject[];
		
		markedObjectsID = new List<int> ();
		markedObjects = new Dictionary<int,GameObject> ();
		foreach (GameObject g in go) 
		{
			// Example: mark all lights
			if (g.GetComponent<Light> () != null)
				markedObjectsID.Add (g.GetInstanceID ());
			markedObjects.Add (g.GetInstanceID(),g);
		}
		
	}
	
	static void HierarchyItemCB (int instanceID, Rect selectionRect)
	{
		// place the icoon to the right of the list:
		Rect r = new Rect (selectionRect); 
		r.x = r.width - 20;
		r.width = 18;
		
		if (markedObjectsID.Contains (instanceID)) 
		{
			// Draw the texture if it's a light (e.g.)
			GUI.Label (r, texture); 
			r.x += 20;
			GUI.Button(r,"b");
			GUI.Button (new Rect(0,-0,100,100),"LOL");
		}
		
	}
}
