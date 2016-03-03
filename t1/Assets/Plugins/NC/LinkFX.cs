using UnityEngine;
using System.Collections;

public class LinkFX : MonoBehaviour 
{
    public Transform from;
    public Transform to;
    public float width;
    public bool faceCamera;
    public Material mat;
    private Mesh _mesh;
    public Color[] _meshColors = new Color[4];
    Vector2[] _uvs;
    int[] _triangles;
	void Start () 
    {
        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
        _mesh = meshFilter.mesh;
        gameObject.AddComponent<MeshRenderer>().material = mat;
        _uvs = new Vector2[4];
        _uvs[0] = new Vector2(0, 0);
        _uvs[1] = new Vector2(0, 1);
        _uvs[2] = new Vector2(1, 0);
        _uvs[3] = new Vector2(1, 1);
        _triangles = new int[6];
        _triangles[0] = 0;
        _triangles[1] = 1;
        _triangles[2] = 2;
        _triangles[3] = 1;
        _triangles[4] = 3;
        _triangles[5] = 2;
	}
	
	void Update () 
    {
        if (from != null && to != null)
        {

            Vector3[] _vertices = new Vector3[4];

            Vector3 perpendicular;
            Vector3 pointDir = to.position - from.position;

            Vector3 vectorToCamera = Camera.main.transform.position - (to.position + from.position) / 2;
            if (faceCamera)
            {
               Vector3 dir = Vector3.Dot(pointDir.normalized, vectorToCamera) * pointDir.normalized + vectorToCamera;
               perpendicular = Vector3.Cross(pointDir, dir).normalized;
            }
            else
            {
                perpendicular = Vector3.Cross(pointDir, Vector3.up).normalized;
            }
            _vertices[0] = from.position + perpendicular * width * 0.5f;
            _vertices[1] = to.position + perpendicular * width * 0.5f;
            _vertices[2] = from.position - perpendicular * width * 0.5f;
            _vertices[3] = to.position - perpendicular * width * 0.5f;
            _mesh.Clear();
            _mesh.vertices = _vertices;
            _mesh.colors = _meshColors;
            _mesh.uv = _uvs;
            _mesh.triangles = _triangles;
        }
	}
}
