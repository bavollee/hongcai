using UnityEngine;

using System.Collections;
using System.Collections.Generic;


public class TrailArc : MonoBehaviour
{

    int _savedIndex;

    int _pointIndex;

    public bool emit = true;
    public Material material;
    public bool faceCamera = true;
    public bool dirForward = true;
    public float lifetime = 1; // Lifetime of each segment
    public Color[] colors;
    public float[] widths;
    public int maxSampleCount = 60;
    public float alpha = 1;
    public float pointDistance = 0.5f;
    float _lifeTimeRatio = 1;

    // Optimization
    float _pointSqrDistance = 0;
    float _tRatio;

    // Objects
    GameObject _trail = null;
    Mesh _mesh = null;
    Material _trailMaterial = null;
    Vector3 _lastSamplePoint;

    // Points
    List<Vector3> _saved;
    List<Vector3> _savedUp;
    List<float> _pointsAlpha;

    private static int _trailId;
    void Start()
    {
        _trailId++;
        // Data Inititialization
        _saved = new List<Vector3>();
        _savedUp = new List<Vector3>();
        _pointsAlpha = new List<float>();
        _pointSqrDistance = pointDistance * pointDistance;
        // Create the mesh object
        _trail = new GameObject("Trail" + _trailId);
        _trail.transform.position = Vector3.zero;
        _trail.transform.rotation = Quaternion.identity;
        _trail.transform.localScale = Vector3.one;
        MeshFilter meshFilter = _trail.AddComponent<MeshFilter>();
        _mesh = meshFilter.mesh;
        _trail.AddComponent<MeshRenderer>();
        _trailMaterial = new Material(material);
        _trail.renderer.material = _trailMaterial;
    }

    void Update()
    {
        Vector3 position = transform.position;
        int sampleCount = _saved.Count;
        _pointSqrDistance = pointDistance * pointDistance;
        if (emit)
        {
            if (sampleCount <= 0)
            {
                // Place the first point behind this as a starter projected point
                _saved.Add(transform.TransformPoint(0, 0, -pointDistance));
                _savedUp.Add(transform.up);
                _pointsAlpha.Add(1.0f);
                sampleCount++;
                // Place the second point at the current position
                _saved.Add(position);
                _savedUp.Add(transform.up);
                _pointsAlpha.Add(1.0f);
                sampleCount++;
                // Begin tracking the saved point creation time
                _lastSamplePoint = position;
            }
            // Do we save a new point?
            Vector3 dir = _lastSamplePoint - position;
            float sqrDis = dir.sqrMagnitude;
            if (sqrDis > _pointSqrDistance)
            {
                _saved.Add(position);
                if (dirForward)
                {
                    _savedUp.Add(Quaternion.AngleAxis(90, transform.forward) * dir);
                }
                else
                {
                    _savedUp.Add(transform.up);
                }
              
                _pointsAlpha.Add(1.0f);
                sampleCount++;
                _lastSamplePoint = position;
                if (sampleCount > maxSampleCount)
                {
                    _saved.RemoveAt(0);
                    _savedUp.RemoveAt(0);
                    _pointsAlpha.RemoveAt(0);
                    sampleCount--;
                }
            }
            else if (sqrDis != 0)
            {
                _saved[sampleCount - 1] = position;
                //_savedUp[sampleCount - 1] = transform.up; ;
            }
        }

        _trail.renderer.enabled = true;

        // Common data
        _lifeTimeRatio = 1f / lifetime;
        Color[] meshColors;
        int pointCnt = _saved.Count;
        if (pointCnt <= 0)
            return;
        for(int i = pointCnt -1;i >= 0;i--)
        {
            _pointsAlpha[i] -= (Time.deltaTime * _lifeTimeRatio);
        }
        // Rebuild the mesh
        Vector3[] vertices = new Vector3[pointCnt * 2];
        Vector2[] uvs = new Vector2[pointCnt * 2];
        int[] triangles = new int[(pointCnt - 1) * 6];
        meshColors = new Color[pointCnt * 2];
        float pointRatio = 1f / (pointCnt - 1);

        Vector3 cameraPos = Camera.main.transform.position;

        for (int i = 0; i < pointCnt; i++)
        {

            Vector3 point = _saved[i];

            float ratio = i * pointRatio;
            ratio = Mathf.Sqrt(ratio);


            // Color

            Color color;

            if (colors.Length == 0)

                color = Color.Lerp(Color.clear, Color.white, ratio);

            else if (colors.Length == 1)

                color = Color.Lerp(Color.clear, colors[0], ratio);

            else if (colors.Length == 2)

                color = Color.Lerp(colors[1], colors[0], ratio);

            else
            {

                float colorRatio = colors.Length - 1 - ratio * (colors.Length - 1);

                if (colorRatio == colors.Length - 1)

                    color = colors[colors.Length - 1];

                else
                {

                    int min = (int)Mathf.Floor(colorRatio);

                    float lerp = colorRatio - min;

                    color = Color.Lerp(colors[min + 0], colors[min + 1], lerp);

                }

            }

            color.a *= (_pointsAlpha[i] * alpha);

            meshColors[i * 2] = color;

            meshColors[(i * 2) + 1] = color;


            // Width

            float width;

            if (widths.Length == 0)

                width = 1;

            else if (widths.Length == 1)

                width = widths[0];

            else if (widths.Length == 2)

                width = Mathf.Lerp(widths[1], widths[0], ratio);

            else
            {

                float widthRatio = widths.Length - 1 - ratio * (widths.Length - 1);

                if (widthRatio == widths.Length - 1)

                    width = widths[widths.Length - 1];

                else
                {

                    int min = (int)Mathf.Floor(widthRatio);

                    float lerp = widthRatio - min;

                    width = Mathf.Lerp(widths[min + 0], widths[min + 1], lerp);

                }

            }

            // Vertices

            if (faceCamera)
            {

                Vector3 from = i == pointCnt - 1 ? _saved[i - 1] : point;

                Vector3 to = i == pointCnt - 1 ? point : _saved[i + 1];

                Vector3 pointDir = to - from;

                Vector3 vectorToCamera = cameraPos - point;

                Vector3 perpendicular = Vector3.Cross(pointDir, vectorToCamera).normalized;

                vertices[i * 2 + 0] = point + perpendicular * width * 0.5f;

                vertices[i * 2 + 1] = point - perpendicular * width * 0.5f;

            }

            else
            {

                vertices[i * 2 + 0] = point + _savedUp[i] * width * 0.5f;

                vertices[i * 2 + 1] = point - _savedUp[i] * width * 0.5f;

            }


            // UVs

            uvs[i * 2 + 0] = new Vector2(ratio, 0);

            uvs[i * 2 + 1] = new Vector2(ratio, 1);


            if (i > 0)
            {

                // Triangles

                int triIndex = (i - 1) * 6;

                int vertIndex = i * 2;

                triangles[triIndex + 0] = vertIndex - 2;

                triangles[triIndex + 1] = vertIndex - 1;

                triangles[triIndex + 2] = vertIndex - 0;


                triangles[triIndex + 3] = vertIndex + 0;

                triangles[triIndex + 4] = vertIndex - 1;

                triangles[triIndex + 5] = vertIndex + 1;

            }

        }

        _trail.transform.position = Vector3.zero;

        _trail.transform.rotation = Quaternion.identity;

        _mesh.Clear();

        _mesh.vertices = vertices;

        _mesh.colors = meshColors;

        _mesh.uv = uvs;

        _mesh.triangles = triangles;
    }

    void OnDestroy()
    {
        if (_trail != null)
            Destroy(_trail);
    }

}

