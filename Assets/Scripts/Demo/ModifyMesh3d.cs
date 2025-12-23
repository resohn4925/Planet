using System.Collections.Generic;
using UnityEngine;

public class ModifyMesh3d : MonoBehaviour
{
    [Header("目标模块")]
    public GameObject targetModule;
    public List<GameObject> moduleList = new List<GameObject>();
    public Material defaultMat;

    [Header("固定边界设置")]
    public float fixedBoundSize = 5f;//默认的原始模型包围盒尺寸

    [Header("球面设置")]
    public float sphereRadius = 20f;
    public float sphereRadiusTop = 25f;
    public float moduleHeight = 2f;

    [Header("可视化选项")]
    public float pointRadius = 0.2f;
    public bool showFixedBounds = true;
    public bool showAllModules = true;

    // 存储生成的模块顶点
    private List<Vector3> bottomVertices = new List<Vector3>();
    private List<Vector3> topVertices = new List<Vector3>();
    private List<Vector3> bottomNormals = new List<Vector3>();

    private List<GameObject> deformedModules = new List<GameObject>();
    private List<ModuleData> moduleDatas = new();

    /// <summary>
    /// 生成单个变形模块的接口
    /// </summary>
    public void GetGenerateModule(List<Vector3> bottomPoints, GameObject sourceModule, float height)
    {
        if (sourceModule == null)
        {
            Debug.LogError("原始模块为空");
            return;
        }

        if (bottomPoints == null || bottomPoints.Count < 4)
        {
            Debug.LogError("底部点数量不足4个");
            return;
        }

        MeshFilter sourceFilter = sourceModule.GetComponent<MeshFilter>();
        if (sourceFilter == null || sourceFilter.sharedMesh == null)
        {
            Debug.LogError("原始模块无网格");
            return;
        }

        Mesh originalMesh = sourceFilter.sharedMesh;

        if (height <= 0)
        {
            height = moduleHeight;
        }

        // 准备顶部点
        List<Vector3> bottomNormals = new List<Vector3>();
        for (int i = 0; i < 4; i++)
        {
            bottomNormals.Add(bottomPoints[i].normalized);
        }

        List<Vector3> topPoints = new List<Vector3>();
        for (int i = 0; i < 4; i++)
        {
            Vector3 bottomPos = bottomPoints[i];
            Vector3 normal = bottomNormals[i];
            Vector3 topPos = bottomPos + normal * height;
            topPoints.Add(topPos);
        }

        Vector3[] originalVerts = originalMesh.vertices;
        Vector3[] originalNormals = originalMesh.normals;
        Vector4[] originalTangents = originalMesh.tangents;

        // 基础变换矩阵
        Quaternion sourceRot = sourceModule.transform.localRotation;
        Vector3 sourceScale = sourceModule.transform.localScale;
        Matrix4x4 bakeMatrix = Matrix4x4.TRS(Vector3.zero, sourceRot, sourceScale);

        // 预计算烘焙顶点
        Vector3[] bakedVerts = new Vector3[originalVerts.Length];
        for (int i = 0; i < originalVerts.Length; i++)
        {
            bakedVerts[i] = bakeMatrix.MultiplyPoint3x4(originalVerts[i]);
        }

        // 固定边界定义
        Vector3 fixedMin = Vector3.zero - new Vector3(fixedBoundSize / 2f, fixedBoundSize / 2f, fixedBoundSize / 2f);
        Vector3 fixedMax = Vector3.zero + new Vector3(fixedBoundSize / 2f, fixedBoundSize / 2f, fixedBoundSize / 2f);

        // 准备新网格数据
        Vector3[] newVerts = new Vector3[bakedVerts.Length];
        Vector3[] newNormals = new Vector3[originalVerts.Length];
        Vector4[] newTangents = new Vector4[originalVerts.Length];

        bool hasNormals = originalNormals != null && originalNormals.Length > 0;
        bool hasTangents = originalTangents != null && originalTangents.Length > 0;

        // 微分步长
        float delta = 0.001f;

        for (int i = 0; i < bakedVerts.Length; i++)
        {
            // 获取归一化坐标 (0-1)
            Vector3 coord = NormalizeToFixedBounds(bakedVerts[i], fixedMin, fixedMax);

            // 计算当前顶点的新位置
            newVerts[i] = TrilinearInterpolation(coord.x, coord.z, coord.y, bottomPoints, topPoints);

            if (hasNormals || hasTangents)
            {
                // 计算X轴方向
                Vector3 pRight;
                if (coord.x < 0.5f)
                    pRight = (TrilinearInterpolation(coord.x + delta, coord.z, coord.y, bottomPoints, topPoints) - newVerts[i]).normalized;
                else
                    pRight = (newVerts[i] - TrilinearInterpolation(coord.x - delta, coord.z, coord.y, bottomPoints, topPoints)).normalized;

                // 计算Z轴方向
                Vector3 pForward;
                if (coord.z < 0.5f)
                    pForward = (TrilinearInterpolation(coord.x, coord.z + delta, coord.y, bottomPoints, topPoints) - newVerts[i]).normalized;
                else
                    pForward = (newVerts[i] - TrilinearInterpolation(coord.x, coord.z - delta, coord.y, bottomPoints, topPoints)).normalized;

                // 计算Y轴方向 (W方向的变化/高度方向)
                Vector3 pUp;
                if (coord.y < 0.5f)
                    pUp = (TrilinearInterpolation(coord.x, coord.z, coord.y + delta, bottomPoints, topPoints) - newVerts[i]).normalized;
                else
                    pUp = (newVerts[i] - TrilinearInterpolation(coord.x, coord.z, coord.y - delta, bottomPoints, topPoints)).normalized;

                // 应用原始旋转到法线
                Vector3 rawNormal = sourceRot * originalNormals[i];

                // 将相对朝向投影到新的变形坐标系中
                Vector3 finalNormal = (pRight * rawNormal.x + pUp * rawNormal.y + pForward * rawNormal.z).normalized;
                newNormals[i] = finalNormal;

                // 处理切线
                if (hasTangents)
                {
                    Vector3 rawTangentDir = sourceRot * new Vector3(originalTangents[i].x, originalTangents[i].y, originalTangents[i].z);
                    Vector3 finalTangentDir = (pRight * rawTangentDir.x + pUp * rawTangentDir.y + pForward * rawTangentDir.z).normalized;
                    newTangents[i] = new Vector4(finalTangentDir.x, finalTangentDir.y, finalTangentDir.z, originalTangents[i].w);
                }
            }
        }

        // 构建网格
        Mesh deformedMesh = new Mesh();
        deformedMesh.name = sourceModule.name + "_modified";
        deformedMesh.vertices = newVerts;
        deformedMesh.uv = originalMesh.uv;

        if (hasNormals) deformedMesh.normals = newNormals;
        if (hasTangents) deformedMesh.tangents = newTangents;

        // 颜色
        if (originalMesh.colors != null && originalMesh.colors.Length > 0)
            deformedMesh.colors = originalMesh.colors;

        // 检查负缩放
        bool needFlipTriangles = sourceScale.x * sourceScale.y * sourceScale.z < 0;

        // 三角形
        deformedMesh.subMeshCount = originalMesh.subMeshCount;
        for (int i = 0; i < originalMesh.subMeshCount; i++)
        {
            int[] triangles = originalMesh.GetTriangles(i);
            if (needFlipTriangles)
            {
                for (int j = 0; j < triangles.Length; j += 3)
                {
                    int temp = triangles[j];
                    triangles[j] = triangles[j + 2];
                    triangles[j + 2] = temp;
                }
            }
            deformedMesh.SetTriangles(triangles, i);
        }

        // 如果原始网格没法线才重算，否则信任上面的计算
        if (!hasNormals) deformedMesh.RecalculateNormals();

        deformedMesh.RecalculateBounds();

        // 场景对象创建
        GameObject modifiedRoot = GameObject.Find("ModifiedRoot");
        if (modifiedRoot == null) modifiedRoot = new GameObject("ModifiedRoot");

        GameObject modifiedObject = new GameObject(sourceModule.name + "_modified");
        modifiedObject.transform.SetParent(modifiedRoot.transform);
        modifiedObject.transform.localPosition = Vector3.zero;
        modifiedObject.transform.localRotation = Quaternion.identity;
        modifiedObject.transform.localScale = Vector3.one;

        MeshFilter meshFilter = modifiedObject.AddComponent<MeshFilter>();
        meshFilter.sharedMesh = deformedMesh;

        MeshRenderer meshRenderer = modifiedObject.AddComponent<MeshRenderer>();
        MeshRenderer sourceRenderer = sourceModule.GetComponent<MeshRenderer>();

        if (sourceRenderer != null && sourceRenderer.sharedMaterials.Length > 0)
            meshRenderer.sharedMaterials = sourceRenderer.sharedMaterials;
        else
            meshRenderer.material = defaultMat != null ? defaultMat : new Material(Shader.Find("Standard"));

        deformedModules.Add(modifiedObject);
    }

