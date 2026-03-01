using Enum;
using System.Collections.Generic;
using TowerStacker;
using UnityEditor;
using UnityEngine;

public class MarchingCube : MonoBehaviour
{
    public float spacing = 5f;
    public int rows = 10;
    public int columns = 10;
    public int layers = 3;
    public bool showModifyPoint;
    public bool isShowGeo;//显示几何调试信息

    public GameObject slopePrefab;
    public GameObject cliffPrefab;

    private List<GameObject> slopeInstances;
    private List<GameObject> cliffInstances;

    public ModuleCalculate m = new();

    [Header("基础模块数据")]
    public string modulePath = "Assets/Resources/Prefabs/Modules_Building_Yoka";
    private List<GameObject> modulePrefabsBasic;
    [HideInInspector] public List<GameObject> moduleInstances;
    public GameObject moduleCollection;

    [Header("hint数据")]
    public GameObject hintPrefab;
    [HideInInspector] public List<GameObject> hintInstances;
    public GameObject hintModuleCollection;

    [Header("地形数据")]
    public TerrainDataSO terrainDataSO;

    private List<Vector3> targetMeshData;

    public List<MarchingCubeData> marchingCubeDatas;

    public MarchingCubeData marchingCubeData;

    [System.Serializable]
    public enum EditMode
    {
        Terrain,
        Slope,
        Cliff
    }
    public void InitAllMarchingCubeDatas(int faceNum)
    {
        Clear();

        InitModule();

        RemoveAllObj(EditMode.Slope);

        LoadPrefab();

        marchingCubeDatas = new List<MarchingCubeData>();
        CubeFace[] allFaces = (CubeFace[])System.Enum.GetValues(typeof(CubeFace));

        for (int i = 0; i < Mathf.Min(faceNum, allFaces.Length); i++)
        {
            marchingCubeData = new MarchingCubeData(rows, columns, layers, spacing);
            marchingCubeData.cubeFace = allFaces[i];
            marchingCubeDatas.Add(marchingCubeData);
        }

        m.ModuleCalcu();

        if (terrainDataSO != null)
        {
            //LoadTerrainData();

            //// 重新生成所有面
            //for (int i = 0; i < marchingCubeDatas.Count; i++)
            //{
            //    marchingCubeDatas[i].CalculateModuleName();
            //    UpdateModules(marchingCubeDatas[i]);
            //    UpdateAllObj(EditMode.Slope);
            //}
        }

        else
        {
            for (int i = 0; i < marchingCubeDatas.Count; i++)
            {
                marchingCubeDatas[i].CalculateModuleName();
            }
        }
    }

    public void LoadPrefab()
    {
//#if UNITY_EDITOR
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
//#endif
    }

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

