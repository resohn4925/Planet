using QFramework;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEditor;
using UnityEngine;
using static MarchingCube.MarchingCubeData;

public class MarchingCube : MonoBehaviour
{
    private List<GameObject> modulePrefabsBasic;

    public GameObject moduleCollection;

    public string modulePath = "Assets/Art/Scene/OpenWorld/Template_Module/Prefab/Modules3d_Terrain";

    public GameObject slopePrefab;

    public GameObject cliffPrefab;

    private List<GameObject> slopeInstances;

    private List<GameObject> cliffInstances;

    public ModuleCalculate m = new();

    public float spacing = 2f;

    public int rows = 10;

    public int columns = 10;

    public int layers = 3;

    [Header("地形数据")]
    public TerrainDataSO terrainDataSO;

    //MeshModify
    public Material defaultMat;

    private List<Vector3> targetMeshData;

    [System.Serializable]
    public enum EditMode
    {
        Terrain,
        Slope,
        Cliff
    }

    public MarchingCubeData marchingCubeData;

    private List<GameObject> moduleInstances;

    public ObjPointData[,,] objPointArray;

    public ModulePointData[,,] modulePointArray;

    public bool isShowGeo;//显示几何调试信息

    public void Init()
    {
        Clear();

        InitModule();

        RemoveAllObj(EditMode.Slope);

        LoadPrefab();

        SetPointData();

        m.ModuleCalcu();

        //如果有地形数据就读取，否则手动计算
        if (terrainDataSO != null)
        {
            LoadTerrainData();
        }
        else
        {
            marchingCubeData.CalculateModuleName();
        }
    }

#if UNITY_EDITOR
    public void LoadPrefab()
    {
        modulePrefabsBasic = new List<GameObject>();

        string[] guids = AssetDatabase.FindAssets("t:Prefab", new[] { modulePath });

        foreach (string guid in guids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
            if (prefab != null)
            {
                modulePrefabsBasic.Add(prefab);
            }
        }
        Debug.Log($"总共加载了 {modulePrefabsBasic.Count} 个预制体");
    }
#endif

    public void InitModule()
    {
        if (moduleInstances == null)
        {
            moduleInstances = new List<GameObject>();
        }

        if (slopeInstances == null)
        {
            slopeInstances = new List<GameObject>();
        }
    }

    #region 设置数据
    public void LoadTerrainData()
    {
        if (terrainDataSO == null)
        {
            Debug.LogWarning("未设置 TerrainDataSO，无法加载");
            return;
        }

        // 检查数据尺寸是否匹配
        if (terrainDataSO.rows != rows || terrainDataSO.columns != columns || terrainDataSO.layers != layers)
        {
            Debug.LogWarning($"尺寸不匹配,加载地形的尺寸为x{terrainDataSO.rows},y{terrainDataSO.layers},z{terrainDataSO.columns}");
            return;
        }

        int index = 0;
        for (int x = 0; x < rows; x++)
        {
            for (int z = 0; z < columns; z++)
            {
                for (int y = 0; y < layers; y++)
                {
                    var point = marchingCubeData.objPointArray[x, z, y];
                    point.isActive = terrainDataSO.isActiveList[index];
                    point.isSlope = terrainDataSO.isSlopeList[index];
                    point.slopeRotation = terrainDataSO.slopeRotation[index];
                    point.isCliff = terrainDataSO.isCliffList[index];
                    point.cliffRotation = terrainDataSO.cliffRotation[index];

                    index++;
                }
            }
        }

        // 重新生成
        marchingCubeData.CalculateModuleName();
        UpdateModules();
        UpdateAllObj(EditMode.Slope);

        Debug.Log($"地形数据已加载: {terrainDataSO.saveTime}");
    }

