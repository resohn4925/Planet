using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifyMesh3d : MonoBehaviour
{
    [Header("目标模块")]
    public GameObject targetModule;
    public Material defaultMat;

    [Header("球体设置")]
    public float sphereRadius = 10f;
    public float moduleHeight = 2f;

    [Header("底部四边形顶点（球面坐标）")]
    [Range(0, 360)] public float theta0 = 0f;
    [Range(-90, 90)] public float phi0 = 0f;

    [Range(0, 360)] public float theta1 = 90f;
    [Range(-90, 90)] public float phi1 = 0f;

    [Range(0, 360)] public float theta2 = 180f;
    [Range(-90, 90)] public float phi2 = 0f;

    [Range(0, 360)] public float theta3 = 270f;
    [Range(-90, 90)] public float phi3 = 0f;

    [Header("几何可视化")]
    public float pointRadius = 0.2f;
    public bool drawWireframe = true;
    public bool drawNormals = true;

    private List<Vector3> bottomVertices = new List<Vector3>();
    private List<Vector3> topVertices = new List<Vector3>();
    private List<Vector3> allVertices = new List<Vector3>();
    private List<Vector3> bottomNormals = new List<Vector3>();
    private List<Vector3> topNormals = new List<Vector3>();

    private GameObject deformedModule;

    List<Vector3> testMeshData = new();

    public void GeneratePoint()
    {
        GenerateSphereQuad();
        ExtrudeToHexahedron();
    }

    /// <summary>
    ///转到直角坐标空间
    /// </summary>
    public void GenerateSphereQuad()
    {
        bottomVertices.Clear();

        Vector3 p0 = SphericalToCartesian(theta0, phi0, sphereRadius);
        Vector3 p1 = SphericalToCartesian(theta1, phi1, sphereRadius);
        Vector3 p2 = SphericalToCartesian(theta2, phi2, sphereRadius);
        Vector3 p3 = SphericalToCartesian(theta3, phi3, sphereRadius);

        bottomVertices.Add(p0);
        bottomVertices.Add(p1);
        bottomVertices.Add(p2);
        bottomVertices.Add(p3);

        //计算法线
        bottomNormals.Clear();
        bottomNormals.Add(p0.normalized);
        bottomNormals.Add(p1.normalized);
        bottomNormals.Add(p2.normalized);
        bottomNormals.Add(p3.normalized);
    }

    /// <summary>
    /// 延伸顶点
    /// </summary>
    void ExtrudeToHexahedron()
    {
        topVertices.Clear();
        allVertices.Clear();
        topNormals.Clear();

        for (int i = 0; i < 4; i++)
        {
            Vector3 bottomPos = bottomVertices[i];
            Vector3 normal = bottomNormals[i];

            Vector3 topPos = bottomPos + normal * moduleHeight;
            topVertices.Add(topPos);
            topNormals.Add(normal);
        }

        allVertices.AddRange(bottomVertices);
        allVertices.AddRange(topVertices);
    }

    private Vector3 SphericalToCartesian(float theta, float phi, float radius)
    {
        float thetaRad = theta * Mathf.Deg2Rad;
        float phiRad = phi * Mathf.Deg2Rad;
        float x = radius * Mathf.Cos(phiRad) * Mathf.Cos(thetaRad);
        float y = radius * Mathf.Sin(phiRad);
        float z = radius * Mathf.Cos(phiRad) * Mathf.Sin(thetaRad);
        
        return new Vector3(x, y, z);
    }

    //private GameObject deformedModule;

    ///// <summary>
    ///// 使用双线性插值算法把目标点的位置变换到mesh中
    ///// </summary>
    //public void ApplyModifyMesh()
    //{
    //    if (targetObj == null)
    //    {
    //        Debug.LogError("target mesh is null");
    //        return;
    //    }

    //    MeshFilter meshFilter = targetObj.GetComponent<MeshFilter>();
    //    if (meshFilter == null || meshFilter.sharedMesh == null)
    //    {
    //        Debug.LogError("no mesh found in the target mesh.");
    //        return;
    //    }

    //    //记录原始物件transform
    //    Quaternion originalRotation = targetObj.transform.rotation;
    //    Vector3 originalScale = targetObj.transform.localScale;
    //    Debug.Log($"原始旋转: {originalRotation.eulerAngles}");
    //    Debug.Log($"原始缩放: {originalScale}");

    //    // 获取原始网格
    //    Mesh originalMesh = meshFilter.sharedMesh;

    //    // 创建网格实例
    //    Mesh newMesh = new Mesh();

    //    // 复制原始网格数据
    //    newMesh.vertices = originalMesh.vertices.Clone() as Vector3[];
    //    newMesh.triangles = originalMesh.triangles.Clone() as int[];
    //    newMesh.normals = originalMesh.normals.Clone() as Vector3[];
    //    newMesh.uv = originalMesh.uv.Clone() as Vector2[];
    //    newMesh.colors = originalMesh.colors.Clone() as Color[];
    //    newMesh.tangents = originalMesh.tangents.Clone() as Vector4[];

    //    //transformedObj
    //    Vector3[] vertices = newMesh.vertices;
    //    for (int i = 0; i < vertices.Length; i++)
    //    {
    //        vertices[i].x *= originalScale.x;
    //        vertices[i].y *= originalScale.y;
    //        vertices[i].z *= originalScale.z;

    //        vertices[i] = originalRotation * vertices[i];
    //    }
    //    newMesh.vertices = vertices;

    //    if (transformedObj != null)
    //    {
    //        DestroyImmediate(transformedObj);
    //    }
    //    transformedObj = new GameObject("transformedObj");

    //    transformedObj.transform.localScale = new Vector3(1, 1, 1);
    //    MeshFilter transformedMeshFilter = transformedObj.AddComponent<MeshFilter>();
    //    newMesh.name = "Mesh";
    //    transformedMeshFilter.sharedMesh = newMesh;
    //    MeshRenderer meshRenderer = transformedObj.AddComponent<MeshRenderer>();
    //    meshRenderer.material = defaultMat;

    //    // 获取原始网格的包围盒来映射UV坐标
    //    Bounds bounds = newMesh.bounds;

    //    float minX = -2.5f;
    //    float maxX = 2.5f;
    //    float minZ = -2.5f;
    //    float maxZ = 2.5f;

    //    //计算原始网格对应模块Cube的坐标点来映射UV坐标


    //    //原始网格坐标点
    //    Vector3 originalA = new Vector3(minX, 0, minZ);
    //    Vector3 originalB = new Vector3(minX, 0, maxZ);
    //    Vector3 originalC = new Vector3(maxX, 0, maxZ);
    //    Vector3 originalD = new Vector3(maxX, 0, minZ);

    //    //目标网格坐标点
    //    Vector3 targetA = new Vector3(posX0, 0, posZ0);
    //    Vector3 targetB = new Vector3(posX1, 0, posZ1);
    //    Vector3 targetC = new Vector3(posX2, 0, posZ2);
    //    Vector3 targetD = new Vector3(posX3, 0, posZ3);

    //    for (int i = 0; i < vertices.Length; i++)
    //    {
    //        // 计算顶点在包围盒中的归一化坐标
    //        float u = Mathf.InverseLerp(minX, maxX, vertices[i].x);
    //        float v = Mathf.InverseLerp(minZ, maxZ, vertices[i].z);

    //        // 使用double lerp计算新位置
    //        Vector3 interpolatedPosition = BilinearInterpolation(u, v, targetA, targetB, targetC, targetD);

    //        // y坐标不变
    //        interpolatedPosition.y = vertices[i].y * 10f;

    //        vertices[i] = interpolatedPosition;
    //    }

    //    newMesh.vertices = vertices;

    //    newMesh.RecalculateNormals();
    //    newMesh.RecalculateBounds();
    //    newMesh.name = "TestMesh";

    //    // 新网格赋给目标物体
    //    transformedMeshFilter.sharedMesh = newMesh;
    //}

    ///// <summary>
    ///// 双线性插值算法
    ///// </summary>
    //private Vector3 BilinearInterpolation(float u, float v, Vector3 A, Vector3 B, Vector3 C, Vector3 D)
    //{
    //    // uv坐标归一化
    //    u = Mathf.Clamp01(u);
    //    v = Mathf.Clamp01(v);

    //    // double lerp
    //    Vector3 result = (1 - u) * (1 - v) * A +
    //                    (1 - u) * v * B +
    //                    u * (1 - v) * D +
    //                    u * v * C;

    //    return result;
    //}

    //public void GeneratePoint()
    //{
    //    testMeshData = new List<Vector3>();
    //    Vector3 newPos = new Vector3(posX0, 0, posZ0);
    //    testMeshData.Add(newPos);
    //    newPos = new Vector3(posX1, 0, posZ1);
    //    testMeshData.Add(newPos);
    //    newPos = new Vector3(posX2, 0, posZ2);
    //    testMeshData.Add(newPos);
    //    newPos = new Vector3(posX3, 0, posZ3);
    //    testMeshData.Add(newPos);
    //}

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        foreach (Vector3 var in allVertices)
        {
            Gizmos.DrawSphere(var, pointRadius);
        }
    }
}