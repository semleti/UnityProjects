using UnityEngine;
using UnityEditor;

public class YourClassAsset
{
	[MenuItem("Assets/Create/YourClass")]
	public static void CreateAsset ()
	{
		ScriptableObjectUtility.CreateAsset<ScriptableObject> ();
	}
}