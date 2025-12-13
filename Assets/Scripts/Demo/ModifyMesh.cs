using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifyMesh : MonoBehaviour
{
    public GameObject targetObj;
    public Material defaultMat;

    //变形四边形网格坐标
    [Header("0号点坐标")]
    public float posX0;
    public float posZ0;

    [Header("1号点坐标")]
    public float posX1;
    public float posZ1;

    [Header("2号点坐标")]
    public float posX2;
    public float posZ2;

    [Header("3号点坐标")]
    public float posX3;
    public float posZ3;

    List<Vector3> testMeshData = new();

    [Header("顶点小球半径")]
    public float sphereRadius;

    private GameObject transformedObj;

    /// <summary>
    /// 使用双线性插值算法把目标点的位置变换到mesh中
    /// </summary>
    public void ApplyModifyMesh()
    {
        if (targetObj == null)
        {
            Debug.LogError("target mesh is null");
            return;
        }

        MeshFilter meshFilter = targetObj.GetComponent<MeshFilter>();
        if (meshFilter == null || meshFilter.sharedMesh == null)
        {
            Debug.LogError("no mesh found in the target mesh.");
            return;
        }

        //记录原始物件transform
        Quaternion originalRotation = targetObj.transform.rotation;
        Vector3 originalScale = targetObj.transform.localScale;
        Debug.Log($"原始旋转: {originalRotation.eulerAngles}");
        Debug.Log($"原始缩放: {originalScale}");

        // 获取原始网格
        Mesh originalMesh = meshFilter.sharedMesh;

        // 创建网格实例
        Mesh newMesh = new Mesh();

        // 复制原始网格数据
        newMesh.vertices = originalMesh.vertices.Clone() as Vector3[];
        newMesh.triangles = originalMesh.triangles.Clone() as int[];
        newMesh.normals = originalMesh.normals.Clone() as Vector3[];
        newMesh.uv = originalMesh.uv.Clone() as Vector2[];
        newMesh.colors = originalMesh.colors.Clone() as Color[];
        newMesh.tangents = originalMesh.tangents.Clone() as Vector4[];

        //transformedObj
        Vector3[] vertices = newMesh.vertices;
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i].x *= originalScale.x;
            vertices[i].y *= originalScale.y;
            vertices[i].z *= originalScale.z;

            vertices[i] = originalRotation * vertices[i];
        }
        newMesh.vertices = vertices;

        if (transformedObj != null)
        {
            DestroyImmediate(transformedObj);
        }
        transformedObj = new GameObject("transformedObj");

        transformedObj.transform.localScale = new Vector3(1, 1, 1);
        MeshFilter transformedMeshFilter = transformedObj.AddComponent<MeshFilter>();
        newMesh.name = "Mesh";
        transformedMeshFilter.sharedMesh = newMesh;
        MeshRenderer meshRenderer = transformedObj.AddComponent<MeshRenderer>();
        meshRenderer.material = defaultMat;

        // 获取原始网格的包围盒来映射UV坐标
        Bounds bounds = newMesh.bounds;

        float minX = -2.5f;
        float maxX = 2.5f;
        float minZ = -2.5f;
        float maxZ = 2.5f;

        //计算原始网格对应模块Cube的坐标点来映射UV坐标


        //原始网格坐标点
        Vector3 originalA = new Vector3(minX, 0, minZ);
        Vector3 originalB = new Vector3(minX, 0, maxZ);
        Vector3 originalC = new Vector3(maxX, 0, maxZ);
        Vector3 originalD = new Vector3(maxX, 0, minZ);

        //目标网格坐标点
        Vector3 targetA = new Vector3(posX0, 0, posZ0);
        Vector3 targetB = new Vector3(posX1, 0, posZ1);
        Vector3 targetC = new Vector3(posX2, 0, posZ2);
        Vector3 targetD = new Vector3(posX3, 0, posZ3);

        for (int i = 0; i < vertices.Length; i++)
        {
            // 计算顶点在包围盒中的归一化坐标
            float u = Mathf.InverseLerp(minX, maxX, vertices[i].x);
            float v = Mathf.InverseLerp(minZ, maxZ, vertices[i].z);

            // 使用double lerp计算新位置
            Vector3 interpolatedPosition = BilinearInterpolation(u, v, targetA, targetB, targetC, targetD);

            // y坐标不变
            interpolatedPosition.y = vertices[i].y * 10f;

            vertices[i] = interpolatedPosition;
        }

        newMesh.vertices = vertices;

        newMesh.RecalculateNormals();
        newMesh.RecalculateBounds();
        newMesh.name = "TestMesh";

        // 新网格赋给目标物体
        transformedMeshFilter.sharedMesh = newMesh;
    }

    /// <summary>
    /// 双线性插值算法
    /// </summary>
    private Vector3 BilinearInterpolation(float u, float v, Vector3 A, Vector3 B, Vector3 C, Vector3 D)
    {
        // uv坐标归一化
        u = Mathf.Clamp01(u);
        v = Mathf.Clamp01(v);

        // double lerp
        Vector3 result = (1 - u) * (1 - v) * A +
                        (1 - u) * v * B +
                        u * (1 - v) * D +
                        u * v * C;

        return result;
    }

    public void GeneratePoint()
    {
        testMeshData = new List<Vector3>();
        Vector3 newPos = new Vector3(posX0, 0, posZ0);
        testMeshData.Add(newPos);
        newPos = new Vector3(posX1, 0, posZ1);
        testMeshData.Add(newPos);
        newPos = new Vector3(posX2, 0, posZ2);
        testMeshData.Add(newPos);
        newPos = new Vector3(posX3, 0, posZ3);
        testMeshData.Add(newPos);
    }

    private void OnDrawGizmos()
    {
        //依次绘制四边形顶点
        Gizmos.color = Color.yellow;
        foreach (Vector3 var in testMeshData)
        {
            Gizmos.DrawSphere(var, sphereRadius);
        }
    }
}