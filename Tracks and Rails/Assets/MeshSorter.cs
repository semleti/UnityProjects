using UnityEngine;
using System;

[ExecuteInEditMode]
public class MeshSorter : MonoBehaviour {
    public Mesh mesh;
    public Vector3 direction;
    public StaticMesh staticMesh;
    public Vector3 translation;
    public Vector3 rotation;
#if UNITY_EDITOR
    public bool update;
#endif
    // Use this for initialization
    void Start () {
	    
	}
	
	// Update is called once per frame
	void Update ()
    {
        Debug.Log(Vector3.forward);
        #if UNITY_EDITOR
        if(update)
        {
            staticMesh.SetMesh(mesh);
            staticMesh.CopyFromMesh();
            staticMesh.Translate(translation);
            staticMesh.Rotate(rotation);
            staticMesh.ApplyVertices();
            update = false;
        }
        #endif
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawMesh(staticMesh, transform.position, transform.rotation,transform.localScale);
    }

    public void SortMesh()
    {
        int[] indices = new int[staticMesh.vertexCount];//used to reorder vertices, normals, tangents, uvs, colors, colors32
        for (int i = 0; i < indices.Length; i++)
            indices[i] = i;

        Array.Sort(indices,ArrayCompare);

        Reorder(staticMesh.vertices, indices);
        Reorder(staticMesh.colors, indices);
        Reorder(staticMesh.colors32, indices);
        Reorder(staticMesh.normals, indices);
        Reorder(staticMesh.tangents, indices);
        Reorder(staticMesh.uv, indices);
        Reorder(staticMesh.uv2, indices);
        Reorder(staticMesh.uv3, indices);
        Reorder(staticMesh.uv4, indices);

        int[] invertedIndices = new int[indices.Length];
        for (int i = 0; i < indices.Length; i++)//used to change index values for triangles, still have to reorder triangles
            invertedIndices[indices[i]] = i;

        for (int i = 0; i < staticMesh.triangles.Length; i++)
            staticMesh.triangles[i] = invertedIndices[staticMesh.triangles[i]];

        for(int i = 0; i < staticMesh.triangles.Length; i+=3){//rotates the triangles so that the point with smallest z is first
            if(staticMesh.triangles[i+1] < staticMesh.triangles[i] || staticMesh.triangles[i+2] < staticMesh.triangles[i]){
                if(staticMesh.triangles[i+1] < staticMesh.triangles[i+2]){
                    int temp = staticMesh.triangles[i];
                    staticMesh.triangles[i] = staticMesh.triangles[i + 1];
                    staticMesh.triangles[i + 1] = staticMesh.triangles[i + 2];
                    staticMesh.triangles[i + 2] = temp;
                }else{
                    int temp = staticMesh.triangles[i + 2];
                    staticMesh.triangles[i + 2] = staticMesh.triangles[i + 1];
                    staticMesh.triangles[i + 1] = staticMesh.triangles[i];
                    staticMesh.triangles[i] = temp;
                }
            }
        }

        int[] triangleIndices = new int[staticMesh.triangles.Length / 3];//used to reorder vertices, normals, tangents, uvs, colors, colors32
        for (int i = 0; i < indices.Length; i++)
            triangleIndices[i] = i*3;

        Array.Sort(triangleIndices, TrianglesCompare);

        TriangleReorder(triangleIndices);
    }

    int ArrayCompare(int storedIndex1, int storedIndex2)
    {
        return staticMesh.vertices[storedIndex1].z.CompareTo(staticMesh.vertices[storedIndex2].z);
    }

    int TrianglesCompare(int storedIndex1, int storedIndex2)
    {
        if (staticMesh.triangles[storedIndex1] < staticMesh.triangles[storedIndex2])
            return -1;
        else if (staticMesh.triangles[storedIndex1] > staticMesh.triangles[storedIndex2])
            return 1;
        else if ((staticMesh.triangles[storedIndex1 + 1] < staticMesh.triangles[storedIndex2 + 1] || staticMesh.triangles[storedIndex1 + 1] < staticMesh.triangles[storedIndex2 + 2])
            && (staticMesh.triangles[storedIndex1 + 2] < staticMesh.triangles[storedIndex2 + 1] || staticMesh.triangles[storedIndex1 + 2] < staticMesh.triangles[storedIndex2 + 2]))
            return 11;
        else if ((staticMesh.triangles[storedIndex1 + 1] > staticMesh.triangles[storedIndex2 + 1] && staticMesh.triangles[storedIndex1 + 1] > staticMesh.triangles[storedIndex2 + 2])
            || (staticMesh.triangles[storedIndex1 + 2] > staticMesh.triangles[storedIndex2 + 1] && staticMesh.triangles[storedIndex1 + 2] > staticMesh.triangles[storedIndex2 + 2]))
            return 1;
        return 0;
    }

    T[] Reorder<T>(T[] array, int[] indices)
    {
        T[] arrayToReturn = new T[array.Length];
        for (int i = 0; i < arrayToReturn.Length; i++)
            arrayToReturn[i] = array[indices[i]];
        return arrayToReturn;
    }

    void TriangleReorder(int[] indices)
    {
        int[] temp = new int[staticMesh.triangles.Length];
        for(int i=0; i < indices.Length; i++)
        {
            temp[i*3] = staticMesh.triangles[indices[i]];
            temp[i*3+1] = staticMesh.triangles[indices[i]+1];
            temp[i*3+2] = staticMesh.triangles[indices[i]+2];
        }
    }
}
