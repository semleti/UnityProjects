using UnityEditor;
using UnityEngine;

public class LayerUtility{
	
	//creates a new layer
	static void CreateLayer(int id, string Name){
		SerializedObject tagManager= new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
		
		SerializedProperty it = tagManager.GetIterator();
		bool showChildren = true;
		while (it.NextVisible(showChildren))
		{
			//set your tags here
			if (it.displayName == ("Element " + id.ToString()))
			{
				it.stringValue = Name;
			}
		}
		tagManager.ApplyModifiedProperties();
	}
}