using UnityEngine;
using System.Collections;

abstract public class Track : MonoBehaviour {
    public Track previous;
    public Track next;
    public float length;
    public Mesh mesh;
    public Mesh meshCollide;
    new protected MeshCollider collider;
    protected MeshFilter filter;

    public Mesh CreateMesh(Mesh model)
    {
        return model;
    }
    // Use this for initialization
    void Awake () {
        filter = GetComponent<MeshFilter>();
        mesh = filter.mesh;

        collider = GetComponent<MeshCollider>();
        meshCollide = collider.sharedMesh;
    }

    void ApplyCollider()
    {
        collider.sharedMesh = null;
        collider.sharedMesh = meshCollide;
    }

    void ApplyMesh()
    {
        filter.mesh = mesh;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
