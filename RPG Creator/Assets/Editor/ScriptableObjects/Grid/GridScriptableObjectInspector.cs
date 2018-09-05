using UnityEngine;  
using UnityEditor;  
using UnityEditorInternal;

[CustomEditor(typeof(GridScriptableObject))]
public class GridScriptableObjectInspector : Editor {  
	

	
	public override void OnInspectorGUI() {
		//serializedObject.Update();
		DrawDefaultInspector();
		//serializedObject.ApplyModifiedProperties();
	}
}