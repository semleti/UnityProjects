using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;


public class ToolBox : EditorWindow {
	public List<ToolInterface> Tools = new List<ToolInterface>();
	private int IconWidth=32;
	private int IconHeight=32;
	
	// Add menu named "My Window" to the Window menu
	[MenuItem ("RPG Creator/Tool Box")]
	static void Init () {
		// Get existing open window or if none, make a new one:
		ToolBox window = (ToolBox)EditorWindow.GetWindow (typeof (ToolBox));
		window.minSize = new Vector2(32 * 2,32);
		window.name = ("Tool Box");
		window.Show();
		SceneInheriter sceneinheritor = (SceneInheriter)EditorWindow.GetWindow (typeof (SceneInheriter));
		sceneinheritor.minSize = new Vector2(32 * 2,32);
		sceneinheritor.position = new Rect(-1000,500,500,500);
		sceneinheritor.name = ("Scene Inheriter");
		sceneinheritor.Show();

		((SceneInheriter)EditorWindow.GetWindow (typeof (SceneInheriter))).title = "potato";
		((SceneInheriter)EditorWindow.GetWindow (typeof (SceneInheriter))).size = 1f;
		((SceneInheriter)EditorWindow.GetWindow (typeof (SceneInheriter))).LookAtDirect(new Vector3(10000,15,10000), new Quaternion(0f,0f,0f,0f));
		((SceneInheriter)EditorWindow.GetWindow (typeof (SceneInheriter))).orthographic= true;
	}
	
	void Awake()
	{

		ToolBoxScriptableObject scriptableObject = ScriptableObjectUtility.LoadAsset<ToolBoxScriptableObject>("Assets/Editor/ScriptableObjects/ToolBox/ToolBoxScriptableObject.asset");
		scriptableObject.setToolBoxReference(this);
	}
	
	void OnGUI () 
	{	
		if(Tools != null && Tools.Count > 0)
			{
			int numOfColumns = (int)position.width/IconWidth;
			for (int row=0; row * numOfColumns < Tools.Count; row++)
			{
				for (int column=0; column < numOfColumns && ((column + row * numOfColumns) < Tools.Count) ;column++)
				{
					int toolsIndex = row * numOfColumns + column;
					if (Tools[toolsIndex].Icon == null)
					{
						Tools[toolsIndex].Icon = Resources.Load("DefaultIcon") as Texture;
					}
					if(GUI.Button(new Rect( column * IconWidth, row * IconHeight, IconWidth,IconHeight), Tools[toolsIndex].Icon))
					{
						Tools[toolsIndex].execute();
					}
				}
			}
		}
		else
		{
			GUI.Label (new Rect(0, 0, IconWidth * 2, IconHeight),"no tools");
		}
	}
}

