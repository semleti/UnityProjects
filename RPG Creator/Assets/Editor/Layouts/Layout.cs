using UnityEngine;
using UnityEditor;

public static class Layout {
	
	[MenuItem("Layout Hack/Save Layout")]
	static void SaveLayoutHack() {
		// Saving the current layout to an asset
		LayoutUtility.SaveLayout(LayoutUtility.LayoutsPath + "Custom Layout.wlt");
		
		//couldn't find a way to refresh Layers dropdownmenu
		//EditorApplication.update();
	}
	
	[MenuItem("Layout Hack/Load Layout")]
	static void LoadLayoutHack() {
		// Loading layout from an asset
		EditorApplication.ExecuteMenuItem("Window/Layouts/Custom layout");
	}
	
}





