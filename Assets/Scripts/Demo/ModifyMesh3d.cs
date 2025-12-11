                     using System.Collections.Generic;
using UnityEngine;

public class ModifyMesh3d : MonoBehaviour
{
    [Header("目标模块")]
    public GameObject targetModule;
    public List<GameObject> moduleList = new List<GameObject>();
    public Material defaultMat;

    [Header("固定边界设置")]
    public float fixedBoundSize = 5f;//默认等于模块立方体长度

    [Header("球体设置")]
    public float sphereRadius = 10f;
    public float moduleHeight = 2f;

    [Header("底部四边形顶点（球面坐标）")]
    [Range(0, 360)] public float theta0 = 45f;
    [Range(-90, 90)] public float phi0 = -2.5f;

    [Range(0, 360)] public float theta1 = 45f;
    [Range(-90, 90)] public float phi1 = 2.5f;

    [Range(0, 360)] public float theta2 = 50f;
    [Range(-90, 90)] public float phi2 = 2.5f;

    [Range(0, 360)] public float theta3 = 50f;
    [Range(-90, 90)] public float phi3 = -2.5f;

    [Header("几何可视化")]
    public float pointRadius = 0.2f;
    public bool showFixedBounds = true;

    // 存储生成的控制点
    private List<Vector3> bottomVertices = new List<Vector3>();
    private List<Vector3> topVertices = new List<Vector3>();
    private List<Vector3> bottomNormals = new List<Vector3>();

    private GameObject deformedModule;

    private List<ModuleData> moduleDatas = new();

    /// <summary>
    /// 生成变形模块
    /// </summary>
    public void GenerateModule()
    {
        GenerateSphereQuad(theta0, phi0, theta1, phi1, theta2, phi2, theta3, phi3, bottomVertices, bottomNormals);
        ExtrudeToHexahedron(bottomVertices, bottomNormals, topVertices);
        //DeformModule();
    }

    /// <summary>
    /// 生成一组测试变形模块
    /// </summary>
    public void GenerateTestModule()
    {
        GenerateTestQuad();
        for (int i = 0; i < 4; i++)
        {
            Debug.Log(moduleDatas[0].bottomVertices[i]);
        }
        ExtrudeToHexahedron(moduleDatas[0].bottomVertices, moduleDatas[0].topVertices, moduleDatas[0].bottomNormals);
        for (int i = 0; i < 4; i++)
        {
            Debug.Log(moduleDatas[0].bottomVertices[i]);
        }
        DeformModule();
    }

    private void GenerateTestQuad()
    {
        moduleDatas.Clear();
        for(int i = 0; i < 1; i++)
        {
            ModuleData moduleData = new();
            moduleDatas.Add(moduleData);
        }

        GenerateSphereQuad(0, -10, 0, 10, 20, 10, 20, -10, moduleDatas[0].bottomVertices, moduleDatas[0].bottomNormals);
        for (int i = 0; i < 4; i++)
        {
            Debug.Log(moduleDatas[0].bottomVertices[i]);
        }
    }

    /// <summary>
    /// 生成底部点
    /// </summary>
    public void GenerateSphereQuad(float vt0, float vp0, float vt1, float vp1, float vt2, float vp2, float vt3, float vp3, List<Vector3> bvs, List<Vector3> bNormals)
    {
        bvs.Clear();
        bNormals.Clear();

        AddSpherePoint(vt1, vp0, bvs, bNormals);
        AddSpherePoint(vt1, vp1, bvs, bNormals);
        AddSpherePoint(vt2, vp2, bvs, bNormals);
        AddSpherePoint(vt3, vp3, bvs, bNormals);
    }

    void AddSpherePoint(float theta, float phi, List<Vector3> bvs, List<Vector3> bNormals)
    {
        Vector3 p = SphericalToCartesian(theta, phi, sphereRadius);
        bvs.Add(p);
        bNormals.Add(p.normalized);
    }

