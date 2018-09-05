using UnityEngine;
using UnityEditor;
using System.IO;

public static class ScriptableObjectUtility
{
	/// <summary>
	//	This makes it easy to create, name and place unique new ScriptableObject asset files.
	/// </summary>
	public static void CreateAsset<T> (string path = "") where T : ScriptableObject
	{
		T asset = ScriptableObject.CreateInstance<T> ();
		string assetPathAndName;
		
		if(path == "")
		{
			path = AssetDatabase.GetAssetPath (Selection.activeObject);
			if (path == "") 
			{
				path = "Assets";
			} 
			else if (Path.GetExtension (path) != "") 
			{
				path = path.Replace (Path.GetFileName (AssetDatabase.GetAssetPath (Selection.activeObject)), "");
			}
				
			assetPathAndName = AssetDatabase.GenerateUniqueAssetPath (path + "/New " + typeof(T).ToString() + ".asset");
			Debug.Log (assetPathAndName);
			AssetDatabase.CreateAsset (asset, assetPathAndName);
			
			AssetDatabase.SaveAssets ();
			AssetDatabase.Refresh();
			EditorUtility.FocusProjectWindow ();
			Selection.activeObject = asset;
		}
		else
		{
			assetPathAndName = AssetDatabase.GenerateUniqueAssetPath (path );
			
			AssetDatabase.CreateAsset (asset, assetPathAndName);
			
			AssetDatabase.SaveAssets ();
			AssetDatabase.Refresh();
			
		}
		
		
	}
	
	public static T LoadAsset<T> (string path) where T : ScriptableObject
	{
		if (Path.GetExtension (path) == "")
		{
				path += ".asset";
		}
		T asset = AssetDatabase.LoadAssetAtPath(path, typeof(T)) as T;
		if(asset == null)
		{
			ScriptableObjectUtility.CreateAsset<T> (path);
			asset = AssetDatabase.LoadAssetAtPath(path, typeof(T)) as T;
			if(asset == null)
			{
				Debug.LogError ("Problem with asset creation: " + path);
			}
		}
		return asset;
	}
}