    public void SaveTerrainData()
    {
        if (terrainDataSO == null)
        {
            Debug.LogWarning("未设置 TerrainDataSO，无法保存");
            return;
        }

        // 保存配置
        terrainDataSO.rows = rows;
        terrainDataSO.columns = columns;
        terrainDataSO.layers = layers;
        terrainDataSO.spacing = spacing;

        // 清空旧数据
        terrainDataSO.isActiveList.Clear();
        terrainDataSO.isSlopeList.Clear();
        terrainDataSO.slopeRotation.Clear();
        terrainDataSO.isCliffList.Clear();
        terrainDataSO.cliffRotation.Clear();

        // 存储新数据
        for (int x = 0; x < rows; x++)
        {
            for (int z = 0; z < columns; z++)
            {
                for (int y = 0; y < layers; y++)
                {
                    var point = marchingCubeData.objPointArray[x, z, y];
                    terrainDataSO.isActiveList.Add(point.isActive);
                    terrainDataSO.isSlopeList.Add(point.isSlope);
                    terrainDataSO.slopeRotation.Add(point.slopeRotation);
                    terrainDataSO.isCliffList.Add(point.isCliff);
                    terrainDataSO.cliffRotation.Add(point.cliffRotation);
                }
            }
        }

        // 记录时间戳
        terrainDataSO.saveTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        // 标记为已修改
#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(terrainDataSO);
        UnityEditor.AssetDatabase.SaveAssets();
#endif

        Debug.Log($"地形数据已保存: {terrainDataSO.saveTime}");
    }

    public void SetPointData()
    {
        marchingCubeData = new MarchingCubeData(rows, columns, layers, spacing);
    }

    public void ModuleCalcu()
    {
        m.ModuleCalcu();
    }
    #endregion

    #region 地形模块生成与销毁
    public void UpdateModules()
    {
        marchingCubeData.CalculateModuleName();

        if (marchingCubeData.objPointDatas == null || marchingCubeData.modulePointDatas == null)
        {
            Debug.LogWarning("data is null");
            return;
        }

        while (moduleInstances.Count < marchingCubeData.modulePointDatas.Count)
        {
            moduleInstances.Add(null);
        }

        for (int i = 0; i < marchingCubeData.modulePointDatas.Count; i++)
        {
            string expectedName = marchingCubeData.modulePointDatas[i].moduleName;
            bool needsUpdate = moduleInstances[i] == null ||
                              moduleInstances[i].name != expectedName;

            if (needsUpdate)
            {
                //destroy old module
                if (moduleInstances[i] != null)
                {
                    DestroyImmediate(moduleInstances[i]);
                }

                //initialize and set new module
                GameObject module = InitializeModule(i);
                if (module != null)
                {
                    module.transform.SetParent(moduleCollection.transform);
                    moduleInstances[i] = module;
                }
            }
        }
    }

    public GameObject InitializeModule(int moduleIndex)
    {
        string originalModuleName = marchingCubeData.modulePointDatas[moduleIndex].moduleName;

        var mapping = m.GetModuleMapping(originalModuleName);
        string mappedName = mapping.baseModule;

        //Debug.Log($"基础模块: {mapping.baseModule}, 旋转: {mapping.rotation}°, 镜像: {mapping.mirrored}");

        float rotation = mapping.rotation;
        bool isMirror = mapping.mirrored;

        Vector3 position = marchingCubeData.modulePointDatas[moduleIndex].pos;

        //Debug.Log($"查找{originalModuleName}模块旋转{rotation}度后的模块:{mappedName}");

        if (string.IsNullOrEmpty(mappedName))
        {
            Debug.LogWarning($"模块 {originalModuleName} 映射失败，无法找到基础模块");
            return null;
        }

        if (mappedName == "00001111" || mappedName == "00000000")
        {
            return null;
        }

        GameObject mappedModule = modulePrefabsBasic.Find(obj =>
            obj.name == mappedName);

        if (mappedModule == null)
        {
            Debug.LogWarning($"在 modulePrefabs 中找不到映射的模块: {mappedName} (原模块: {originalModuleName})");
            return null;
        }

        Quaternion finalRotation = Quaternion.Euler(0, rotation, 0);
        Vector3 originalScale = mappedModule.transform.localScale;
        Vector3 finalScale = originalScale;

        if (isMirror)
        {
            finalScale.x = -originalScale.x;
            finalRotation = finalRotation * Quaternion.Euler(0, 180, 0);
        }

        GameObject instance = Instantiate(mappedModule, position, finalRotation);
        instance.transform.localScale = finalScale;
        instance.name = originalModuleName;

        return instance;
    }