    /// <summary>
    /// 生成顶部点
    /// </summary>
    void ExtrudeToHexahedron(List<Vector3> bPos, List<Vector3>tPos, List<Vector3> bNormal)
    {
        topVertices.Clear();

        for (int i = 0; i < 4; i++)
        {
            Vector3 bottomPos = bPos[i];
            Vector3 normal = bNormal[i];

            Vector3 topPos = bottomPos + normal * moduleHeight;
            tPos.Add(topPos);
        }
    }

    /// <summary>
    /// 将模块映射到变形六面体
    /// </summary>
    void DeformModule()
    {
        if (targetModule == null)
        {
            Debug.LogError("目标模块为空");
            return;
        }

        MeshFilter sourceFilter = targetModule.GetComponent<MeshFilter>();
        if (sourceFilter == null || sourceFilter.sharedMesh == null)
        {
            Debug.LogError("目标模块没有网格");
            return;
        }

        Mesh originalMesh = sourceFilter.sharedMesh;
        Vector3[] originalVerts = originalMesh.vertices;

        Quaternion sourceRot = targetModule.transform.localRotation;
        Vector3 sourceScale = targetModule.transform.localScale;
        Matrix4x4 bakeMatrix = Matrix4x4.TRS(Vector3.zero, sourceRot, sourceScale);

        Vector3[] bakedVerts = new Vector3[originalVerts.Length];
        for (int i = 0; i < originalVerts.Length; i++)
        {
            bakedVerts[i] = bakeMatrix.MultiplyPoint3x4(originalVerts[i]);
        }

        Vector3[] newVerts = new Vector3[bakedVerts.Length];

        Vector3 fixedMin = Vector3.zero - new Vector3(fixedBoundSize / 2f, fixedBoundSize / 2f, fixedBoundSize / 2f);
        Vector3 fixedMax = Vector3.zero + new Vector3(fixedBoundSize / 2f, fixedBoundSize / 2f, fixedBoundSize / 2f);

        for (int i = 0; i < bakedVerts.Length; i++)
        {
            Vector3 normalized = NormalizeToFixedBounds(bakedVerts[i], fixedMin, fixedMax);

            newVerts[i] = TrilinearInterpolation(normalized.x, normalized.z, normalized.y);
        }

        Mesh deformedMesh = new Mesh();
        deformedMesh.name = "DeformedInstance_FixedBounds";
        deformedMesh.vertices = newVerts;
        deformedMesh.uv = originalMesh.uv;
        deformedMesh.triangles = originalMesh.triangles;

        deformedMesh.RecalculateNormals();
        deformedMesh.RecalculateTangents();
        deformedMesh.RecalculateBounds();

        if (deformedModule != null)
        {
            DestroyImmediate(deformedModule);
        }

        deformedModule = new GameObject("DeformedModule_FixedBounds");
        deformedModule.transform.position = transform.position;
        deformedModule.transform.rotation = Quaternion.identity;
        deformedModule.transform.localScale = Vector3.one;

        MeshFilter newFilter = deformedModule.AddComponent<MeshFilter>();
        newFilter.sharedMesh = deformedMesh;

        MeshRenderer newRenderer = deformedModule.AddComponent<MeshRenderer>();
        newRenderer.material = defaultMat != null ? defaultMat : new Material(Shader.Find("Standard"));

        Debug.Log($"模块变形完成 - 固定边界: {fixedBoundSize}, 顶点数: {newVerts.Length}");
    }

    /// <summary>
    /// 使用固定边界归一化（就像MarchingCube中的固定间距）
    /// </summary>
    Vector3 NormalizeToFixedBounds(Vector3 point, Vector3 min, Vector3 max)
    {
        float x = Mathf.InverseLerp(min.x, max.x, point.x);
        float y = Mathf.InverseLerp(min.y, max.y, point.y);
        float z = Mathf.InverseLerp(min.z, max.z, point.z);

        return new Vector3(x, y, z);
    }

