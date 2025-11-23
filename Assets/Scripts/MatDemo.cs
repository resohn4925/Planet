using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class MatDemo : MonoBehaviour
{
    private Mesh mesh;
    private List<Vector3> verts = new();
    private List<Vector2> uvs = new();
    private List<int> triangles = new();

    private void Start()
    {
        mesh = new Mesh();

        verts.Add(new Vector3(0, 0, 0));
        verts.Add(new Vector3(0, 0, 10));
        verts.Add(new Vector3(0, 10, 0));

        uvs.Add(new Vector2(0, 0));
        uvs.Add(new Vector2(1, 0));
        uvs.Add(new Vector2(0, 1));

        triangles.Add(0);
        triangles.Add(2);
        triangles.Add(1);

        mesh.vertices = verts.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.triangles = triangles.ToArray();

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        GetComponent<MeshFilter>().mesh = mesh;
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer.sharedMaterial == null)
        {
            meshRenderer.sharedMaterial = new Material(Shader.Find("Standard"));
        }
    }

    private void OnDrawGizmos()
    {
        if (verts == null || verts.Count < 3)
        return;
        Gizmos.color = Color.white;
        Gizmos.DrawLine(verts[0], verts[1]);
        Gizmos.DrawLine(verts[1], verts[2]);
        Gizmos.DrawLine(verts[2], verts[0]);

        if (mesh != null && mesh.normals != null && mesh.normals.Length == verts.Count)
        {
            for (int i = 0; i < verts.Count; i++)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(verts[i], verts[i] + mesh.normals[i]*2.5f);
            }
        }
    }
}