        //初始化hint
        if (hintInstances == null)
        {
            hintInstances = new List<GameObject>();
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

        if (terrainDataSO.faceDataList.Count == 0)
        {
            Debug.LogWarning("TerrainDataSO中无地形数据");
            return;
        }

        // 加载每个面的数据
        for (int faceIndex = 0; faceIndex < Mathf.Min(marchingCubeDatas.Count, terrainDataSO.faceDataList.Count); faceIndex++)
        {
            FaceData faceData = terrainDataSO.faceDataList[faceIndex];
            MarchingCubeData currentCubeData = marchingCubeDatas[faceIndex];

            int index = 0;
            for (int x = 0; x < rows + 2; x++)
            {
                for (int z = 0; z < columns + 2; z++)
                {
                    for (int y = 0; y < layers; y++)
                    {
                        var point = currentCubeData.objPointArray[x, z, y];
                        point.isActive = faceData.isActiveList[index];
                        point.isSlope = faceData.isSlopeList[index];
                        point.slopeRotation = faceData.slopeRotation[index];
                        point.isCliff = faceData.isCliffList[index];
                        point.cliffRotation = faceData.cliffRotation[index];
                        index++;
                    }
                }
            }
        }

        Debug.Log($"地形数据已加载: {terrainDataSO.saveTime}，加载了 {Mathf.Min(marchingCubeDatas.Count, terrainDataSO.faceDataList.Count)} 个面");
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
        terrainDataSO.faceDataList.Clear();

        // 存储所有面的数据
        for (int faceIndex = 0; faceIndex < marchingCubeDatas.Count; faceIndex++)
        {
            MarchingCubeData currentCubeData = marchingCubeDatas[faceIndex];
            FaceData faceData = new FaceData();

            for (int x = 0; x < rows + 2; x++)
            {
                for (int z = 0; z < columns + 2; z++)
                {
                    for (int y = 0; y < layers; y++)
                    {
                        var point = currentCubeData.objPointArray[x, z, y];
                        faceData.isActiveList.Add(point.isActive);
                        faceData.isSlopeList.Add(point.isSlope);
                        faceData.slopeRotation.Add(point.slopeRotation);
                        faceData.isCliffList.Add(point.isCliff);
                        faceData.cliffRotation.Add(point.cliffRotation);
                    }
                }
            }

            terrainDataSO.faceDataList.Add(faceData);
        }

        terrainDataSO.saveTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(terrainDataSO);
        UnityEditor.AssetDatabase.SaveAssets();
#endif

        Debug.Log($"地形数据已保存: {terrainDataSO.saveTime}，保存了 {marchingCubeDatas.Count} 个面");
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

    #region 基础obj生成与销毁
    public void UpdateModules(MarchingCubeData marchingCubeData)
    {
        marchingCubeData.CalculateModuleName();

        if (marchingCubeData.objPointDatas == null || marchingCubeData.modulePointDatas == null)
        {
            Debug.LogWarning("data is null");
            return;
        }

        foreach (GameObject module in marchingCubeData.faceModuleInstances)
        {
            if (module != null)
            {
                DestroyImmediate(module);
            }
        }
        marchingCubeData.faceModuleInstances.Clear();

        while (marchingCubeData.faceModuleInstances.Count < marchingCubeData.modulePointDatas.Count)
        {
            marchingCubeData.faceModuleInstances.Add(null);
        }

        for (int i = 0; i < marchingCubeData.modulePointDatas.Count; i++)
        {
            string expectedName = marchingCubeData.modulePointDatas[i].moduleName;
            bool needsUpdate = marchingCubeData.faceModuleInstances[i] == null ||
                              marchingCubeData.faceModuleInstances[i].name != expectedName;

            if (needsUpdate)
            {
                // 销毁旧模块实例
                if (marchingCubeData.faceModuleInstances[i] != null)
                {
                    DestroyImmediate(marchingCubeData.faceModuleInstances[i]);
                }

                // 初始化和设置新模块
                GameObject module = InitializeModule(marchingCubeData, i);
                if (module != null)
                {
                    module.transform.SetParent(moduleCollection.transform);
                    marchingCubeData.faceModuleInstances[i] = module;
                }
            }
        }
    }

    public GameObject InitializeModule(MarchingCubeData marchingCubeData, int moduleIndex)
    {
        if (marchingCubeData.modulePointDatas[moduleIndex].yIndex == 0)
        {
            return null;
        }

        string originalModuleName = marchingCubeData.modulePointDatas[moduleIndex].moduleName;

        var mapping = m.GetModuleMapping(originalModuleName);
        string mappedName = mapping.baseModule;

        float rotation = mapping.rotation;
        bool isMirror = mapping.mirrored;

        Vector3 position = marchingCubeData.modulePointDatas[moduleIndex].pos;

        if (string.IsNullOrEmpty(mappedName))
        {
            Debug.LogWarning($"模块 {originalModuleName} 映射失败，无法找到基础模块");
            return null;
        }

        if (mappedName == "00001111" || mappedName == "00000000")
        {
            return null;
        }

        string targetModuleName = mappedName;
        
        if (marchingCubeData.modulePointDatas[moduleIndex].yIndex == 1)
        {
            string variantName = mappedName + "_B";
            bool variantExists = false;
            
            if (modulePrefabsBasic != null && modulePrefabsBasic.Count > 0)
            {
                var variantModule = modulePrefabsBasic.Find(obj =>
                    obj.name == variantName || obj.name == variantName + ".prefab");
                
                if (variantModule != null)
                {
                    variantExists = true;
                    targetModuleName = variantName;
                }
            }
        }

        GameObject mappedModule = null;
        if (modulePrefabsBasic == null || modulePrefabsBasic.Count == 0)
        {
            mappedModule = CreateVariantModule(targetModuleName);
        }
        else
        {
            mappedModule = modulePrefabsBasic.Find(obj =>
                obj.name == targetModuleName || obj.name == targetModuleName + ".prefab");
        }

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
        // 清除全局模块实例列表
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

        // 清除每个面的模块实例列表
        if (marchingCubeDatas != null)
        {
            foreach (var marchingCubeData in marchingCubeDatas)
            {
                if (marchingCubeData.faceModuleInstances != null)
                {
                    foreach (GameObject module in marchingCubeData.faceModuleInstances)
                    {
                        if (module != null)
                        {
                            DestroyImmediate(module);
                        }
                    }
                    marchingCubeData.faceModuleInstances.Clear();
                }
            }
        }
    }
    #endregion

    #region Hint模块生成与销毁
    public void UpdateHint(MarchingCube marchingCube)
    {
        //生成hintmodule,hintmodule是一个完整的obj，实例化hintprefab并存储在已声明的hintInstances中
        ClearFaceHintInstances(marchingCubeData);
        if (hintPrefab == null)
        {
            Debug.LogWarning("hintPrefab未赋值，无法生成提示物体");
            return;
        }

        //foreach (var hintPoint in marchingCubeData.hintObjPointDatas)
        //{
        //    if (hintPoint.isActive)
        //    {
        //        CreateHintInstance(marchingCubeData, hintPoint);
        //    }
        //}
        foreach (var marchingCubeData in marchingCube.marchingCubeDatas)
        {
            ClearFaceHintInstances(marchingCubeData);

            if (hintPrefab == null)
            {
                Debug.LogWarning("hintPrefab未赋值，无法生成提示物体");
                continue;
            }

            foreach (var hintPoint in marchingCubeData.hintObjPointDatas)
            {
                if (hintPoint.isActive)
                {
                    CreateHintInstance(marchingCubeData, hintPoint);
                }
            }
        }
    }

    public void ClearFaceHintInstances(MarchingCubeData marchingCubeData)
    {
        if (marchingCubeData.faceHintInstances == null) return;

        // 销毁实例并从全局列表中移除
        foreach (var hint in marchingCubeData.faceHintInstances)
        {
            if (hint != null)
            {
                DestroyImmediate(hint);
                hintInstances.Remove(hint);
            }
        }
        marchingCubeData.faceHintInstances.Clear();
    }

    private void CreateHintInstance(MarchingCubeData marchingCubeData, MarchingCubeData.ObjPointData hintPoint)
    {
        // 实例化hint预制体
        GameObject hintInstance = Instantiate(hintPrefab, hintModuleCollection.transform);

        hintInstance.transform.position = hintPoint.pos;

        hintInstance.name = $"Hint_{marchingCubeData.cubeFace}_{hintPoint.xIndex}_{hintPoint.zIndex}_{hintPoint.yIndex}";

        marchingCubeData.faceHintInstances.Add(hintInstance);
        hintInstances.Add(hintInstance);
    }

    public void ClearAllHintInstances()
    {
        if (hintInstances == null) return;

        foreach (var hint in hintInstances)
        {
            if (hint != null)
            {
                DestroyImmediate(hint);
            }
        }
        hintInstances.Clear();

        if (marchingCubeDatas == null) return;
        foreach (var data in marchingCubeDatas)
        {
            data.faceHintInstances?.Clear();
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

        if (marchingCubeDatas == null || !isShowGeo) return;

        //绘制objpoint
        //foreach (var data in marchingCubeDatas)
        //{
        //    foreach (MarchingCubeData.ObjPointData pointData in data.objPointDatas)
        //    {
        //        Vector3 worldPos = pointData.pos;
        //        if (data.cubeFace == CubeFace.Back && pointData.xIndex == 0 && pointData.zIndex == 1)
        //        {
        //            Gizmos.color = Color.red;
        //            Gizmos.DrawSphere(worldPos, 0.5f);
        //        }

        //        else
        //        {
        //            Gizmos.color = Color.yellow;
        //            Gizmos.DrawSphere(worldPos, 0.25f);
        //        }
        //    }
        //}

        //绘制objpoint(变形后)
        Gizmos.color = Color.yellow;
        foreach (MarchingCubeData marchingCubeData in marchingCubeDatas)
        {
            foreach (MarchingCubeData.ModifyModuleData modifyModuleData in marchingCubeData.modifyModuleDatas)
            {
                Vector3 worldPos = modifyModuleData.pos;
                Gizmos.DrawSphere(worldPos, 0.3f);
            }
        }

        //绘制modulepoint
        //Gizmos.color = Color.grey;
        //foreach (MarchingCubeData.ModulePointData modulePointData in marchingCubeData.modulePointDatas)
        //{
        //    Vector3 worldPos = modulePointData.pos;
        //    Gizmos.DrawSphere(worldPos, 0.5f);
        //}

        //if (marchingCubeData.modifyPointDatas == null || !showModifyPoint) return;

        Gizmos.color = Color.green;
        foreach (MarchingCubeData marchingCubeData in marchingCubeDatas)
        {
            foreach (MarchingCubeData.ModifyPointData modifyPointData in marchingCubeData.modifyPointDatas)
            {
                Vector3 worldPos = modifyPointData.pos;
                Gizmos.DrawSphere(worldPos, 0.3f);
            }
        }
    }

    public class MarchingCubeData
    {
        public int rows;
        public int columns;
        public int layers;
        public float spacing;
        public CubeFace cubeFace;

        //modifymesh相关
        public float sphereRadius;
        public float moduleHeight;

        //基础obj相关
        public ObjPointData[,,] objPointArray;
        public List<ObjPointData> objPointDatas = new List<ObjPointData>();
        public List<ModulePointData> modulePointDatas = new List<ModulePointData>();
        public List<ModifyPointData> modifyPointDatas = new List<ModifyPointData>();//module的边界点
        public List<ModifyModuleData> modifyModuleDatas = new List<ModifyModuleData>();//obj的边界点
        public List<GameObject> faceModuleInstances = new List<GameObject>();

        //hintobj相关
        public ObjPointData[,,] hintObjPointArray;
        public List<ObjPointData> hintObjPointDatas = new List<ObjPointData>();
        public List<GameObject> faceHintInstances = new List<GameObject>();

        public MarchingCubeData(int rows, int columns, int layers, float spacing)
        {
            this.rows = rows;
            this.columns = columns;
            this.layers = layers;
            this.spacing = spacing;

            SetObjPointData();
            SetModulePointData();
            SetHintObjPointData();
        }

        public class ObjPointData
        {
            public BuildingType type;
            public Face face;
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
            int extendedRows = rows + 2;
            int extendedColumns = columns + 2;

            objPointArray = new ObjPointData[extendedRows, extendedColumns, layers];
            objPointDatas.Clear();

            // 扩展后的obj点阵
            for (int i = 0; i < extendedRows; i++)
            {
                for (int j = 0; j < extendedColumns; j++)
                {
                    for (int k = 0; k < layers; k++)
                    {
                        ObjPointData objPointData = new();

                        objPointData.xIndex = i;
                        objPointData.zIndex = j;
                        objPointData.yIndex = k;

                        Vector3 p = new Vector3();
                        p.x = (i - 0.5f) * spacing;
                        p.z = (j - 0.5f) * spacing;
                        p.y = k * spacing + 1f / 2 * spacing;
                        objPointData.pos = p;
                        objPointData.isSlope = false;
                        objPointData.slopeRotation = 0f;
                        objPointData.isActive = false;

                        objPointArray[i, j, k] = objPointData;
                        objPointDatas.Add(objPointData);
                    }
                }
            }
        }

        public class ModulePointData
        {
            //type
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

        public class ModifyModuleData
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
        }

        public void SetModifyModuleData(List<ModifyModuleData> datas)
        {
            modifyModuleDatas = new List<ModifyModuleData>(datas);
        }

        public void SetHintObjPointData()
        {
            int extendedRows = rows + 2;
            int extendedColumns = columns + 2;

            hintObjPointArray = new ObjPointData[extendedRows, extendedColumns, layers];
            hintObjPointDatas.Clear();

            for (int i = 0; i < extendedRows; i++)
            {
                for (int j = 0; j < extendedColumns; j++)
                {
                    for (int k = 0; k < layers; k++)
                    {
                        ObjPointData objPointData = new ObjPointData();

                        objPointData.xIndex = i;
                        objPointData.zIndex = j;
                        objPointData.yIndex = k;

                        Vector3 p = new Vector3();
                        p.x = (i - 0.5f) * spacing;
                        p.z = (j - 0.5f) * spacing;
                        p.y = k * spacing + 1f / 2 * spacing;
                        objPointData.pos = p;
                        objPointData.isActive = false;  // 默认不激活

                        hintObjPointArray[i, j, k] = objPointData;
                        hintObjPointDatas.Add(objPointData);
                    }
                }
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

        /// <summary>
        /// 根据module计算modifypoint
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public List<Vector3> GetModifyPointsAroundModule(int x, int y, int z)
        {
            List<Vector3> modifyPointPos = new();

            (int, int, int)[] points =
            {
        (x, y, z),
        (x, y, z + 1),
        (x + 1, y, z + 1),
        (x + 1, y, z)
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

        /// <summary>
        /// 根据module计算modifyModulePoint
        /// </summary>
        /// <param name="x">module的x索引</param>
        /// <param name="y">module的y索引（层索引）</param>
        /// <param name="z">module的z索引</param>
        /// <returns>周围4个modifyModuleData的pos列表</returns>
        public List<Vector3> GetModifyModulePointsAroundModule(int x, int y, int z)
        {
            List<Vector3> modifyModulePos = new List<Vector3>();

            // 复用原有的4个周围点索引规则（和Point点逻辑一致）
            (int, int, int)[] points =
            {
        (x, y, z),
        (x, y, z + 1),
        (x + 1, y, z + 1),
        (x + 1, y, z)
    };

            for (int i = 0; i < points.Length; i++)
            {
                var (px, py, pz) = points[i];
                ModifyModuleData found = null;

                foreach (ModifyModuleData data in modifyModuleDatas)
                {
                    if (data.xIndex == px && data.yIndex == py && data.zIndex == pz)
                    {
                        found = data;
                        break;
                    }
                }

                if (found == null)
                {
                    Debug.LogWarning($"未找到ModifyModuleData：索引({px},{py},{pz})");
                    modifyModulePos.Add(Vector3.zero);
                    continue;
                }

                // Debug.Log($"Module索引({px},{py},{pz}), 位置:{found.pos.ToString()}");
                modifyModulePos.Add(found.pos);
            }

            return modifyModulePos;
        }

        private bool GetObjPointState(int x, int z, int y)
        {
            // 计算在扩展数组中的索引
            int extendedX = x + 1;
            int extendedZ = z + 1;

            // 检查边界
            if (extendedX < 0 || extendedX >= rows + 2 ||
                extendedZ < 0 || extendedZ >= columns + 2 ||
                y < 0 || y >= layers)
            {
                return false;
            }

            return objPointArray[extendedX, extendedZ, y].isActive;
        }
    }

    /// <summary>
    /// 在变体模式下动态创建基础模块
    /// </summary>
    private GameObject CreateVariantModule(string moduleName)
    {
        // 创建一个空的GameObject作为基础模块
        GameObject variantModule = new GameObject(moduleName);
        
        // 根据模块名称的不同，添加不同的组件和配置
        // 这里可以根据实际需求添加不同的几何形状或组件
        
        // 添加一个简单的立方体作为基础几何形状
        MeshFilter meshFilter = variantModule.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = variantModule.AddComponent<MeshRenderer>();
        
        // 创建一个简单的立方体网格
        Mesh cubeMesh = CreateCubeMesh();
        meshFilter.mesh = cubeMesh;
        
        // 设置默认材质
        meshRenderer.material = new Material(Shader.Find("Standard"));
        
        // 设置模块的默认大小
        variantModule.transform.localScale = Vector3.one * spacing;
        
        return variantModule;
    }

    /// <summary>
    /// 创建一个简单的立方体网格
    /// </summary>
    private Mesh CreateCubeMesh()
    {
        Mesh mesh = new Mesh();
        
        // 立方体的顶点
        Vector3[] vertices = new Vector3[]
        {
            new Vector3(-0.5f, -0.5f, -0.5f),
            new Vector3(0.5f, -0.5f, -0.5f),
            new Vector3(0.5f, 0.5f, -0.5f),
            new Vector3(-0.5f, 0.5f, -0.5f),
            new Vector3(-0.5f, 0.5f, 0.5f),
            new Vector3(0.5f, 0.5f, 0.5f),
            new Vector3(0.5f, -0.5f, 0.5f),
            new Vector3(-0.5f, -0.5f, 0.5f)
        };
        
        // 立方体的三角形索引
        int[] triangles = new int[]
        {
            // 前面
            0, 2, 1, 0, 3, 2,
            // 上面
            2, 3, 4, 2, 4, 5,
            // 后面
            1, 2, 5, 1, 5, 6,
            // 下面
            0, 7, 4, 0, 4, 3,
            // 左面
            0, 1, 6, 0, 6, 7,
            // 右面
            4, 7, 6, 4, 6, 5
        };
        
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        
        return mesh;
    }
}