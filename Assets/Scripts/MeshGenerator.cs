using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
[CustomEditor(typeof(MeshGenerator))]
public class MeshGenerator : MonoBehaviour
{
    [Header("网格生成参数")]
    public int meshNum;
    public float meshSize;
    public float sphereRadius;
    public bool isMaptoSphere;

    [Header("可视化参数")]
    public bool showVerts;
    public bool showMeshs;
    public bool showNormals;
    public bool isDebug;
    public float normalLength;

    [Header("Atmos")]
    public GameObject atmos;

    private List<MeshData> meshDataList = new List<MeshData>();

    private bool isRunning;
    private Mesh mesh;
    private GameObject atmosInstance;
    private void Start()
    {
        GenarateMesh();
    }

    /// <summary>
    /// 生成网格面并归一化
    /// </summary>
    /// 
    public void GenarateMesh()
    {
        Init();
        GenerateVertices();
        Normalize();
        GenerateTriangles();
        SetMeshData();
        SetMat();
        GenerateAtmos();
    }

    public void Init()
    {
        isRunning = true;

        mesh = new Mesh();
        mesh.name = "PlaneToSphereMesh";

        CubeFace[] allFaces = (CubeFace[])System.Enum.GetValues(typeof(CubeFace));
        foreach (CubeFace face in allFaces)
        {
            MeshData meshData = new MeshData(face);
            meshDataList.Add(meshData);
        }

        foreach (MeshData var in meshDataList)
        {
            var.ClearData();
        }

        GetComponent<MeshFilter>().mesh = mesh;

        //查询场景中所有atmos并销毁
        GameObject[] existingAtmos = GameObject.FindObjectsOfType<GameObject>();
        foreach (GameObject obj in existingAtmos)
        {
            if (obj != null && obj.name == "Atmos(Clone)")
            {
                if (Application.isPlaying)
                    Destroy(obj);
                else
                    DestroyImmediate(obj);
            }
        }
    }

    private void GenerateVertices()
    {
        foreach (MeshData var in meshDataList)
        {
            var.GenerateVertices(meshNum, meshSize);
        }
    }

    private void Normalize()
    {
        foreach (MeshData var in meshDataList)
        {
            var.Normalize(isMaptoSphere, meshNum, meshSize, sphereRadius);
        }
    }

    private void GenerateTriangles() 
    {
        //generate triangles
        foreach (MeshData var in meshDataList)
        {
            var.GenerateTriangles(meshNum);
        }
    }

    /// <summary>
    /// merge data
    /// </summary>
    private void SetMeshData()
    {
        List<Vector3> allVerts = new();
        List<Vector2> allUVs = new();
        List<Vector3> allNormals = new();
        List<int> allTris = new();

        int vertexOffset = 0;

        //merge data
        foreach (MeshData meshData in meshDataList)
        {
            Debug.Log("set mesh data of " + meshData.faceType);
            {
                allVerts.AddRange(meshData.vertices);
                allUVs.AddRange(meshData.uvs);
                allNormals.AddRange(meshData.normals);
                foreach (int triangleIndex in meshData.triangles)
                {
                    allTris.Add(triangleIndex + vertexOffset);
                }

                vertexOffset += meshData.vertices.Count;
            }
        }

        mesh.vertices = allVerts.ToArray();
        mesh.uv = allUVs.ToArray();
        mesh.triangles = allTris.ToArray();
        mesh.normals = allNormals.ToArray();
        //mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }

    private void OnDrawGizmos()
    {
        if (!isRunning) return;
        if(meshNum <= 0) return;
        //绘制坐标轴
        Gizmos.color = Color.red;
        Gizmos.DrawLine(Vector3.zero, Vector3.right * meshSize * meshNum / 2f);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(Vector3.zero, Vector3.forward * meshSize * meshNum / 2f);

        //绘制vert
        Gizmos.color = Color.yellow;
        if(showVerts)
        foreach (MeshData mesh in meshDataList)
        {
            foreach (Vector3 var in mesh.vertices)
            {
                Gizmos.DrawSphere(var, meshSize / 20f);
            }
        }

        //绘制mesh
        Gizmos.color = Color.white;
        if(showMeshs)
        foreach (MeshData mesh in meshDataList)
        {
            for (int i = 0; i < mesh.triangles.Count; i += 3)
            {
                Vector3 v1 = transform.TransformPoint(mesh.vertices[mesh.triangles[i]]);
                Vector3 v2 = transform.TransformPoint(mesh.vertices[mesh.triangles[i + 1]]);
                Vector3 v3 = transform.TransformPoint(mesh.vertices[mesh.triangles[i + 2]]);

                Gizmos.DrawLine(v1, v2);
                Gizmos.DrawLine(v2, v3);
                Gizmos.DrawLine(v3, v1);
            }
        }

        //绘制debugmesh
        if (isDebug)
        {
            Gizmos.color = Color.green;
            foreach (MeshData mesh in meshDataList)
            {
                for (int i = 0; i < 3; i += 3)
                {
                    Vector3 v1 = transform.TransformPoint(mesh.vertices[mesh.triangles[i]]);
                    Vector3 v2 = transform.TransformPoint(mesh.vertices[mesh.triangles[i + 1]]);
                    Vector3 v3 = transform.TransformPoint(mesh.vertices[mesh.triangles[i + 2]]);

                    Gizmos.DrawLine(v1, v2);
                    Gizmos.DrawLine(v2, v3);
                    Gizmos.DrawLine(v3, v1);
                }
            }
        }

        if (showNormals)
        {
            foreach(MeshData meshData in meshDataList)
            {
                for (int i = 0; i < meshData.vertices.Count; i++)
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawLine(meshData.vertices[i], meshData.vertices[i] + meshData.normals[i] * 10f);
                }
            }
        }
    }

    private void SetMat()
    {
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer.sharedMaterial == null)
        {
            meshRenderer.sharedMaterial = new Material(Shader.Find("Standard"));
        }
    }

    private void GenerateAtmos()
    {
        if (atmos == null)
            return;

        atmosInstance = Instantiate(atmos);
        atmosInstance.transform.localScale *= 2.02f * sphereRadius;
    }

}

public enum CubeFace
{
    Top,
    Bottom,
    Front,
    Back,
    Left,
    Right
}