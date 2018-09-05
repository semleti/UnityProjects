using UnityEngine;
using UnityEditor;
using System.Collections;

[ExecuteInEditMode]
public class GridScriptableObject : ScriptableObject {
	public bool isActive = false;
	public bool enabled
	{
		get
		{
			return isActive;
		}
		set
		{
			if(value)
			{
				Enable();
				Debug.Log ("value set to true");
			}
			else
			{
				Disable ();
				Debug.Log ("value set to false");
			}
		}
	}
	void OnEnable(){
		Enable ();
	}
	
	void OnDisable(){
		//Disable();
	}
	
	
	
	void OnSceneGUI(SceneView sceneview = null)
	{
		if(SceneView.currentDrawingSceneView.name == "")
		{
			if(isActive)
			{
				Activate ();
				//DrawDefaultInspector();
				Handles.color = Color.blue;
				Handles.DrawLine(new Vector3(0,0,0), new Vector3(1,1,1));
				Handles.BeginGUI();
				GUILayout.BeginArea(new Rect(0,0,Screen.width - 10, Screen.height));
				if(GUILayout.Button("Reset Area"))
				{
					Deactivate ();
				}
				GUILayout.EndArea();
				Handles.EndGUI();
			}
			else{
				Deactivate();
			}
		}
		else if(SceneView.currentDrawingSceneView.name == "Scene Inheriter")
		{
			GUILayout.BeginArea(new Rect(100,Screen.height-100,Screen.width - 100, 100));
			GUI.backgroundColor = new Color(255f,70f,70f);
			GUI.color = new Color(70f,255f,70f);
			
			GUILayout.Button("test");
			GUILayout.EndArea();
		}
	}
	
	void Disable()
	{
		Deactivate ();
		SceneView.onSceneGUIDelegate -= this.OnSceneGUI;
		Debug.Log ("Disabled!");
	}
	
	void Enable()
	{
		Activate ();
		SceneView.onSceneGUIDelegate -= this.OnSceneGUI;
		SceneView.onSceneGUIDelegate += this.OnSceneGUI;
	}
	
	void Activate()
	{
		isActive = true;
		Tools.current = new Tool();
		Tools.hidden = true; //hide default tools when activated
	}
	
	void Deactivate()
	{
		isActive = false;
		Tools.hidden = false; //unhide default tools when deactived
	}
}
