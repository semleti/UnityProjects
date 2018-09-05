using UnityEngine;
using System.Collections;

public class StaticMesh : MonoBehaviour {
    Matrix4x4[] _bindposes;
    int _blendShapeCount;
    BoneWeight[] _boneWeights;
    Bounds _bounds;
    Color[] _colors;
    Color32[] _colors32;
    bool _isReadable;
    Vector3[] _normals;
    int _subMeshCount;
    Vector4[] _tangents;
    int[] _triangles;
    Vector2[] _uv;
    Vector2[] _uv2;
    Vector2[] _uv3;
    Vector2[] _uv4;
    int _vertexCount;
    Vector3[] _vertices;

    /*public bool bindposesChanged;
    public bool boneWeightsChanged;
    public bool boundsChanged;
    public bool colorsChanged;
    public bool colors32Changed;
    public bool normalsChanged;
    public bool subMeshCountChanged;
    public bool tangentsChanged;
    public bool trianglesChanged;
    public bool uvChanged;
    public bool uv2Changed;
    public bool uv3Changed;
    public bool uv4Changed;
    public bool verticesChanged;*/

    Mesh mesh;

    public Matrix4x4[] bindposes {
        get { return _bindposes; }
        set { _bindposes = value; /*bindposesChanged = true;*/ }
    }
    public int blendShapeCount {
        get { return _blendShapeCount; }
    }
    public BoneWeight[] boneWeights {
        get { return _boneWeights; }
        set { _boneWeights = value; /*boneWeightsChanged = true;*/ }
    }
    public Bounds bounds {
        get { return _bounds; }
        set { _bounds = value; /*boundsChanged = true;*/ }
    }
    public Color[] colors {
        get { return _colors; }
        set { _colors = value; /*colorsChanged = true;*/ }
    }
    public Color32[] colors32 {
        get { return _colors32; }
        set { _colors32 = value; /*colors32Changed = true;*/ }
    }
    public bool isReadable {
        get { return _isReadable; }
    }
    public Vector3[] normals {
        get { return _normals; }
        set { _normals = value; /*normalsChanged = true;*/ }
    }
    public int submeshCount {
        get { return _subMeshCount; }
        set { _subMeshCount = value; /*subMeshCountChanged = true;*/ }
    }
    public Vector4[] tangents {
        get { return _tangents; }
        set { _tangents = value; /*tangentsChanged = true;*/ }
    }
    public int[] triangles {
        get { return _triangles; }
        set { _triangles = value; /*trianglesChanged = true;*/ }
    }
    public Vector2[] uv {
        get { return _uv; }
        set { _uv = value; /*uvChanged = true;*/ }
    }
    public Vector2[] uv2 {
        get { return _uv2; }
        set { _uv2 = value; /*uv2Changed = true;*/ }
    }
    public Vector2[] uv3 {
        get { return _uv3; }
        set { _uv3 = value; /*uv3Changed = true;*/ }
    }
    public Vector2[] uv4 {
        get { return _uv4; }
        set { _uv4 = value; /*uv4Changed = true;*/ }
    }
    public int vertexCount {
        get { return _vertexCount; }
    }
    public Vector3[] vertices {
        get { return _vertices; }
        set { _vertices = value; /*verticesChanged = true;*/ }
    }

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void SetMesh(Mesh mesh)
    {
        this.mesh = mesh;
    }

    public void CopyFromMesh()
    {
        _bindposes = mesh.bindposes;
        _blendShapeCount = mesh.blendShapeCount;
        _boneWeights = mesh.boneWeights;
        _bounds = mesh.bounds;
        _colors = mesh.colors;
        _colors32 = mesh.colors32;
        _isReadable = mesh.isReadable;
        _normals = mesh.normals;
        _subMeshCount = mesh.subMeshCount;
        _tangents = mesh.tangents;
        _triangles = mesh.triangles;
        _uv = mesh.uv;
        _uv2 = mesh.uv2;
        _uv3 = mesh.uv3;
        _uv4 = mesh.uv4;
        _vertexCount = mesh.vertexCount;
        _vertices = mesh.vertices;

         /*bindposesChanged = false;
         boneWeightsChanged = false;
         boundsChanged = false;
         colorsChanged = false;
         colors32Changed = false;
         normalsChanged = false;
         subMeshCountChanged = false;
         tangentsChanged = false;
         trianglesChanged = false;
         uvChanged = false;
         uv2Changed = false;
         uv3Changed = false;
         uv4Changed = false;
         verticesChanged = false;*/
}