    /// <summary>
    /// 三线性插值
    /// </summary>
    Vector3 TrilinearInterpolation(float u, float v, float w)
    {
        Vector3 bottomPos = BilinearInterpolation(u, v,
            bottomVertices[0], bottomVertices[1],
            bottomVertices[2], bottomVertices[3]);

        Vector3 topPos = BilinearInterpolation(u, v,
            topVertices[0], topVertices[1],
            topVertices[2], topVertices[3]);

        return Vector3.Lerp(bottomPos, topPos, w);
    }

    /// <summary>
    /// 双线性插值公式
    /// </summary>
    Vector3 BilinearInterpolation(float u, float v, Vector3 p00, Vector3 p01, Vector3 p11, Vector3 p10)
    {
        return (1 - u) * (1 - v) * p00 +
               (1 - u) * v * p01 +
               u * v * p11 +
               u * (1 - v) * p10;
    }

    /// <summary>
    /// 球面坐标转直角坐标
    /// </summary>
    private Vector3 SphericalToCartesian(float theta, float phi, float radius)
    {
        float thetaRad = theta * Mathf.Deg2Rad;
        float phiRad = phi * Mathf.Deg2Rad;
        float x = radius * Mathf.Cos(phiRad) * Mathf.Cos(thetaRad);
        float y = radius * Mathf.Sin(phiRad);
        float z = radius * Mathf.Cos(phiRad) * Mathf.Sin(thetaRad);
        return new Vector3(x, y, z);
    }

    /// <summary>
    /// 绘制调试信息
    /// </summary>
    private void OnDrawGizmos()
    {
        List<Vector3> bottomVertices = moduleDatas[0].bottomVertices;
        List<Vector3> topVertices = moduleDatas[0].topVertices;

        if (showFixedBounds)
        {
            Gizmos.color = Color.green.WithAlpha(0.1f);
            Gizmos.DrawWireCube(Vector3.zero, Vector3.one * fixedBoundSize);

            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(Vector3.zero, 0.5f);
        }

        Gizmos.color = Color.yellow;
        foreach (Vector3 v in bottomVertices)
        {
            Gizmos.DrawSphere(transform.position + v, pointRadius);
        }

        Gizmos.color = Color.green;
        foreach (Vector3 v in topVertices)
        {
            Gizmos.DrawSphere(transform.position + v, pointRadius);
        }

        if (bottomVertices.Count >= 4)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(transform.position + bottomVertices[0], transform.position + bottomVertices[1]);
            Gizmos.DrawLine(transform.position + bottomVertices[1], transform.position + bottomVertices[2]);
            Gizmos.DrawLine(transform.position + bottomVertices[2], transform.position + bottomVertices[3]);
            Gizmos.DrawLine(transform.position + bottomVertices[3], transform.position + bottomVertices[0]);

            Gizmos.color = Color.blue.WithAlpha(0.5f);
            for (int i = 0; i < 4; i++)
            {
                //Debug.Log(bottomVertices[i]);
                //Debug.Log(topVertices[i]);
                if (topVertices.Count > i)
                {
                    Gizmos.DrawLine(transform.position + bottomVertices[i], transform.position + topVertices[i]);
                }
            }

            if (topVertices.Count >= 4)
            {
                Gizmos.color = Color.magenta;
                Gizmos.DrawLine(transform.position + topVertices[0], transform.position + topVertices[1]);
                Gizmos.DrawLine(transform.position + topVertices[1], transform.position + topVertices[2]);
                Gizmos.DrawLine(transform.position + topVertices[2], transform.position + topVertices[3]);
                Gizmos.DrawLine(transform.position + topVertices[3], transform.position + topVertices[0]);
            }
        }
    }
}

/// <summary>
/// Color扩展方法
/// </summary>
public static class ColorExtensions
{
    public static Color WithAlpha(this Color color, float alpha)
    {
        return new Color(color.r, color.g, color.b, alpha);
    }
}

[System.Serializable]
public class ModuleData
{
    public int vertIndex;
    public List<Vector3> bottomVertices = new List<Vector3>();
    public List<Vector3> topVertices = new List<Vector3>();
    public List<Vector3> bottomNormals = new List<Vector3>();
}