    /// <summary>
    /// 生成一系列变形模块
    /// </summary>
    public void GenerateModule()
    {
        ClearAllModules();
        GenerateQuad();

        for (int i = 0; i < moduleDatas.Count; i++)
        {
            ModuleData moduleData = moduleDatas[i];

            GameObject sourceModule = i < moduleList.Count ? moduleList[i] : targetModule;
            if (sourceModule == null)
            {
                Debug.LogWarning($"模块{i}没有对应的预制体，跳过");
                continue;
            }

            GameObject deformedModule = DeformModule(sourceModule, moduleData.bottomVertices, moduleData.topVertices);
            if (deformedModule != null)
            {
                deformedModule.name = $"Deformed_Module_{i}";
                deformedModules.Add(deformedModule);
            }
        }
    }

    private void GenerateQuad()
    {
        moduleDatas.Clear();

        ModuleData module1 = new ModuleData();
        GenerateSphereQuad(-22.5f, -22.5f, -22.5f, 0, 0, 0, 0, -22.5f, module1.bottomVertices, module1.bottomNormals, sphereRadius);
        ExtrudeToHexahedron(module1.bottomVertices, module1.bottomNormals, module1.topVertices);
        moduleDatas.Add(module1);

        ModuleData module2 = new ModuleData();
        GenerateSphereQuad(-22.5f, 0, -22.5f, 22.5f, 0, 22.5f, 0, 0, module2.bottomVertices, module2.bottomNormals, sphereRadius);
        ExtrudeToHexahedron(module2.bottomVertices, module2.bottomNormals, module2.topVertices);
        moduleDatas.Add(module2);

        ModuleData module3 = new ModuleData();
        GenerateSphereQuad(0, 0, 0, 22.5f, 22.5f, 22.5f, 22.5f, 0, module3.bottomVertices, module3.bottomNormals, sphereRadius);
        ExtrudeToHexahedron(module3.bottomVertices, module3.bottomNormals, module3.topVertices);
        moduleDatas.Add(module3);

        ModuleData module4 = new ModuleData();
        GenerateSphereQuad(0, -22.5f, 0, 0, 22.5f, 0, 22.5f, -22.5f, module4.bottomVertices, module4.bottomNormals, sphereRadius);
        ExtrudeToHexahedron(module4.bottomVertices, module4.bottomNormals, module4.topVertices);
        moduleDatas.Add(module4);

        // 生成顶部四个模块
        ModuleData module5 = new ModuleData();
        GenerateSphereQuad(-22.5f, -22.5f, -22.5f, 0, 0, 0, 0, -22.5f, module5.bottomVertices, module5.bottomNormals, sphereRadiusTop);
        ExtrudeToHexahedron(module5.bottomVertices, module5.bottomNormals, module5.topVertices);
        moduleDatas.Add(module5);

        ModuleData module6 = new ModuleData();
        GenerateSphereQuad(-22.5f, 0, -22.5f, 22.5f, 0, 22.5f, 0, 0, module6.bottomVertices, module6.bottomNormals, sphereRadiusTop);
        ExtrudeToHexahedron(module6.bottomVertices, module6.bottomNormals, module6.topVertices);
        moduleDatas.Add(module6);

        ModuleData module7 = new ModuleData();
        GenerateSphereQuad(0, 0, 0, 22.5f, 22.5f, 22.5f, 22.5f, 0, module7.bottomVertices, module7.bottomNormals, sphereRadiusTop);
        ExtrudeToHexahedron(module7.bottomVertices, module7.bottomNormals, module7.topVertices);
        moduleDatas.Add(module7);

        ModuleData module8 = new ModuleData();
        GenerateSphereQuad(0, -22.5f, 0, 0, 22.5f, 0, 22.5f, -22.5f, module8.bottomVertices, module8.bottomNormals, sphereRadiusTop);
        ExtrudeToHexahedron(module8.bottomVertices, module8.bottomNormals, module8.topVertices);
        moduleDatas.Add(module8);
    }