    public void Clear()
    {
        if (moduleInstances != null)
        {
            foreach (GameObject module in moduleInstances)
            {
                if (module != null)
                {
                    DestroyImmediate(module);
                }
            }

            moduleInstances.Clear();
        }
    }
    #endregion

    #region 散件生成与销毁
    public void UpdateChangedObj(int x, int z, int y, bool isCreate, float rotation, EditMode editMode)
    {
        if (marchingCubeData.objPointDatas == null || marchingCubeData.modulePointDatas == null)
        {
            Debug.LogWarning("data is null");
            return;
        }

        if (x < 0 || x >= marchingCubeData.rows ||
            z < 0 || z >= marchingCubeData.columns ||
            y < 0 || y >= marchingCubeData.layers)
        {
            //Debug.LogWarning("index out of range");
            return;
        }

        Vector3 pos = marchingCubeData.objPointArray[x, z, y].pos;
        Debug.Log($"物件{x},{z},{y}的状态设置为{isCreate}");

        switch (editMode)
        {
            case EditMode.Slope:
                if (!marchingCubeData.objPointArray[x, z, y].isSlope)
                {
                    CreateObj(pos, rotation, editMode);
                    marchingCubeData.objPointArray[x, z, y].isSlope = true;
                    marchingCubeData.objPointArray[x, z, y].slopeRotation = rotation;
                }
                else
                {
                    RemoveObjAtPosition(pos, editMode);
                    marchingCubeData.objPointArray[x, z, y].isSlope = false;
                    marchingCubeData.objPointArray[x, z, y].slopeRotation = 0f;
                }
                return;

            case EditMode.Cliff:
                if (!marchingCubeData.objPointArray[x, z, y].isCliff)
                {
                    CreateObj(pos, rotation, editMode);
                    marchingCubeData.objPointArray[x, z, y].isCliff = true;
                    marchingCubeData.objPointArray[x, z, y].cliffRotation = rotation;
                }
                else
                {
                    RemoveObjAtPosition(pos, editMode);
                    marchingCubeData.objPointArray[x, z, y].isCliff = false;
                    marchingCubeData.objPointArray[x, z, y].cliffRotation = 0f;
                }
                return;
        }

    }

    public void UpdateAllObj(EditMode editMode)
    {
        RemoveAllObj(editMode);

        if (marchingCubeData.objPointDatas == null || marchingCubeData.modulePointDatas == null)
        {
            Debug.LogWarning("data is null");
            return;
        }

        foreach (MarchingCubeData.ObjPointData obj in marchingCubeData.objPointArray)
        {
            switch (editMode)
            {
                case EditMode.Slope:
                    if (obj.isSlope)
                    {
                        CreateObj(obj.pos, obj.slopeRotation, editMode);
                    }
                    return;
                case EditMode.Cliff:
                    if (obj.isCliff)
                    {
                        CreateObj(obj.pos, obj.cliffRotation, editMode);
                    }
                    return;
            }
        }
    }

    public void CreateObj(Vector3 pos, float rotation, EditMode editMode)
    {
        switch (editMode)
        {
            case EditMode.Slope:
                GameObject slopeInstance = Instantiate(slopePrefab, moduleCollection.transform);
                slopeInstance.transform.position = pos;
                slopeInstance.transform.rotation = Quaternion.Euler(0, rotation, 0);
                slopeInstances.Add(slopeInstance);
                return;

            case EditMode.Cliff:
                GameObject cliffInstance = Instantiate(cliffPrefab, moduleCollection.transform);
                cliffInstance.transform.position = pos;
                cliffInstance.transform.rotation = Quaternion.Euler(0, rotation, 0);
                cliffInstances.Add(cliffInstance);
                return;
        }
    }

