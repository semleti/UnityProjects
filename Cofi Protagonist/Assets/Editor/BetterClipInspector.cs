using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BetterAnimationClip))]
public class BetterClipInspector : Editor
{

	public override void OnInspectorGUI()
    {
        BetterAnimationClip targ = (BetterAnimationClip)target;
        base.OnInspectorGUI();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("m_i"), true);
        Transform t = targ.gameObject.transform;
        EditorGUI.indentLevel = 0;
        SerializedProperty ser = serializedObject.GetIterator();
        ser.Next(true);
        while(ser.Next(true))
        {
            EditorGUI.indentLevel = ser.depth;
            EditorGUILayout.PropertyField(ser, true);
        }
        EditorGUI.indentLevel = 0;
        Vector3 position = EditorGUILayout.Vector3Field("Position", t.localPosition);
        Vector3 eulerAngles = EditorGUILayout.Vector3Field("Rotation", t.localEulerAngles);
        Vector3 scale = EditorGUILayout.Vector3Field("Scale", t.localScale);

        if (GUI.changed)
        {
            Undo.RegisterCompleteObjectUndo(t,"Transform Change");

            t.localPosition = FixIfNaN(position);
            t.localEulerAngles = FixIfNaN(eulerAngles);
            t.localScale = FixIfNaN(scale);
            targ.enabled = false;
        }


        GUILayout.Space(50);

        Component[] comps = targ.GetComponents<Component>();
        foreach(Component comp in comps)
        {
            EditorGUILayout.LabelField(comp.GetType().ToString(), GUILayout.Height(20));
            Type componentType = comp.GetType();
            PropertyInfo[] propertyInfos = componentType.GetProperties();
            EditorGUILayout.BeginVertical();
            SerializedObject sComp = new SerializedObject(comp);
            SerializedProperty it = sComp.GetIterator();
            /*do
            {
                EditorGUILayout.LabelField(it.name, GUILayout.Height(20));
                EditorGUILayout.PropertyField(it);

            } while (it.Next(true));*/
            it.NextVisible(true);
            do
            {
                try
                {
                    //EditorGUILayout.LabelField(it.displayName, GUILayout.Height(20));
                    EditorGUILayout.PropertyField(it);
                    EditorGUILayout.Space();
                }
                catch (Exception e) { }
                //Debug.Log(it.name);
            } while (it.NextVisible(false)) ;
                EditorGUILayout.Space();
            //Debug.Log("------------------------------------");
            /*foreach(var prop in propertyInfos)
            {
                
                if (prop.PropertyType.IsSerializable)
                {
                    //Debug.Log(prop.Name);
                    EditorGUILayout.LabelField(prop.Name, GUILayout.Height(20));
                    SerializedProperty sPop = sComp.FindProperty(prop.Name);
                    if (sPop == null)
                        sPop = sComp.FindProperty("m_" + prop.Name);
                    if (sPop != null)
                        EditorGUILayout.PropertyField(sPop);
                }
            }*/
            sComp.ApplyModifiedProperties();
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space();

            //Debug.Log("#####################################");
        }
    }

    private Vector3 FixIfNaN(Vector3 v)
    {
        if (float.IsNaN(v.x))
        {
            v.x = 0;
        }
        if (float.IsNaN(v.y))
        {
            v.y = 0;
        }
        if (float.IsNaN(v.z))
        {
            v.z = 0;
        }
        return v;
    }
}