    /// <summary>
    /// 生成球面四边形
    /// </summary>
    public void GenerateSphereQuad(float vt0, float vp0, float vt1, float vp1, float vt2, float vp2, float vt3, float vp3,
                                  List<Vector3> bvs, List<Vector3> bNormals, float sphereRadius)
    {
        bvs.Clear();
        bNormals.Clear();

        AddSpherePoint(vt0, vp0, bvs, bNormals, sphereRadius);
        AddSpherePoint(vt1, vp1, bvs, bNormals, sphereRadius);
        AddSpherePoint(vt2, vp2, bvs, bNormals, sphereRadius);
        AddSpherePoint(vt3, vp3, bvs, bNormals, sphereRadius);
    }

    void AddSpherePoint(float theta, float phi, List<Vector3> bvs, List<Vector3> bNormals, float sphereRadius)
    {
        Vector3 p = SphericalToCartesian(theta, phi, sphereRadius);
        bvs.Add(p);
        bNormals.Add(p.normalized);
    }

    /// <summary>
    /// 挤出形成六面体
    /// </summary>
    void ExtrudeToHexahedron(List<Vector3> bPos, List<Vector3> bNormal, List<Vector3> tPos)
    {
        tPos.Clear();

        for (int i = 0; i < 4; i++)
        {
            Vector3 bottomPos = bPos[i];
            Vector3 normal = bNormal[i];
            Vector3 topPos = bottomPos + normal * moduleHeight;
            tPos.Add(topPos);
        }
    }

