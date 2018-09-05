﻿using UnityEditor;
using UnityEngine;
using System;
using System.Reflection;

public class DisableAllGizmos
{
	[MenuItem("Window/Gizmos/Disable All")]
	static void DisableAllGizmosMenu()
	{
		var Annotation = Type.GetType("UnityEditor.Annotation, UnityEditor");
		var ClassId = Annotation.GetField("classID");
		var ScriptClass = Annotation.GetField("scriptClass");
		
		Type AnnotationUtility = Type.GetType("UnityEditor.AnnotationUtility, UnityEditor");
		var GetAnnotations = AnnotationUtility.GetMethod("GetAnnotations", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);
		var SetGizmoEnabled = AnnotationUtility.GetMethod("SetGizmoEnabled", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);
		var SetIconEnabled = AnnotationUtility.GetMethod("SetIconEnabled", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);
		
		Array annotations = (Array)GetAnnotations.Invoke(null, null);
		foreach (var a in annotations)
		{
			int classId = (int)ClassId.GetValue(a);
			string scriptClass = (string)ScriptClass.GetValue(a);
			
			SetGizmoEnabled.Invoke(null, new object[] { classId, scriptClass, 0 });
			SetIconEnabled.Invoke(null, new object[] { classId, scriptClass, 0 });
		}
	}
	[MenuItem("Window/Gizmos/Enable All")]
	static void EnableeAllGizmosMenu()
	{
		var Annotation = Type.GetType("UnityEditor.Annotation, UnityEditor");
		var ClassId = Annotation.GetField("classID");
		var ScriptClass = Annotation.GetField("scriptClass");
		
		Type AnnotationUtility = Type.GetType("UnityEditor.AnnotationUtility, UnityEditor");
		foreach(MethodInfo info in Annotation.GetMethods())
		{
			Debug.Log(info.Name);
		}
		var GetAnnotations = AnnotationUtility.GetMethod("GetAnnotations", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);
		var SetGizmoEnabled = AnnotationUtility.GetMethod("SetGizmoEnabled", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);
		var SetIconEnabled = AnnotationUtility.GetMethod("SetIconEnabled", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);

		Array annotations = (Array)GetAnnotations.Invoke(null, null);
		foreach (var a in annotations)
		{
			int classId = (int)ClassId.GetValue(a);
			string scriptClass = (string)ScriptClass.GetValue(a);
			
			SetGizmoEnabled.Invoke(null, new object[] { classId, scriptClass, 1 });
			SetIconEnabled.Invoke(null, new object[] { classId, scriptClass, 1 });
		}
	}
}