using UnityEngine;
using UnityEditor;

using System.IO;
using System.Reflection;

using Type = System.Type;

public static class LayoutUtility {
	private static MethodInfo _miLoadWindowLayout;
	private static MethodInfo _miSaveWindowLayout;
	private static MethodInfo _miReloadWindowLayoutMenu;
	
	private static bool _available;
	private static string _layoutsPath;
	
	static LayoutUtility() {
		
		Type tyWindowLayout = Type.GetType("UnityEditor.WindowLayout,UnityEditor");
		Type tyEditorUtility = Type.GetType("UnityEditor.EditorUtility,UnityEditor");
		Type tyInternalEditorUtility = Type.GetType("UnityEditorInternal.InternalEditorUtility,UnityEditor");
		
		if (tyWindowLayout != null && tyEditorUtility != null && tyInternalEditorUtility != null) {
			foreach (MethodInfo info in tyEditorUtility.GetMethods())
			{
				Debug.Log (info.ToString() + " static?: " + info.IsStatic + "public?: " + info.IsPublic);
			}
			
			//method not present anymore
			//MethodInfo miGetLayoutsPath = tyWindowLayout.GetMethod("GetLayoutsPath", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);
			
			#if UNITY_EDITOR_WIN
			_layoutsPath = System.Environment.ExpandEnvironmentVariables("%appdata%/Unity/Editor-5.x/Preferences/Layouts/").Replace("\\","/");
			#endif
			#if UNITY_EDITOR_OSX
			_layoutsPath = System.Environment.ExpandEnvironmentVariables("~/Library/Preferences/Unity/Editor-5.x/Layouts/").Replace("\\","/");
			#endif
			
			_miLoadWindowLayout = tyWindowLayout.GetMethod("LoadWindowLayout", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static, null, new Type[] { typeof(string), typeof(bool) }, null);
			_miSaveWindowLayout = tyWindowLayout.GetMethod("SaveWindowLayout", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static, null, new Type[] { typeof(string) }, null);
			_miReloadWindowLayoutMenu = tyInternalEditorUtility.GetMethod("ReloadWindowLayoutMenu", BindingFlags.Public | BindingFlags.Static);

			if (_miLoadWindowLayout == null || _miSaveWindowLayout == null || _miReloadWindowLayoutMenu == null)
			{
				return;
			}
			
			_available = true;
		}
	}
	
	// Gets a value indicating whether all required Unity API
	// functionality is available for usage.
	public static bool IsAvailable {
		get { return _available; }
	}
	
	// Gets absolute path of layouts directory.
	// Returns `null` when not available.
	public static string LayoutsPath {
		get { return _layoutsPath; }
	}
	
	// Save current window layout to asset file.
	// `assetPath` must be relative to project directory.
	public static void SaveLayoutToAsset(string assetPath) {
		SaveLayout(Path.Combine(Directory.GetCurrentDirectory(), assetPath));
	}
	
	// Load window layout from asset file.
	// `assetPath` must be relative to project directory.
	public static void LoadLayoutFromAsset(string assetPath) {
		if (_miLoadWindowLayout != null) {
			string path = Path.Combine(Directory.GetCurrentDirectory(), assetPath);
			_miLoadWindowLayout.Invoke(null, new object[] { path, true });
		}

	}
	
	// Save current window layout to file.
	// `path` must be absolute.
	public static void SaveLayout(string path) {
		if (_miSaveWindowLayout != null)
		{
			_miSaveWindowLayout.Invoke(null, new object[] { path});
		}

	}
	
}