    /// <summary>
    /// 将模型映射到四边形变形体
    /// </summary>
    GameObject DeformModule(GameObject sourceModule, List<Vector3> bvs, List<Vector3> tvs)
    {
        if (sourceModule == null)
        {
            Debug.LogError("源模块为空");
            return null;
        }

        MeshFilter sourceFilter = sourceModule.GetComponent<MeshFilter>();
        if (sourceFilter == null || sourceFilter.sharedMesh == null)
        {
            Debug.LogError("源模块没有有效网格");
            return null;
        }

        Mesh originalMesh = sourceFilter.sharedMesh;
        Vector3[] originalVerts = originalMesh.vertices;

        Quaternion sourceRot = sourceModule.transform.localRotation;
        Vector3 sourceScale = sourceModule.transform.localScale;
        Matrix4x4 bakeMatrix = Matrix4x4.TRS(Vector3.zero, sourceRot, sourceScale);

        Vector3[] bakedVerts = new Vector3[originalVerts.Length];
        for (int i = 0; i < originalVerts.Length; i++)
        {
            bakedVerts[i] = bakeMatrix.MultiplyPoint3x4(originalVerts[i]);
        }

        Vector3 fixedMin = Vector3.zero - new Vector3(fixedBoundSize / 2f, fixedBoundSize / 2f, fixedBoundSize / 2f);
        Vector3 fixedMax = Vector3.zero + new Vector3(fixedBoundSize / 2f, fixedBoundSize / 2f, fixedBoundSize / 2f);

        Vector3[] newVerts = new Vector3[bakedVerts.Length];
        for (int i = 0; i < bakedVerts.Length; i++)
        {
            Vector3 normalized = NormalizeToFixedBounds(bakedVerts[i], fixedMin, fixedMax);
            newVerts[i] = TrilinearInterpolation(normalized.x, normalized.z, normalized.y, bvs, tvs);
        }

        Mesh deformedMesh = new Mesh();
        deformedMesh.name = "DeformedMesh";
        deformedMesh.vertices = newVerts;
        deformedMesh.uv = originalMesh.uv;
        deformedMesh.triangles = originalMesh.triangles;

        deformedMesh.RecalculateNormals();
        deformedMesh.RecalculateTangents();
        deformedMesh.RecalculateBounds();

        GameObject deformedModule = new GameObject();
        deformedModule.transform.position = Vector3.zero;
        deformedModule.transform.rotation = Quaternion.identity;
        deformedModule.transform.localScale = Vector3.one;

        MeshFilter newFilter = deformedModule.AddComponent<MeshFilter>();
        newFilter.sharedMesh = deformedMesh;

        MeshRenderer newRenderer = deformedModule.AddComponent<MeshRenderer>();
        newRenderer.material = defaultMat != null ? defaultMat : new Material(Shader.Find("Standard"));

        return deformedModule;
    }