    public void ApplyAll()
    {
        /*if (bindposesChanged) { mesh.bindposes = _bindposes; bindposesChanged = false; }
        if (boneWeightsChanged) { mesh.boneWeights = _boneWeights; boneWeightsChanged = false; }
        if (boundsChanged) { mesh.bounds = _bounds; boundsChanged = false; }
        if (colorsChanged) { mesh.colors = _colors; colorsChanged = false; }
        if (colors32Changed) { mesh.colors32 = _colors32; colors32Changed = false; }
        if (normalsChanged) { mesh.normals = _normals; normalsChanged = false; }
        if (subMeshCountChanged) { mesh.subMeshCount = _subMeshCount; subMeshCountChanged = false; }
        if (tangentsChanged) { mesh.tangents = _tangents; tangentsChanged = false; }
        if (trianglesChanged) { mesh.triangles = _triangles; trianglesChanged = false; }
        if (uvChanged) { mesh.uv = _uv; uvChanged = false; }
        if (uv2Changed) { mesh.uv2 = _uv2; uv2Changed = false; }
        if (uv3Changed) { mesh.uv3 = _uv3; uv3Changed = false; }
        if (uv4Changed) { mesh.uv4 = _uv4; uv4Changed = false; }
        if (verticesChanged) { mesh.vertices = _vertices; verticesChanged = false; }*/
        mesh.bindposes = _bindposes;
        mesh.boneWeights = _boneWeights;
        mesh.bounds = _bounds;
        mesh.colors = _colors;
        mesh.colors32 = _colors32;
        mesh.normals = _normals;
        mesh.subMeshCount = _subMeshCount;
        mesh.tangents = _tangents;
        mesh.triangles = _triangles;
        mesh.uv = _uv;
        mesh.uv2 = _uv2;
        mesh.uv3 = _uv3;
        mesh.uv4 = _uv4;
        mesh.vertices = _vertices;
    }

    public void ApplyBindposes() {
        /*if (bindposesChanged) { mesh.bindposes = _bindposes; bindposesChanged = false; }*/
        mesh.bindposes = _bindposes;
    }
    public void ApplyBoneWeights() {
        /*if (boneWeightsChanged) { mesh.boneWeights = _boneWeights; boneWeightsChanged = false; }*/
        mesh.boneWeights = _boneWeights;
    }
    public void ApplyBounds() {
        /*if (boundsChanged) { mesh.bounds = _bounds; boundsChanged = false; }*/
        mesh.bounds = _bounds;
    }
    public void ApplyColors() {
        /*if (colorsChanged) { mesh.colors = _colors; colorsChanged = false; }*/
        mesh.colors = _colors;
    }
    public void ApplyColors32() {
        /*if (colors32Changed) { mesh.colors32 = _colors32; colors32Changed = false; }*/
        mesh.colors32 = _colors32;
    }
    public void ApplyNormals() {
        /*if (normalsChanged) { mesh.normals = _normals; normalsChanged = false; }*/
        mesh.normals = _normals;
    }
    public void ApplySubMeshCount() {
        /*if (subMeshCountChanged) { mesh.subMeshCount = _subMeshCount; subMeshCountChanged = false; }*/
        mesh.subMeshCount = _subMeshCount;
    }
    public void ApplyTangents() {
        /*if (tangentsChanged) { mesh.tangents = _tangents; tangentsChanged = false; }*/
        mesh.tangents = _tangents;
    }
    public void ApplyTriangles() {
        /*if (trianglesChanged) { mesh.triangles = _triangles; trianglesChanged = false; }*/
        mesh.triangles = _triangles;
    }
    public void ApplyUv() {
        /*if (uvChanged) { mesh.uv = _uv; uvChanged = false; }*/
        mesh.uv = _uv;
    }
    public void ApplyUv2() {
        /*if (uv2Changed) { mesh.uv2 = _uv2; uv2Changed = false; }*/
        mesh.uv2 = _uv2;
    }
    public void ApplyUv3() {
        /*if (uv3Changed) { mesh.uv3 = _uv3; uv3Changed = false; }*/
        mesh.uv3 = _uv3;
    }
    public void ApplyUv4() {
        /*if (uv4Changed) { mesh.uv4 = _uv4; uv4Changed = false; }*/
        mesh.uv4 = _uv4;
    }
    public void ApplyVertices() {
        /*if (verticesChanged) { mesh.vertices = _vertices; verticesChanged = false; }*/
        mesh.vertices = _vertices;
    }

    public void Translate(Vector3 translation)
    {
        for (int i = 0; i < vertices.Length; i++)
            vertices[i] += translation;
    }

    public void Rotate(Vector3 rotation)
    {
        Rotate(Quaternion.Euler(rotation));
    }

    public void Rotate(Quaternion rotation)
    {
        for (int i = 0; i < vertices.Length; i++)
            vertices[i] = rotation * vertices[i];
    }

    public static implicit operator Mesh(StaticMesh staticMesh)
    {
        return staticMesh.mesh;
    }

}
