using UnityEngine;
using System.Collections;
using UnityEditor;

[ExecuteInEditMode]
public class UItoOnGUI : ScriptableObject {
	static public UItoOnGUIWindow window;
	public Canvas canvas;
	
	[MenuItem ("UItoOnGUI/window")]
	static void Init () {
		// Get existing open window or if none, make a new one:
		window = (UItoOnGUIWindow)EditorWindow.GetWindow (typeof (UItoOnGUIWindow));
		window.minSize = new Vector2(32 * 2,32);
		window.name = ("UItoOnGUI");
		window.Show ();
	}
	
	void OnEnable () {
		if(!window)
			Init ();
		UItoOnGUIWindow.inspector = this;
		Debug.Log ("scriptable");
	}
	
}