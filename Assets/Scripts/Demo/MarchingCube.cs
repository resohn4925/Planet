using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MarchingCube : MonoBehaviour
{
    private List<GameObject> modulePrefabsBasic;//可通过空间变换得到所有模块的一组基础模块

    public GameObject moduleCollection;

    public GameObject brush;

    public string folderPath;

    public ModuleCalculate m = new();

    [Header("笔刷模式")]
    [SerializeField]
    private EditMode currentMode;

    [Header("笔刷大小")]
    [Range(0.5f, 5f)]
    [SerializeField]
    private float brushSize = 1f;

    private float currentBrushSize = 0f;

    [Header("实时更新")]
    public bool isUpdate;

    [Header("单位长度")]
    public float spacing = 2f;

    [Header("x行数量")]
    public int rows = 5;

    [Header("z行数量")]
    public int columns = 5;

    [Header("y行数量")]
    public int layers = 5;

    [System.Serializable]
    public enum EditMode { CreateMode, EraserMode }

    public MarchingCubeData marchingCubeData;

    private List<GameObject> moduleInstances;

    public void Init()
    {
        Clear();

        LoadPrefab();//读取指定路径的prefab

        SetPointData();

        marchingCubeData.CalculateModuleName();

        m.ModuleCalcu();//预计算

        UpdateAllModules();
    }

    private void UpdateBrush()
    {
        if (currentBrushSize == brushSize)
        {
            return;
        }

        Vector3 currentScale = brush.transform.localScale;
        currentScale.x = brushSize;
        currentScale.y = brushSize;
        currentScale.z = brushSize;
        brush.transform.localScale = currentScale;

        currentBrushSize = brushSize;
    }

#if UNITY_EDITOR
    public void LoadPrefab()
    {
        modulePrefabsBasic = new List<GameObject>();

        string[] guids = AssetDatabase.FindAssets("t:Prefab", new[] { folderPath });

        foreach (string guid in guids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
            if (prefab != null)
            {
                modulePrefabsBasic.Add(prefab);
                //Debug.Log($"加载预制体: {prefab.name}");
            }
        }
        Debug.Log($"总共加载了 {modulePrefabsBasic.Count} 个预制体");
    }
#endif

    public void SetPointData()
    {
        marchingCubeData = new MarchingCubeData(rows, columns, layers, spacing);
    }

    public void UpdateAllModules()
    {
        Clear();

        moduleInstances = new List<GameObject>();

        if (marchingCubeData.objPointDatas == null || marchingCubeData.modulePointDatas == null)
        {
            Debug.LogWarning("data is null");
            return;
        }

        for (int i = 0; i < marchingCubeData.modulePointDatas.Count; i++)
        {
            GameObject module = InitializeModule(i);
            if (module != null)
            {
                module.transform.SetParent(moduleCollection.transform);
                moduleInstances.Add(module);
            }
        }
    }

    public void UpdateChangedModules()
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

    public void OutPut()
    {
        m.Output();
    }

    public bool IsInsideBrush(Vector3 posPoint, Vector3 brushPoint, float r)
    {
        float dis = Vector3.Distance(posPoint, brushPoint);
        return dis < r;
    }

    public void ModuleCalcu()
    {
        m.ModuleCalcu();
    }

    public void OutPutMapping()
    {
        m.OutPutMapping();
    }

    private void OnDrawGizmos()
    {
        if (marchingCubeData == null || marchingCubeData.objPointDatas == null)
        {
            return;
        }

        Vector3 brushPoint = brush.transform.position;

        //brush
        foreach (MarchingCubeData.ObjPointData pointData in marchingCubeData.objPointDatas)
        {
            Vector3 worldPos = pointData.pos;
            if (IsInsideBrush(worldPos, brushPoint, brushSize / 2))
            {
                pointData.isActive = (currentMode == EditMode.CreateMode);
            }
        }

        //draw obj point
        foreach (MarchingCubeData.ObjPointData pointData in marchingCubeData.objPointDatas)
        {
            Vector3 worldPos = pointData.pos;
            Gizmos.color = pointData.isActive ? Color.red : Color.yellow;
            Gizmos.DrawSphere(worldPos, 0.05f);
        }

        //draw module point
        Gizmos.color = Color.grey;
        foreach (MarchingCubeData.ModulePointData modulePointData in marchingCubeData.modulePointDatas)
        {
            Vector3 worldPos = modulePointData.pos;
            Gizmos.DrawSphere(worldPos, 0.05f);
        }

        if (isUpdate)
        {
            marchingCubeData.CalculateModuleName();
            UpdateChangedModules();
        }

        UpdateBrush();
    }

    public class MarchingCubeData
    {
        public int rows;
        public int columns;
        public int layers;
        public float spacing;

        public List<ObjPointData> objPointDatas = new List<ObjPointData>();

        public MarchingCubeData(int rows, int columns, int layers, float spacing)
        {
            this.rows = rows;
            this.columns = columns;
            this.layers = layers;
            this.spacing = spacing;

            SetPointData();

            SetModulePointData();
        }

        public class ObjPointData
        {
            public int xIndex;
            public int yIndex;
            public int zIndex;
            public Vector3 pos;
            public bool isActive;
        }

        /// <summary>
        /// 设置Obj点阵数组数据
        /// </summary>
        public void SetPointData()
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    for(int k = 0; k < layers; k++)
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
            //foreach(ObjPointData var in objPointDatas)
            //{
            //    Debug.Log(var.isActive);
            //}

            foreach (ModulePointData modulePoint in modulePointDatas)
            {
                bool bottomLeftDown = GetObjPointState(modulePoint.xIndex - 1, modulePoint.zIndex - 1, modulePoint.yIndex - 1); // 左下后
                bool bottomRightDown = GetObjPointState(modulePoint.xIndex - 1, modulePoint.zIndex, modulePoint.yIndex - 1);    // 右下后
                bool topRightDown = GetObjPointState(modulePoint.xIndex, modulePoint.zIndex, modulePoint.yIndex - 1);          // 右前下
                bool topLeftDown = GetObjPointState(modulePoint.xIndex, modulePoint.zIndex - 1, modulePoint.yIndex - 1);        // 左前下

                bool bottomLeftUp = GetObjPointState(modulePoint.xIndex - 1, modulePoint.zIndex - 1, modulePoint.yIndex);      // 左上后
                bool bottomRightUp = GetObjPointState(modulePoint.xIndex - 1, modulePoint.zIndex, modulePoint.yIndex);         // 右上后
                bool topRightUp = GetObjPointState(modulePoint.xIndex, modulePoint.zIndex, modulePoint.yIndex);               // 右前上
                bool topLeftUp = GetObjPointState(modulePoint.xIndex, modulePoint.zIndex - 1, modulePoint.yIndex);             // 左前上

                int blbd = bottomLeftDown ? 1 : 0;   // 下层左下后
                int brbd = bottomRightDown ? 1 : 0;  // 下层右下后
                int brfd = topRightDown ? 1 : 0;     // 下层右前下
                int blfd = topLeftDown ? 1 : 0;      // 下层左前下

                int tlbu = bottomLeftUp ? 1 : 0;     // 上层左上后
                int trbu = bottomRightUp ? 1 : 0;    // 上层右上后
                int trfu = topRightUp ? 1 : 0;       // 上层右前上
                int tlfu = topLeftUp ? 1 : 0;        // 上层左前上

                modulePoint.moduleName = $"{blbd}{brbd}{brfd}{blfd}{tlbu}{trbu}{trfu}{tlfu}";
                //Debug.Log($"{modulePoint.xIndex},{modulePoint.yIndex},{modulePoint.zIndex}:{modulePoint.moduleName}");
            }
        }

        private bool GetObjPointState(int x, int z, int y)
        {
            if (x < 0 || x >= rows || z < 0 || z >= columns || y < 0 || y >= layers)
                return false;

            foreach (ObjPointData objPoint in objPointDatas)
            {
                if (objPoint.xIndex == x && objPoint.zIndex == z && objPoint.yIndex == y)
                {
                    //if (objPoint.isActive)
                    //{
                    //    Debug.Log($"{x}{y}{z}");
                    //}
                    return objPoint.isActive;
                }
            }

            return false;
        }
    }
}