    /// <summary>
    /// 清除所有生成的模块
    /// </summary>
    public void ClearAllModules()
    {
        foreach (GameObject obj in deformedModules)
        {
            if (obj != null)
                DestroyImmediate(obj);
        }
        deformedModules.Clear();
    }

    /// <summary>
    /// 使用固定边界归一化一个点
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
    Vector3 TrilinearInterpolation(float u, float v, float w, List<Vector3> bvs, List<Vector3> tvs)
    {
        u = Mathf.Clamp01(u);
        v = Mathf.Clamp01(v);
        w = Mathf.Clamp01(w);

        Vector3 bottomPos = BilinearInterpolation(u, v,
            bvs[0], bvs[1],
            bvs[2], bvs[3]);

        Vector3 topPos = BilinearInterpolation(u, v,
            tvs[0], tvs[1],
            tvs[2], tvs[3]);

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
    /// 绘制Gizmos显示信息
    /// </summary>
    private void OnDrawGizmos()
    {
        if (showFixedBounds)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawWireCube(Vector3.zero, Vector3.one * fixedBoundSize);

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(Vector3.zero, 0.5f);
        }

        if (showAllModules && moduleDatas.Count > 0)
        {
            for (int moduleIndex = 0; moduleIndex < moduleDatas.Count; moduleIndex++)
            {
                ModuleData moduleData = moduleDatas[moduleIndex];

                Gizmos.color = Color.yellow;
                foreach (Vector3 v in moduleData.bottomVertices)
                {
                    Gizmos.DrawSphere(transform.position + v, pointRadius);
                }

                foreach (Vector3 v in moduleData.topVertices)
                {
                    Gizmos.DrawSphere(transform.position + v, pointRadius);
                }

                if (moduleData.bottomVertices.Count >= 4)
                {
                    Gizmos.color = Color.white;
                    Gizmos.DrawLine(transform.position + moduleData.bottomVertices[0], transform.position + moduleData.bottomVertices[1]);
                    Gizmos.DrawLine(transform.position + moduleData.bottomVertices[1], transform.position + moduleData.bottomVertices[2]);
                    Gizmos.DrawLine(transform.position + moduleData.bottomVertices[2], transform.position + moduleData.bottomVertices[3]);
                    Gizmos.DrawLine(transform.position + moduleData.bottomVertices[3], transform.position + moduleData.bottomVertices[0]);
                }

                if (moduleData.topVertices.Count >= 4)
                {
                    Gizmos.color = Color.white;
                    Gizmos.DrawLine(transform.position + moduleData.topVertices[0], transform.position + moduleData.topVertices[1]);
                    Gizmos.DrawLine(transform.position + moduleData.topVertices[1], transform.position + moduleData.topVertices[2]);
                    Gizmos.DrawLine(transform.position + moduleData.topVertices[2], transform.position + moduleData.topVertices[3]);
                    Gizmos.DrawLine(transform.position + moduleData.topVertices[3], transform.position + moduleData.topVertices[0]);
                }

                Gizmos.color = Color.white;
                for (int i = 0; i < 4; i++)
                {
                    if (moduleData.topVertices.Count > i && moduleData.bottomVertices.Count > i)
                    {
                        Gizmos.DrawLine(transform.position + moduleData.bottomVertices[i], transform.position + moduleData.topVertices[i]);
                    }
                }
            }
        }
        else if (moduleDatas.Count > 0)
        {
            List<Vector3> bottomVertices = moduleDatas[0].bottomVertices;
            List<Vector3> topVertices = moduleDatas[0].topVertices;

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

                Gizmos.color = Color.white;
                for (int i = 0; i < 4; i++)
                {
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
}

[System.Serializable]
public class ModuleData
{
    public int vertIndex;
    public List<Vector3> bottomVertices = new List<Vector3>();
    public List<Vector3> topVertices = new List<Vector3>();
    public List<Vector3> bottomNormals = new List<Vector3>();
    public float radius;
}