using UnityEditor;
using UnityEngine;
using System.IO;
using System.Linq;

/*
Author: Benjamin Schaaf
Rewritten to C#: semleti
*/
class BlenderAssetProcessor : AssetPostprocessor
{
    //After an asset was imported, but before it is saved to disk
    public void OnPostprocessModel(GameObject obj) {

        //only perform corrections with blender files
        ModelImporter importer = assetImporter as ModelImporter;
        if (Path.GetExtension(importer.assetPath) == ".blend" || Path.GetExtension(importer.assetPath) == ".fbx")
        {
            RotateObject(obj.transform);
        }

        //Don't know why we need this...
        //Fixes wrong parent rotation
        obj.transform.rotation = Quaternion.identity;
    }

    //recursively rotate a object tree individualy
    private void RotateObject(Transform obj) {
        obj.rotation = Quaternion.Euler(obj.eulerAngles.x + 90, obj.eulerAngles.y, obj.eulerAngles.z);

        //if a meshFilter is attached, we rotate the vertex mesh data
        MeshFilter meshFilter = obj.GetComponent<MeshFilter>();
        if (meshFilter)
        {
            RotateMesh(meshFilter.sharedMesh);
        }

        //do this too for all our children
        foreach (Transform child in obj.transform)
        {
            RotateObject(child);
        }
    }

    //"rotate" the mesh data
    private void RotateMesh(Mesh mesh){
        int index = 0;

        //switch all vertex z values with y values
        Vector3[] vertices = mesh.vertices;
        for (index = 0; index < vertices.Length; index++)
        {
            vertices[index] = new Vector3(vertices[index].x, vertices[index].z, vertices[index].y);
        }
        mesh.vertices = vertices;

        //for each submesh, we invert the order of vertices for all triangles
        //for some reason changing the vertex positions flips all the normals???
        for (int submesh = 0; submesh < mesh.subMeshCount; submesh++) {
            int[] triangles = mesh.GetTriangles(submesh);
            for (index = 0; index < triangles.Length; index += 3)
            {
                int intermediate = triangles[index];
                triangles[index] = triangles[index + 2];
                triangles[index + 2] = intermediate;
            }
            mesh.SetTriangles(triangles, submesh);
        }

        //recalculate other relevant mesh data
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }
}