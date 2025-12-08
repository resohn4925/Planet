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
    }

    public class MarchingCubeData
    {
        public int rows;
        public int columns;
        public int layers;
        public float spacing;

        public ObjPointData[,,] objPointArray;

        public List<ObjPointData> objPointDatas = new List<ObjPointData>();

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

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    for (int k = 0; k < layers; k++)
                    {
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
        }
        public void GetPointData(int xIndex, int zIndex)
        {
        }

        public List<ModulePointData> modulePointDatas = new List<ModulePointData>();

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

        private bool GetObjPointState(int x, int z, int y)
        {
            if (x < 0 || x >= rows || z < 0 || z >= columns || y < 0 || y >= layers)
                return false;

            return objPointArray[x, z, y].isActive;
        }
    }
}