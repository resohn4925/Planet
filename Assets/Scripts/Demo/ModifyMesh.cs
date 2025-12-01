using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifyMesh : MonoBehaviour
{
    public GameObject targetMesh;
    
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

    /// <summary>
    /// 使用double插值算法把目标点的位置变换到mesh中
    /// </summary>
    public void ApplyModifyMesh()
    {
        if (targetMesh == null)
        {
            Debug.LogError("target mesh is null");
            return;
        }

        MeshFilter meshFilter = targetMesh.GetComponent<MeshFilter>();
        if (meshFilter == null || meshFilter.sharedMesh == null)
        {
            Debug.LogError("no mesh found in the target mesh.");
            return;
        }

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

        Vector3[] vertices = newMesh.vertices;
        //原始网格坐标点
        Vector3 originalA = new Vector3(0, 0, 0);
        Vector3 originalB = new Vector3(0, 0, 1);
        Vector3 originalC = new Vector3(1, 0, 1);
        Vector3 originalD = new Vector3(1, 0, 0);
        //目标网格坐标点
        Vector3 targetA = new Vector3(posX0, 0, posZ0);
        Vector3 targetB = new Vector3(posX1, 0, posZ1);
        Vector3 targetC = new Vector3(posX2, 0, posZ2);
        Vector3 targetD = new Vector3(posX3, 0, posZ3);
        for (int i = 0; i < vertices.Length; i++)
        {
            //double lerp算法
            vertices[i] = UniversalBilinearInterpolation(
                vertices[i],
                originalA, originalB, originalC, originalD,
                targetA, targetB, targetC, targetD
            );
        }

        newMesh.vertices = vertices;

        newMesh.RecalculateNormals();
        newMesh.RecalculateBounds();
        newMesh.name = "TestMesh";

        // 新网格赋给目标物体
        meshFilter.sharedMesh = newMesh;
    }

    private Vector3 UniversalBilinearInterpolation(
        Vector3 originalPoint,
        Vector3 origA, Vector3 origB, Vector3 origC, Vector3 origD,
        Vector3 targetA, Vector3 targetB, Vector3 targetC, Vector3 targetD)
    {
        // 计算点在原始正方形中的参数化坐标 (u, v)
        // 使用简单的投影方法计算u和v

        // 计算原始正方形的宽度和高度
        float origWidth = Vector3.Distance(origA, origD);
        float origHeight = Vector3.Distance(origA, origB);

        // 计算原始正方形的中心
        Vector3 origCenter = (origA + origB + origC + origD) / 4f;

        // 计算原始正方形的主方向
        Vector3 origRight = (origD - origA).normalized;
        Vector3 origUp = (origB - origA).normalized;

        // 计算点在原始方向上的投影
        Vector3 fromOrigCenter = originalPoint - origCenter;
        float u = Vector3.Dot(fromOrigCenter, origRight) / origWidth;
        float v = Vector3.Dot(fromOrigCenter, origUp) / origHeight;

        // 调整u,v到正确的范围（考虑原始正方形的实际位置）
        u += 0.5f; // 因为中心在(0.5,0.5)，所以需要偏移
        v += 0.5f;

        // 使用双线性插值计算目标位置
        Vector3 result =
            (1 - u) * (1 - v) * targetA +
            (1 - u) * v * targetB +
            u * (1 - v) * targetD +
            u * v * targetC;

        // 保持原始Y坐标不变（高度）
        result.y = originalPoint.y;

        return result;
    }

public void GeneratePoint()
    {
        testMeshData = new();
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
