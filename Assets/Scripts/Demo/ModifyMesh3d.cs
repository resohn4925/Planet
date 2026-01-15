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
    public void GenerateModule(List<Vector3> bottomPoints, GameObject sourceModule, GameObject parentModule, float height, Matrix4x4 worldTransform)
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
        Vector3 worldPosition = worldTransform.GetPosition();
        Quaternion worldRotation = worldTransform.rotation;
        Vector3 worldScale = worldTransform.lossyScale;

        if (height <= 0)
        {
            height = moduleHeight;
        }

        // 准备底部法线
        List<Vector3> bottomNormals = new List<Vector3>();
        for (int i = 0; i < 4; i++)
        {
            bottomNormals.Add(bottomPoints[i].normalized);
        }

        // 准备顶部点
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

        // 基础变换矩阵，包含缩放
        Quaternion sourceRot = sourceModule.transform.rotation;
        Vector3 sourceScale = sourceModule.transform.lossyScale;

        // 顶点变换矩阵
        Matrix4x4 bakeMatrix = Matrix4x4.TRS(Vector3.zero, sourceRot, sourceScale);

        // 法线变换矩阵
        Matrix4x4 normalMatrix = GetNormalMatrix(sourceScale, sourceRot);

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

            newVerts[i] = TrilinearInterpolation(
                normalized.x, normalized.z, normalized.y,
                bottomPoints, topPoints
            );
        }

        // 计算变形后的法线
        Vector3[] newNormals = CalculateDeformedNormals(
            originalVerts, bakedVerts, bottomPoints, topPoints,
            fixedMin, fixedMax, originalNormals, normalMatrix
        );

        // 计算变形后的切线
        Vector4[] newTangents = CalculateDeformedTangents(
            originalVerts, bakedVerts, bottomPoints, topPoints,
            fixedMin, fixedMax, originalTangents, normalMatrix
        );

        Mesh deformedMesh = new Mesh();
        deformedMesh.name = sourceModule.name + "_modified";
        deformedMesh.vertices = newVerts;
        deformedMesh.uv = originalMesh.uv;

        // 设置法线
        if (newNormals != null && newNormals.Length > 0)
        {
            deformedMesh.normals = newNormals;
        }
        else if (originalMesh.normals != null && originalMesh.normals.Length > 0)
        {
            deformedMesh.normals = originalMesh.normals;
        }

        // 设置切线
        if (newTangents != null && newTangents.Length > 0)
        {
            deformedMesh.tangents = newTangents;
        }
        else if (originalMesh.tangents != null && originalMesh.tangents.Length > 0)
        {
            deformedMesh.tangents = originalMesh.tangents;
        }

        // 复制颜色数据
        if (originalMesh.colors != null && originalMesh.colors.Length > 0)
        {
            deformedMesh.colors = originalMesh.colors;
        }

        // 处理多个子网格，保留原始材质ID
        // 检查是否需要翻转三角形
        bool needFlipTriangles = sourceScale.x * sourceScale.y * sourceScale.z < 0;
        if (needFlipTriangles)
        {
            Debug.Log($"检测到负缩放: {sourceScale}，将翻转三角形顺序");
        }

        // 处理多个子网格
        int subMeshCount = originalMesh.subMeshCount;
        deformedMesh.subMeshCount = subMeshCount;

        for (int i = 0; i < subMeshCount; i++)
        {
            int[] triangles = originalMesh.GetTriangles(i);

            // 如果需要翻转三角形
            if (needFlipTriangles)
            {
                // 翻转每个三角形的顶点顺序
                for (int j = 0; j < triangles.Length; j += 3)
                {
                    int temp = triangles[j];
                    triangles[j] = triangles[j + 2];
                    triangles[j + 2] = temp;
                }
            }

            deformedMesh.SetTriangles(triangles, i);
        }

        // 如果上面没有设置法线，则重新计算
        if ((originalNormals == null || originalNormals.Length == 0) &&
            (newNormals == null || newNormals.Length == 0))
        {
            deformedMesh.RecalculateNormals();
        }

        // 如果上面没有设置切线，则重新计算
        if ((originalTangents == null || originalTangents.Length == 0) &&
            (newTangents == null || newTangents.Length == 0))
        {
            deformedMesh.RecalculateTangents();
        }

        deformedMesh.RecalculateBounds();

        GameObject modifiedObject = new GameObject(sourceModule.name + "_modified");

        // 将新对象设置为module的子对象
        modifiedObject.transform.SetParent(parentModule.transform);

        modifiedObject.transform.localPosition = Vector3.zero;
        modifiedObject.transform.localRotation = Quaternion.identity;
        modifiedObject.transform.localScale = Vector3.one;

        MeshFilter meshFilter = modifiedObject.AddComponent<MeshFilter>();
        meshFilter.sharedMesh = deformedMesh;

        MeshRenderer meshRenderer = modifiedObject.AddComponent<MeshRenderer>();

        // 拷贝材质
        MeshRenderer sourceRenderer = sourceModule.GetComponent<MeshRenderer>();
        if (sourceRenderer != null && sourceRenderer.sharedMaterials.Length > 0)
        {
            Material[] originalMaterials = sourceRenderer.sharedMaterials;
            Material[] newMaterials = new Material[originalMaterials.Length];

            for (int i = 0; i < originalMaterials.Length; i++)
            {
                if (originalMaterials[i] != null)
                {
                    newMaterials[i] = originalMaterials[i];
                }
                else
                {
                    if (defaultMat != null)
                    {
                        newMaterials[i] = defaultMat;
                    }
                    else
                    {
                        newMaterials[i] = new Material(Shader.Find("Standard"));
                    }
                }
            }

            meshRenderer.sharedMaterials = newMaterials;
        }
        else
        {
            if (defaultMat != null)
            {
                meshRenderer.material = defaultMat;
            }
            else
            {
                meshRenderer.material = new Material(Shader.Find("Standard"));
            }
        }

        deformedModules.Add(modifiedObject);

        MeshCollider meshCollider = modifiedObject.AddComponent<MeshCollider>();

        //Debug.Log($"已创建变形模块: {modifiedObject.name}，包含{subMeshCount}个子网格和{meshRenderer.sharedMaterials.Length}个材质");
    }

    /// <summary>
    /// 输入初始点 + 底部控制点 + 高度，返回变形后的位置
    /// </summary>
    /// <param name="initialPoint">初始点的局部坐标（注意：如果原模型有旋转缩放，这里传入的点应当是经过旋转缩放处理后的点）</param>
    /// <param name="modifyPointPos">底部4个控制点的列表</param>
    /// <param name="height">变形模块的高度</param>
    /// <returns>变形后的局部坐标</returns>
    public Vector3 GetDeformedPoint(Vector3 initialPoint, List<Vector3> modifyPointPos, float height)
    {
        // 1. 安全检查
        if (modifyPointPos == null || modifyPointPos.Count < 4)
        {
            Debug.LogError("底部控制点不足4个，无法计算变形");
            return initialPoint;
        }

        // 2. 自动根据底部点和高度，推算顶部点 (Top Vertices)
        // 逻辑需严格保持与 GenerateModule 一致：沿径向向外挤出
        List<Vector3> calculatedTopPoints = new List<Vector3>();
        for (int i = 0; i < 4; i++)
        {
            Vector3 bottomPos = modifyPointPos[i];
            // 原代码逻辑：法线是坐标归一化（适用于球心在原点的情况）
            Vector3 normal = bottomPos.normalized;
            Vector3 topPos = bottomPos + normal * height;
            calculatedTopPoints.Add(topPos);
        }

        // 3. 归一化初始点 (Normalize)
        // 将点映射到 FixedBoundSize 定义的 0~1 空间
        Vector3 fixedMin = Vector3.zero - new Vector3(fixedBoundSize / 2f, fixedBoundSize / 2f, fixedBoundSize / 2f);
        Vector3 fixedMax = Vector3.zero + new Vector3(fixedBoundSize / 2f, fixedBoundSize / 2f, fixedBoundSize / 2f);

        Vector3 normalized = NormalizeToFixedBounds(initialPoint, fixedMin, fixedMax);

        // 4. 三线性插值计算结果
        // 注意参数顺序：X -> u, Z -> v, Y -> w (高度)
        return TrilinearInterpolation(
            normalized.x,
            normalized.z,
            normalized.y,
            modifyPointPos,
            calculatedTopPoints
        );
    }

    /// <summary>
    /// 输入原始网格顶点 + 原始Transform + 底部控制点 + 高度，返回变形后位置
    /// </summary>
    public Vector3 GetDeformedPointWithTransform(Vector3 rawMeshVertex, Transform sourceTransform, List<Vector3> modifyPointPos, float height)
    {
        // 1. 先进行“烘焙”变换 (Bake Transform)
        // 将原始网格点 乘以 缩放和旋转，转换到“固定包围盒”空间
        Quaternion sourceRot = sourceTransform.rotation;
        Vector3 sourceScale = sourceTransform.lossyScale;
        Matrix4x4 bakeMatrix = Matrix4x4.TRS(Vector3.zero, sourceRot, sourceScale);

        Vector3 bakedPoint = bakeMatrix.MultiplyPoint3x4(rawMeshVertex);

        // 2. 调用上面的基础方法计算
        return GetDeformedPoint(bakedPoint, modifyPointPos, height);
    }

    /// <summary>
    /// 获取法线变换矩阵（逆转置矩阵）
    /// </summary>
    private Matrix4x4 GetNormalMatrix(Vector3 scale, Quaternion rotation)
    {
        // 创建缩放矩阵
        Matrix4x4 scaleMatrix = Matrix4x4.Scale(scale);

        // 创建旋转矩阵
        Matrix4x4 rotationMatrix = Matrix4x4.Rotate(rotation);

        // 组合变换矩阵
        Matrix4x4 transformMatrix = rotationMatrix * scaleMatrix;

        // 计算逆转置矩阵
        Matrix4x4 normalMatrix = transformMatrix.inverse.transpose;

        return normalMatrix;
    }

    /// <summary>
    /// 计算变形后的法线（使用微分法）
    /// </summary>
    private Vector3[] CalculateDeformedNormals(
        Vector3[] originalVerts, Vector3[] bakedVerts,
        List<Vector3> bottomPoints, List<Vector3> topPoints,
        Vector3 fixedMin, Vector3 fixedMax,
        Vector3[] originalNormals, Matrix4x4 normalMatrix)
    {
        if (originalNormals == null || originalNormals.Length == 0)
            return null;

        Vector3[] newNormals = new Vector3[originalNormals.Length];

        // 微分步长
        float delta = 0.001f;

        for (int i = 0; i < bakedVerts.Length; i++)
        {
            // 获取归一化坐标
            Vector3 coord = NormalizeToFixedBounds(bakedVerts[i], fixedMin, fixedMax);

            // 计算当前顶点的位置
            Vector3 currentPos = TrilinearInterpolation(
                coord.x, coord.z, coord.y,
                bottomPoints, topPoints
            );

            // 计算X轴方向（使用前向差分或后向差分）
            Vector3 pRight;
            if (coord.x < 0.5f)
                pRight = (TrilinearInterpolation(coord.x + delta, coord.z, coord.y, bottomPoints, topPoints) - currentPos).normalized;
            else
                pRight = (currentPos - TrilinearInterpolation(coord.x - delta, coord.z, coord.y, bottomPoints, topPoints)).normalized;

            // 计算Z轴方向
            Vector3 pForward;
            if (coord.z < 0.5f)
                pForward = (TrilinearInterpolation(coord.x, coord.z + delta, coord.y, bottomPoints, topPoints) - currentPos).normalized;
            else
                pForward = (currentPos - TrilinearInterpolation(coord.x, coord.z - delta, coord.y, bottomPoints, topPoints)).normalized;

            // 计算Y轴方向
            Vector3 pUp;
            if (coord.y < 0.5f)
                pUp = (TrilinearInterpolation(coord.x, coord.z, coord.y + delta, bottomPoints, topPoints) - currentPos).normalized;
            else
                pUp = (currentPos - TrilinearInterpolation(coord.x, coord.z, coord.y - delta, bottomPoints, topPoints)).normalized;

            // 使用逆转置矩阵正确变换原始法线
            Vector3 rawNormal = normalMatrix.MultiplyVector(originalNormals[i]).normalized;

            // 将变换后的法线投影到变形后的局部坐标系
            Vector3 finalNormal = (pRight * rawNormal.x + pUp * rawNormal.y + pForward * rawNormal.z).normalized;
            newNormals[i] = finalNormal;
        }

        return newNormals;
    }

    /// <summary>
    /// 计算变形后的切线
    /// </summary>
    private Vector4[] CalculateDeformedTangents(
        Vector3[] originalVerts, Vector3[] bakedVerts,
        List<Vector3> bottomPoints, List<Vector3> topPoints,
        Vector3 fixedMin, Vector3 fixedMax,
        Vector4[] originalTangents, Matrix4x4 normalMatrix)
    {
        if (originalTangents == null || originalTangents.Length == 0)
            return null;

        Vector4[] newTangents = new Vector4[originalTangents.Length];

        // 微分步长
        float delta = 0.001f;

        for (int i = 0; i < bakedVerts.Length; i++)
        {
            // 获取归一化坐标
            Vector3 coord = NormalizeToFixedBounds(bakedVerts[i], fixedMin, fixedMax);

            // 计算当前顶点的位置
            Vector3 currentPos = TrilinearInterpolation(
                coord.x, coord.z, coord.y,
                bottomPoints, topPoints
            );

            // 计算X轴方向
            Vector3 pRight;
            if (coord.x < 0.5f)
                pRight = (TrilinearInterpolation(coord.x + delta, coord.z, coord.y, bottomPoints, topPoints) - currentPos).normalized;
            else
                pRight = (currentPos - TrilinearInterpolation(coord.x - delta, coord.z, coord.y, bottomPoints, topPoints)).normalized;

            // 计算Z轴方向
            Vector3 pForward;
            if (coord.z < 0.5f)
                pForward = (TrilinearInterpolation(coord.x, coord.z + delta, coord.y, bottomPoints, topPoints) - currentPos).normalized;
            else
                pForward = (currentPos - TrilinearInterpolation(coord.x, coord.z - delta, coord.y, bottomPoints, topPoints)).normalized;

            // 计算Y轴方向
            Vector3 pUp;
            if (coord.y < 0.5f)
                pUp = (TrilinearInterpolation(coord.x, coord.z, coord.y + delta, bottomPoints, topPoints) - currentPos).normalized;
            else
                pUp = (currentPos - TrilinearInterpolation(coord.x, coord.z, coord.y - delta, bottomPoints, topPoints)).normalized;

            // 使用逆转置矩阵正确变换原始切线
            Vector3 rawTangent = normalMatrix.MultiplyVector(
                new Vector3(originalTangents[i].x, originalTangents[i].y, originalTangents[i].z)
            ).normalized;

            // 将变换后的切线投影到变形后的局部坐标系
            Vector3 finalTangent = (pRight * rawTangent.x + pUp * rawTangent.y + pForward * rawTangent.z).normalized;
            newTangents[i] = new Vector4(finalTangent.x, finalTangent.y, finalTangent.z, originalTangents[i].w);
        }

        return newTangents;
    }

    /// <summary>
    /// 生成一系列变形模块
    /// </summary>
    //public void GenerateModule()
    //{
    //    ClearAllModules();
    //    GenerateQuad();

    //    for (int i = 0; i < moduleDatas.Count; i++)
    //    {
    //        ModuleData moduleData = moduleDatas[i];

    //        GameObject sourceModule = i < moduleList.Count ? moduleList[i] : targetModule;
    //        if (sourceModule == null)
    //        {
    //            Debug.LogWarning($"模块{i}没有对应的预制体，跳过");
    //            continue;
    //        }

    //        // 使用统一的GetGenerateModule方法
    //        GetGenerateModule(moduleData.bottomVertices, sourceModule, moduleHeight);
    //    }
    //}

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
    /// 使用固定边界计算相对坐标
    /// </summary>
    Vector3 NormalizeToFixedBounds(Vector3 point, Vector3 min, Vector3 max)
    {
        float x = (point.x - min.x) / (max.x - min.x);
        float y = (point.y - min.y) / (max.y - min.y);
        float z = (point.z - min.z) / (max.z - min.z);

        return new Vector3(x, y, z);
    }

    /// <summary>
    /// 三线性插值
    /// </summary>
    Vector3 TrilinearInterpolation(float u, float v, float w, List<Vector3> bvs, List<Vector3> tvs)
    {
        Vector3 bottomPos = BilinearInterpolation(u, v,
            bvs[0], bvs[1],
            bvs[2], bvs[3]);

        Vector3 topPos = BilinearInterpolation(u, v,
            tvs[0], tvs[1],
            tvs[2], tvs[3]);

        return Vector3.LerpUnclamped(bottomPos, topPos, w);
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