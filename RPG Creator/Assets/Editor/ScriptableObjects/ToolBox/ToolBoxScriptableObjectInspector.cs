using UnityEngine;  
using UnityEditor;  
using UnityEditorInternal;

[CustomEditor(typeof(ToolBoxScriptableObject))]
public class ToolBoxScriptableObjectInspector : Editor {  
	private ReorderableList list;
	
	private void OnEnable() {
		list = new ReorderableList(serializedObject, 
		                           serializedObject.FindProperty("test"), 
		                           true, true, true, true);
		list.drawElementCallback =  
		(Rect rect, int index, bool isActive, bool isFocused) => {
			var element = list.serializedProperty.GetArrayElementAtIndex(index);
			rect.y += 2;
			EditorGUI.PropertyField(
				new Rect(rect.x+10, rect.y, rect.width-10, EditorGUIUtility.singleLineHeight),
				element, GUIContent.none);
			
		};
	}
	
	public override void OnInspectorGUI() {
		serializedObject.Update();
		list.DoLayoutList();
		serializedObject.ApplyModifiedProperties();
	}
}