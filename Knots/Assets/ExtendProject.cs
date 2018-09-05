using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class ExtendProject : Editor {

	static Dictionary<string,string> markedAssetsID;
	
	static ExtendProject ()
	{

		EditorApplication.projectWindowItemOnGUI += ProjectItemCB;
	}
	
	static void UpdateCB ()
	{
		markedAssetsID = new Dictionary<string,string> ();
	}

	//Do smthg like the markedObjects
	static void ProjectItemCB (string instanceID, Rect selectionRect)
	{
		
		// place the icoon to the right of the list:
		Rect r = new Rect (selectionRect); 
		r.y = r.height ;
		r.height = 18;
		//Debug.Log ("instanceID " + AssetDatabase.GUIDToAssetPath(instanceID));
		// Draw the texture if it's a light (e.g.)
		
		
		string newPath = GUI.TextField (r, AssetDatabase.GUIDToAssetPath (instanceID));
		string[] splits2 = newPath.Split("/".ToCharArray());
		string newPathSplit = splits2[splits2.Length-1];
		if (newPathSplit.Length > 4) {
			newPathSplit = newPathSplit.Remove (newPathSplit.Length - 4);
		}
		Debug.Log (newPathSplit);
		string previousPath = "";
		if(markedAssetsID.TryGetValue(AssetDatabase.GUIDToAssetPath (instanceID), out previousPath))
		{
			if(previousPath!=newPath)
			{
				if(!markedAssetsID.ContainsKey(newPath))
				{
					Debug.Log ("change: "+ previousPath +" "+newPathSplit );
					Debug.Log ("error: "+AssetDatabase.RenameAsset (previousPath,newPathSplit ));
					markedAssetsID.Remove(previousPath);
					markedAssetsID.Add(newPath,newPath);
				}
				else{
					Debug.Log ("There already is an asset named: "+newPath);
				}
			}
		}
		else
		{
			markedAssetsID.Add(newPath,newPath);
		}
	}
}
