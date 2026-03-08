using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering;
//*******************************************************************************//
//  https://discussions.unity.com/t/stealth-game-field-of-view-issue/691749/5
//*******************************************************************************//
public class FieldOfViewMesh : MonoBehaviour
{
    public float viewRadius = 10f;
    [Range(0f, 360f)] public float viewAngle = 90f;
    public LayerMask obstacleMask;
    [Range(0.01f, 2f)] public float meshResolution = 1f;
    public float viewMeshOffset = 0.1f;
    public MeshFilter viewMeshFilter;
    // public MeshCollider viewMeshCollider;

    Mesh mesh;

    [SerializeField] GameObject _fovMesh;    
    [SerializeField] private Material _normalFOVMaterial;
    [SerializeField] private Material _alarmFOVMaterial;
    private Material _initialFOVMaterial;
    private MeshRenderer _fovMeshRenderer;

    [SerializeField] SimpleSFM _enemySFM;

    void Awake()
    {
        if (_enemySFM == null) _enemySFM = GetComponent<SimpleSFM>();
        mesh = new Mesh { name = "FOV Mesh (Simple)" };
        if (!viewMeshFilter) viewMeshFilter = GetComponent<MeshFilter>();
        viewMeshFilter.sharedMesh = mesh;
        //if (viewMeshCollider) viewMeshCollider.sharedMesh = mesh;
        _fovMeshRenderer = _fovMesh.GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        UpdateFOVColor();
    }

    void LateUpdate()
    {
        BuildMesh();
    }

    void BuildMesh()
    {
        int steps = Mathf.Max(1, Mathf.RoundToInt(viewAngle * meshResolution));
        float stepAngle = viewAngle / steps;

        var points = new List<Vector3>(steps + 1);

        for (int i = 0; i <= steps; i++)
        {
            float angle = -viewAngle * 0.5f + stepAngle * i;
            Vector3 dir = DirFromAngle(angle);
            Vector3 point = GetPoint(dir);
            points.Add(point);
        }

        UpdateMesh(points);
    }

    Vector3 DirFromAngle(float localAngleDegrees)
    {
        return Quaternion.Euler(0f, localAngleDegrees, 0f) * transform.forward;
    }

    Vector3 GetPoint(Vector3 dir)
    {
        if (Physics.Raycast(transform.position, dir, out RaycastHit hit, viewRadius, obstacleMask)) return hit.point;
        return transform.position + dir * viewRadius;
    }

    void UpdateMesh(List<Vector3> worldPoints)
    {
        int vertexCount = worldPoints.Count + 1;

        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount - 2) * 3];

        vertices[0] = Vector3.zero;

        for (int i = 0; i < worldPoints.Count; i++)
        {
            vertices[i + 1] = transform.InverseTransformPoint(worldPoints[i]) + Vector3.forward * viewMeshOffset;

            if (i < worldPoints.Count - 1)
            {
                int t = i * 3;
                triangles[t] = 0;
                triangles[t + 1] = i + 1;
                triangles[t + 2] = i + 2;
            }
        }

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

       // if (viewMeshCollider) viewMeshCollider.sharedMesh = mesh;
    }

    public void UpdateFOVColor()
    {
        if (_enemySFM.CanSeeTarget)
        {
            _fovMeshRenderer.material= _alarmFOVMaterial;
        }
        else
        {
            _fovMeshRenderer.material = _normalFOVMaterial;
        }
    }

}