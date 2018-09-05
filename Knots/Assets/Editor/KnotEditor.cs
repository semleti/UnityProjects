using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(Knot))]
[ExecuteInEditMode]
public class KnotEditor : Editor
{
	[MenuItem("GameObject/3D Object/Knot")]
	static void createKnot()
	{
		GameObject go = new GameObject ();
		go.transform.position = Vector3.zero;
		go.AddComponent (typeof(Knot));
		go.name = "Knot";
	}

	static public void OnEnable()
	{

	}

	public override void OnInspectorGUI()
	{
		Knot myTarget = (Knot)target;
		//base.OnInspectorGUI ();
		Draw (myTarget, 0);
		base.OnInspectorGUI ();
	}

	static public void Draw(Knot target, int index)
	{
		target.prefab = (GameObject)EditorGUILayout.ObjectField ("prefab:", target.prefab, typeof(GameObject) );
		int i = 0;
		int indexToInsert = -1;
		int indexToRemove = -1;
		foreach (GameObject go in target.points) {
			GUILayout.BeginHorizontal();
			string button = "";
			if(i==index)
			{
				button = " ->";
			}
			button += go.name;
			if(GUILayout.Button(button))
			{
				Selection.activeGameObject = go;
			}
			if(GUILayout.Button("+",GUILayout.Width(20)))
			{
				indexToInsert = i;
			}
			if(GUILayout.Button("-",GUILayout.Width(20)))
			{
				indexToRemove = i;
			}
			GUILayout.EndHorizontal();
			i++;
		}
		if (indexToInsert > -1) {
			AddPoint (target, indexToInsert+1);
		}
		if (indexToRemove > -1) {
			RemovePoint (target, indexToRemove);
		}

		GUILayout.BeginHorizontal();
		if (GUILayout.Button ("add")) {
			AddPoint(target,target.points.Count);
		}
		if (GUILayout.Button ("remove")) {
			RemovePoint(target,target.points.Count-1);
		}
		GUILayout.EndHorizontal();

		bool tempBool;
		tempBool= target.drawFlatBool;
		target.drawFlatBool = GUILayout.Toggle (target.drawFlatBool, "draw flat");
		if (tempBool != target.drawFlatBool) {
			EditorUtility.SetDirty(target);
		}
		tempBool = target.drawFlatBool;
		target.draw3DCrossingBool = GUILayout.Toggle (target.draw3DCrossingBool, "draw 3D Cross");

		/*serializedObject.Update();
		serializedObject.ApplyModifiedProperties();*/

	}

	public static void AddPoint(Knot target, int index)
	{
		GameObject newGo;
		if(target.prefab==null)
		{
			newGo = GameObject.CreatePrimitive(PrimitiveType.Cube);
			newGo.GetComponent<MeshFilter>().hideFlags = HideFlags.HideInInspector;
			newGo.GetComponent<MeshRenderer>().hideFlags = HideFlags.HideInInspector;
			newGo.GetComponent<Collider>().hideFlags = HideFlags.HideInInspector;
			newGo.hideFlags = HideFlags.HideInHierarchy;
		}
		else
		{
			newGo = Instantiate(target.prefab);
		}
		newGo.transform.SetParent(target.transform);
		if (target.points.Count == 0) {
			newGo.transform.localPosition = new Vector3 (0, 0, 0);
		} else {
			newGo.transform.localPosition = target.points[(index-1)%target.points.Count].transform.localPosition;
		}
		newGo.AddComponent(typeof(Point));
		((Point)newGo.GetComponent(typeof(Point))).knotParent = target;
		if(target.prefab==null)
		{
			newGo.transform.localScale = new Vector3(0.1f,0.1f,0.1f);
		}
		if (target.points.Count < index) {
			target.points.Add (newGo);
		} else {
			target.points.Insert(index,newGo);
		}
		Selection.activeGameObject = newGo.gameObject;
	}

	public static void RemovePoint(Knot target, int index)
	{
		DestroyImmediate(target.points[index]);
		target.points.RemoveAt(index);
		if(target.points.Count>0)
		{
			if(index>=target.points.Count)
			{
				index = target.points.Count-1;
			}
			Selection.activeGameObject = target.points[index].gameObject;
		}
		else
		{
			Selection.activeGameObject = target.gameObject;
		}
	}
	
}