    private void RemoveAllObj(EditMode editMode)
    {
        switch (editMode)
        {
            case EditMode.Slope:
                if (slopeInstances == null) return;
                foreach (GameObject slope in slopeInstances)
                {
                    if (slope != null)
                    {
                        DestroyImmediate(slope);
                    }
                }
                slopeInstances.Clear();
                return;
            case EditMode.Cliff:
                if (cliffInstances == null) return;
                foreach (GameObject cliff in cliffInstances)
                {
                    if (cliff != null)
                    {
                        DestroyImmediate(cliff);
                    }
                }
                cliffInstances.Clear();
                return;
        }
    }

    private void RemoveObjAtPosition(Vector3 pos, EditMode editMode)
    {
        switch (editMode)
        {
            case EditMode.Slope:
                if (slopeInstances == null) return;
                for (int i = slopeInstances.Count - 1; i >= 0; i--)
                {
                    if (slopeInstances[i] != null &&
                        slopeInstances[i].transform.position == pos)
                    {
                        DestroyImmediate(slopeInstances[i]);
                        slopeInstances.RemoveAt(i);
                        return;
                    }
                }
                return;
            case EditMode.Cliff:
                if (cliffInstances == null) return;
                for (int i = cliffInstances.Count - 1; i >= 0; i--)
                {
                    if (cliffInstances[i] != null &&
                        cliffInstances[i].transform.position == pos)
                    {
                        DestroyImmediate(cliffInstances[i]);
                        cliffInstances.RemoveAt(i);
                        return;
                    }
                }
                return;
        }
    }
    #endregion

    #region MeshModify
    /// <summary>
    /// 使用双线性插值算法把目标点的位置变换到mesh中
    /// </summary>
    public void ApplyModifyMesh()
    {
        if (moduleInstances == null || moduleInstances.Count == 0)
        {
            Debug.LogWarning("无模块实例");
            return;
        }

        for (int i = 0; i < marchingCubeData.modulePointDatas.Count; i++)
        {
            if (moduleInstances[i] == null) continue;

            var modulePoint = marchingCubeData.modulePointDatas[i];

            targetMeshData = new List<Vector3>();

            int moduleIndexX = marchingCubeData.modulePointDatas[i].xIndex;
            int moduleIndexY = marchingCubeData.modulePointDatas[i].yIndex;
            int moduleIndexZ = marchingCubeData.modulePointDatas[i].zIndex;
            //Debug.Log($"{moduleIndexX},{moduleIndexY},{moduleIndexZ}对应模块为{moduleInstances[i].name}");

            if (moduleIndexX - 1 < 0 || moduleIndexX > marchingCubeData.columns - 1 || moduleIndexZ - 1 < 0 || moduleIndexZ > marchingCubeData.rows - 1 || moduleIndexY < 0 || moduleIndexY > marchingCubeData.layers - 1) continue;
            targetMeshData.Add(marchingCubeData.objPointArray[moduleIndexX - 1, moduleIndexZ - 1, moduleIndexY].pos);
            targetMeshData.Add(marchingCubeData.objPointArray[moduleIndexX - 1, moduleIndexZ, moduleIndexY].pos);
            targetMeshData.Add(marchingCubeData.objPointArray[moduleIndexX, moduleIndexZ, moduleIndexY].pos);
            targetMeshData.Add(marchingCubeData.objPointArray[moduleIndexX, moduleIndexZ - 1, moduleIndexY].pos);

            CreateTransformedMesh(moduleInstances[i], moduleInstances[i].name);
        }
    }

