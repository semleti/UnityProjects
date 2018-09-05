using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public delegate void VoidDelegate ();

[CustomEditor(typeof(GameObject))]
public class OnSceneGUICatcher : Editor
{
	public static List<VoidDelegate> list = new List<VoidDelegate>();
	[DrawGizmo(GizmoType.NotInSelectionHierarchy)]
	static void RenderCustomGizmo(Transform objectTransform, GizmoType gizmoType)
	{
		foreach (VoidDelegate voidDelegate in list) {
			voidDelegate();
		}
	}
}
