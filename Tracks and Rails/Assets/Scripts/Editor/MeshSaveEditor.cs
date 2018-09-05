using UnityEditor;

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class MeshSaverEditor
{

    [MenuItem("CONTEXT/MeshFilter/Save Mesh...")]
    public static void SaveMeshInPlace(MenuCommand menuCommand)
    {
        MeshFilter mf = menuCommand.context as MeshFilter;
        Mesh m = mf.sharedMesh;
        SaveMesh(m, m.name, false, true);
    }

    [MenuItem("CONTEXT/MeshFilter/Save Mesh As New Instance...")]
    public static void SaveMeshNewInstanceItem(MenuCommand menuCommand)
    {
        MeshFilter mf = menuCommand.context as MeshFilter;
        Mesh m = mf.sharedMesh;
        SaveMesh(m, m.name, true, true);
    }

    public static void SaveMesh(Mesh mesh, string name, bool makeNewInstance, bool optimizeMesh)
    {
        string path = EditorUtility.SaveFilePanel("Save Separate Mesh Asset", "Assets/", name, "asset");
        if (string.IsNullOrEmpty(path)) return;

        path = FileUtil.GetProjectRelativePath(path);

        Mesh meshToSave = (makeNewInstance) ? Object.Instantiate(mesh) as Mesh : mesh;

        if (optimizeMesh)
            meshToSave.Optimize();

        Mesh asset = AssetDatabase.LoadAssetAtPath<Mesh>(path);
        if (asset != null)
        {
            CopyMesh(meshToSave, asset);
        }
        else
        {
            asset = new Mesh();
            AssetDatabase.CreateAsset(meshToSave, path);
        }
        AssetDatabase.SaveAssets();
    }

    public static void CopyMesh(Mesh meshSource, Mesh meshDestination)
    {
        if(meshDestination.vertexCount <= meshSource.vertexCount)
            meshDestination.vertices = meshSource.vertices;
        meshDestination.triangles = meshSource.triangles;
        if(meshDestination.vertexCount > meshSource.vertexCount)
            meshDestination.vertices = meshSource.vertices;
        meshDestination.uv = meshSource.uv;
        meshDestination.normals = meshSource.normals;
        meshDestination.colors = meshSource.colors;
        meshDestination.tangents = meshSource.tangents;
    }

}