    public void CreateTransformedMesh(GameObject targetObj, string moduleName)
    {
        if (targetObj == null)
        {
            Debug.LogError("target mesh is null");
            return;
        }

        MeshFilter meshFilter = targetObj.GetComponent<MeshFilter>();
        MeshRenderer meshRenderer = targetObj.GetComponent<MeshRenderer>();

        if (meshFilter == null || meshFilter.sharedMesh == null)
        {
            Debug.LogError("no mesh found in the target mesh.");
            return;
        }

        // 记录原始物件transform
        Quaternion originalRotation = targetObj.transform.rotation;
        Vector3 originalScale = targetObj.transform.localScale;
        Vector3 originalPosition = targetObj.transform.position;

        // 检测是否是镜像
        bool isMirrored = originalScale.x < 0;

        // 获取原始网格
        Mesh originalMesh = meshFilter.sharedMesh;

        // 创建网格实例
        Mesh newMesh = new Mesh();

        //复制基础顶点数据
        newMesh.vertices = originalMesh.vertices.Clone() as Vector3[];
        newMesh.normals = originalMesh.normals.Clone() as Vector3[];
        newMesh.uv = originalMesh.uv.Clone() as Vector2[];
        newMesh.colors = originalMesh.colors.Clone() as Color[];
        newMesh.tangents = originalMesh.tangents.Clone() as Vector4[];

        //复制所有子网格信息
        int subMeshCount = originalMesh.subMeshCount;
        newMesh.subMeshCount = subMeshCount;

        for (int i = 0; i < subMeshCount; i++)
        {
            int[] triangles = originalMesh.GetTriangles(i);

            // 如果镜像，反转三角形顶点顺序
            if (isMirrored)
            {
                for (int j = 0; j < triangles.Length; j += 3)
                {
                    int temp = triangles[j];
                    triangles[j] = triangles[j + 2];
                    triangles[j + 2] = temp;
                }
            }

            newMesh.SetTriangles(triangles, i);
        }

        // 应用原始变换到顶点
        Vector3[] vertices = newMesh.vertices;
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i].x *= originalScale.x;
            vertices[i].y *= originalScale.y;
            vertices[i].z *= originalScale.z;

