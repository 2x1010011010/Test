using System.Collections.Generic;
using UnityEngine;

public class PartMesh : MonoBehaviour
{
    [SerializeField] private GameObject _gameObject;
    private List<Vector3> _verticies = new List<Vector3>();
    private List<Vector3> _normals = new List<Vector3>();
    private List<List<int>> _triangles = new List<List<int>>();
    private List<Vector2> _uvMaps = new List<Vector2>();

    public Vector3[] Vertices;
    public Vector3[] Normals;
    public int[][] Triangles;
    public Vector2[] UVMaps;
    public Bounds Bounds = new Bounds();

    public GameObject GameObject => _gameObject;


    public void AddTriangle(int submesh, Vector3 vertex1, Vector3 vertex2, Vector3 vertex3, Vector3 normal1, Vector3 normal2, Vector3 normal3, Vector2 uv1, Vector2 uv2, Vector2 uv3)
    {
        if (_triangles.Count - 1 < submesh)
            _triangles.Add(new List<int>());

        _triangles[submesh].Add(_verticies.Count);
        _verticies.Add(vertex1);
        _triangles[submesh].Add(_verticies.Count);
        _verticies.Add(vertex2);
        _triangles[submesh].Add(_verticies.Count);
        _verticies.Add(vertex3);
        _normals.Add(normal1);
        _normals.Add(normal2);
        _normals.Add(normal3);
        _uvMaps.Add(uv1);
        _uvMaps.Add(uv2);
        _uvMaps.Add(uv3);

        Bounds.min = Vector3.Min(Bounds.min, vertex1);
        Bounds.min = Vector3.Min(Bounds.min, vertex2);
        Bounds.min = Vector3.Min(Bounds.min, vertex3);
        Bounds.max = Vector3.Min(Bounds.max, vertex1);
        Bounds.max = Vector3.Min(Bounds.max, vertex2);
        Bounds.max = Vector3.Min(Bounds.max, vertex3);
    }

    public void FillArrays()
    {
        Vertices = _verticies.ToArray();
        Normals = _normals.ToArray();
        UVMaps = _uvMaps.ToArray();
        Triangles = new int[_triangles.Count][];
        for (var i = 0; i < _triangles.Count; i++)
            Triangles[i] = _triangles[i].ToArray();
    }

    public void MakeGameobject(MeshDestroy original)
    {
        _gameObject = new GameObject(original.name);
        _gameObject.transform.position = original.transform.position;
        _gameObject.transform.rotation = original.transform.rotation;
        _gameObject.transform.localScale = original.transform.localScale;

        var mesh = new Mesh();
        mesh.name = original.GetComponent<MeshFilter>().mesh.name;

        mesh.vertices = Vertices;
        mesh.normals = Normals;
        mesh.uv = UVMaps;
        for (var i = 0; i < Triangles.Length; i++)
            mesh.SetTriangles(Triangles[i], i, true);
        Bounds = mesh.bounds;

        var renderer = _gameObject.AddComponent<MeshRenderer>();
        renderer.materials = original.GetComponent<MeshRenderer>().materials;

        var filter = _gameObject.AddComponent<MeshFilter>();
        filter.mesh = mesh;

        var collider = _gameObject.AddComponent<MeshCollider>();
        collider.convex = true;

        var rigidbody = _gameObject.AddComponent<Rigidbody>();
        var meshDestroy = _gameObject.AddComponent<MeshDestroy>();
        var collision = _gameObject.AddComponent<CollisionHandler>();

        meshDestroy.SetCutCascades(original.CutCascades);
        meshDestroy.SetExplodeForce(original.ExplodeForce);
    }
}