            vertices[i] = originalRotation * vertices[i];
        }
        newMesh.vertices = vertices;

        // 如果镜像，反转法线方向
        if (isMirrored)
        {
            Vector3[] normals = newMesh.normals;
            for (int i = 0; i < normals.Length; i++)
            {
                // 反转X轴的法线分量
                normals[i].x = -normals[i].x;
            }
            newMesh.normals = normals;
        }

        // 重置目标物体的变换
        targetObj.transform.localScale = Vector3.one;
        Vector3 pos = new Vector3(0, targetObj.transform.position.y, 0);
        targetObj.transform.position = pos;
        targetObj.transform.rotation = Quaternion.identity;
        newMesh.name = "Transformed_" + moduleName;

        float minX = -spacing / 2;
        float maxX = spacing / 2;
        float minZ = -spacing / 2;
        float maxZ = spacing / 2;

        float averageY = 0f;
        for (int i = 0; i < targetMeshData.Count; i++)
        {
            averageY += targetMeshData[i].y;
        }
        averageY /= targetMeshData.Count;

        for (int i = 0; i < vertices.Length; i++)
        {
            float u = Mathf.InverseLerp(minX, maxX, vertices[i].x);
            float v = Mathf.InverseLerp(minZ, maxZ, vertices[i].z);

            Vector3 interpolatedPosition = BilinearInterpolation(u, v,
                targetMeshData[0], targetMeshData[1],
                targetMeshData[2], targetMeshData[3]);

            float heightOffset = vertices[i].y - averageY;
            interpolatedPosition.y += heightOffset;

            vertices[i] = interpolatedPosition;
        }

        newMesh.vertices = vertices;

        // 重新计算法线
        newMesh.RecalculateNormals();
        newMesh.RecalculateBounds();
        newMesh.RecalculateTangents();

        meshFilter.sharedMesh = newMesh;

        // 复制材质数组
        if (meshRenderer != null && meshRenderer.sharedMaterials != null)
        {
            Material[] originalMaterials = meshRenderer.sharedMaterials;
            meshRenderer.sharedMaterials = originalMaterials;
        }
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
    #endregion

    public void OutPut()
    {
        m.Output();
    }

    private void OnDrawGizmos()
    {
        if (marchingCubeData == null || marchingCubeData.objPointDatas == null)
        {
            return;
        }

        if (!isShowGeo) return;

        //foreach (MarchingCubeData.ObjPointData pointData in marchingCubeData.objPointDatas)
        //{
        //    Vector3 worldPos = pointData.pos;
        //    Gizmos.color = pointData.isActive ? Color.red : Color.yellow;
        //    Gizmos.DrawSphere(worldPos, 0.5f);
        //}

        //draw module point
        //Gizmos.color = Color.grey;
        //foreach (MarchingCubeData.ModulePointData modulePointData in marchingCubeData.modulePointDatas)
        //{
        //    Vector3 worldPos = modulePointData.pos;
        //    Gizmos.DrawSphere(worldPos, 0.5f);
        //}

        if (marchingCubeData.modifyPointDatas == null) return;
        Gizmos.color = Color.green;
        foreach (MarchingCubeData.ModifyPointData modifyPointData in marchingCubeData.modifyPointDatas)
        {
            Vector3 worldPos = modifyPointData.pos;
            Gizmos.DrawSphere(worldPos, 0.4f);
        }
    }

    public class MarchingCubeData
    {
        public int rows;
        public int columns;
        public int layers;
        public float spacing;

        //modifymesh相关
        public float sphereRadius;
        public float moduleHeight;

        public ObjPointData[,,] objPointArray;

        public List<ObjPointData> objPointDatas = new List<ObjPointData>();

        public List<ModulePointData> modulePointDatas = new List<ModulePointData>();

        public List<ModifyPointData> modifyPointDatas = new List<ModifyPointData>();

        public MarchingCubeData(int rows, int columns, int layers, float spacing)
        {
            this.rows = rows;
            this.columns = columns;
            this.layers = layers;
            this.spacing = spacing;

            SetObjPointData();
            SetModulePointData();
        }

        public class ObjPointData
        {
            public int xIndex;
            public int yIndex;
            public int zIndex;
            public Vector3 pos;
            public bool isActive;
            public bool isSlope;
            public float slopeRotation;
            public bool isCliff;
            public float cliffRotation;
        }

        /// <summary>
        /// 设置Obj点阵数组数据
        /// </summary>
        public void SetObjPointData()
        {
            objPointArray = new ObjPointData[rows, columns, layers];
            objPointDatas.Clear();

            //网格不变形的情况
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    for (int k = 0; k < layers; k++)
                    {
                        continue;
                        ObjPointData objPointData = new();

                        objPointData.xIndex = i;
                        objPointData.zIndex = j;
                        objPointData.yIndex = k;
                        Vector3 p = new Vector3();
                        p.x = i * spacing + 1f / 2 * spacing;
                        p.z = j * spacing + 1f / 2 * spacing;
                        p.y = k * spacing + 1f / 2 * spacing;
                        objPointData.pos = p;
                        objPointData.isSlope = false;
                        objPointData.slopeRotation = 0f;

                        objPointArray[i, j, k] = objPointData;
                        objPointDatas.Add(objPointData);
                    }
                }
            }

            //网格变形的情况
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    float randomOffsetX = 0f;
                    float randomOffsetZ = 0f;

                    // 只在非边缘位置生成随机偏移
                    if (i != 0 && i != rows - 1 && j != 0 && j != columns - 1)
                    {
                        randomOffsetX = Random.Range(-0.15f, 0.15f) * spacing;
                        randomOffsetZ = Random.Range(-0.15f, 0.15f) * spacing;
                    }

                    for (int k = 0; k < layers; k++)
                    {
                        ObjPointData objPointData = new();

                        objPointData.xIndex = i;
                        objPointData.zIndex = j;
                        objPointData.yIndex = k;

                        Vector3 p = new Vector3();
                        p.x = i * spacing + 1f / 2 * spacing + randomOffsetX;
                        p.z = j * spacing + 1f / 2 * spacing + randomOffsetZ;
                        p.y = k * spacing + 1f / 2 * spacing;

                        objPointData.pos = p;
                        objPointData.isSlope = false;
                        objPointData.slopeRotation = 0f;

                        objPointArray[i, j, k] = objPointData;
                        objPointDatas.Add(objPointData);
                    }
                }
            }
        }

        public class ModulePointData
        {
            public int xIndex;
            public int yIndex;
            public int zIndex;
            public Vector3 pos;
            public string moduleName;
        }

        /// <summary>
        /// 设置module点阵数据
        /// </summary>
        public void SetModulePointData()
        {
            for (int i = 0; i < rows + 1; i++)
            {
                for (int j = 0; j < columns + 1; j++)
                {
                    for (int k = 0; k < layers + 1; k++)
                    {
                        ModulePointData modulePointData = new();

                        modulePointData.xIndex = i;
                        modulePointData.zIndex = j;
                        modulePointData.yIndex = k;

                        Vector3 p = new Vector3();
                        p.x = i * spacing;
                        p.z = j * spacing;
                        p.y = k * spacing;
                        modulePointData.pos = p;

                        modulePointDatas.Add(modulePointData);
                    }
                }
            }
        }

        public class ModifyPointData
        {
            public int xIndex;
            public int yIndex;
            public int zIndex;
            public Vector3 pos;
            public Vector3 normal;
        }

        public void SetModifyPointData(List<ModifyPointData> datas)
        {
            modifyPointDatas = new List<ModifyPointData>(datas);
            foreach (ModifyPointData data in modifyPointDatas)
            {
                Debug.Log($"ModifyPointData: 索引({data.xIndex},{data.yIndex},{data.zIndex}) 位置:{data.pos}");
            }
        }

        public void CalculateModuleName()
        {
            foreach (ModulePointData modulePoint in modulePointDatas)
            {
                bool bottomLeftDown = GetObjPointState(modulePoint.xIndex - 1, modulePoint.zIndex - 1, modulePoint.yIndex - 1);
                bool bottomRightDown = GetObjPointState(modulePoint.xIndex - 1, modulePoint.zIndex, modulePoint.yIndex - 1);
                bool topRightDown = GetObjPointState(modulePoint.xIndex, modulePoint.zIndex, modulePoint.yIndex - 1);
                bool topLeftDown = GetObjPointState(modulePoint.xIndex, modulePoint.zIndex - 1, modulePoint.yIndex - 1);

                bool bottomLeftUp = GetObjPointState(modulePoint.xIndex - 1, modulePoint.zIndex - 1, modulePoint.yIndex);
                bool bottomRightUp = GetObjPointState(modulePoint.xIndex - 1, modulePoint.zIndex, modulePoint.yIndex);
                bool topRightUp = GetObjPointState(modulePoint.xIndex, modulePoint.zIndex, modulePoint.yIndex);
                bool topLeftUp = GetObjPointState(modulePoint.xIndex, modulePoint.zIndex - 1, modulePoint.yIndex);           

                int blbd = bottomLeftDown ? 1 : 0;
                int brbd = bottomRightDown ? 1 : 0;
                int brfd = topRightDown ? 1 : 0;
                int blfd = topLeftDown ? 1 : 0;

                int tlbu = bottomLeftUp ? 1 : 0;
                int trbu = bottomRightUp ? 1 : 0;
                int trfu = topRightUp ? 1 : 0;
                int tlfu = topLeftUp ? 1 : 0;

                modulePoint.moduleName = $"{blbd}{brbd}{brfd}{blfd}{tlbu}{trbu}{trfu}{tlfu}";
                //Debug.Log($"{modulePoint.xIndex},{modulePoint.yIndex},{modulePoint.zIndex}:{modulePoint.moduleName}");
            }
        }

        public List<Vector3> PrintModifyPointsAroundModule(int x, int y, int z)
        {
            List<Vector3> modifyPointPos = new();

            (int, int, int)[] points =
            {
        (x, y, z),
        (x, y, z + 1),
        (x + 1, y, z),
        (x + 1, y, z + 1)
    };
            for (int i = 0; i < points.Length; i++)
            {
                var (px, py, pz) = points[i];
                ModifyPointData found = null;

                foreach (ModifyPointData data in modifyPointDatas)
                {
                    if (data.xIndex == px && data.yIndex == py && data.zIndex == pz)
                    {
                        found = data;
                        break;
                    }
                }

                //Debug.Log($"索引({px},{py},{pz}), 位置:{(found != null ? found.pos.ToString() : "未找到")}");
                modifyPointPos.Add(found.pos);
            }
            return modifyPointPos;
        }

        private bool GetObjPointState(int x, int z, int y)
        {
            if (x < 0 || x >= rows || z < 0 || z >= columns || y < 0 || y >= layers)
                return false;

            return objPointArray[x, z, y].isActive;
        }
    }
}