using System.Collections.Generic;
using UnityEngine;


public class MarchingSquare : MonoBehaviour
{
    public List<GameObject> modulePrefabs;

    public List<GameObject> modulePrefabsBasic;//可通过空间变换得到所有模块的一组基础模块

    public GameObject moduleCollection;

    public GameObject brush;

    //[Header("笔刷模式")]
    //public bool isCreateMode;

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

    [System.Serializable]
    public enum EditMode { CreateMode, EraserMode }

    private MarchingCubeData marchingSquareData;

    //private List<string> currentModuleNames = new();

    private List<GameObject> moduleInstances;

    //module映射关系
    private Dictionary<string, string> moduleNameToBaseMap = new Dictionary<string, string>();

    private Dictionary<string, float> moduleNameToRotationMap = new Dictionary<string, float>();

    public void Init()
    {
        Clear();

        InitMapping();

        SetPointData();

        marchingSquareData.CalculateModuleName();

        UpdateAllModules();
    }

    #region module映射
    private void InitMapping()
    {
        moduleNameToBaseMap.Clear();
        moduleNameToRotationMap.Clear();

        var groups = new Dictionary<string, (string[] modules, float[] rotations)>
    {
        { "0000", (new[] { "0000" }, new[] { 0f }) },
        { "0001", (new[] { "0001", "0010", "0100", "1000" }, new[] { 0f, 270f, 180f, 90f }) },
        { "0011", (new[] { "0011", "1001", "1100", "0110" }, new[] { 0f, 90f, 180f, 270f }) },
        { "0101", (new[] { "0101", "1010" }, new[] { 0f, 90f }) },
        { "0111", (new[] { "0111", "1101", "1110", "1011" }, new[] { 0f, 180f, 270f, 90f }) },
        { "1111", (new[] { "1111" }, new[] { 0f }) }
    };

        foreach (var group in groups)
        {
            string baseModule = group.Key;
            string[] modules = group.Value.modules;
            float[] rotations = group.Value.rotations;

            for (int i = 0; i < modules.Length; i++)
            {
                moduleNameToBaseMap[modules[i]] = baseModule;
                moduleNameToRotationMap[modules[i]] = rotations[i];
            }
        }
    }

    private (string baseModuleName, float rotation) ModuleMapping(string moduleName)
    {
        if (moduleNameToBaseMap == null || moduleNameToRotationMap == null)
            return (null, 0f);

        //Debug.Log(moduleName + " map to " + moduleNameToBaseMap[moduleName]);

        return (moduleNameToBaseMap[moduleName], moduleNameToRotationMap[moduleName]);
    }

    #endregion

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

    public void SetPointData()
    {
        marchingSquareData = new MarchingCubeData(rows, columns, spacing);
    }

    public void UpdateAllModules()
    {
        Clear();

        moduleInstances = new List<GameObject>();

        if (marchingSquareData.objPointDatas == null || marchingSquareData.modulePointDatas == null)
        {
            Debug.LogWarning("data is null");
            return;
        }

        for (int i = 0; i < marchingSquareData.modulePointDatas.Count; i++)
        {
            GameObject module = InitializeModule(i);
            if (module != null)
            {
                module.transform.SetParent(moduleCollection.transform);
                moduleInstances.Add(module);
                //DestroyImmediate(moduleInstances[i]);
            }
        }
        //Debug.Log(moduleInstances.Count);
    }

    public void UpdateChangedModules()
    {
        if (marchingSquareData.objPointDatas == null || marchingSquareData.modulePointDatas == null)
        {
            Debug.LogWarning("data is null");
            return;
        }

        while (moduleInstances.Count < marchingSquareData.modulePointDatas.Count)
        {
            moduleInstances.Add(null);
        }

        for (int i = 0; i < marchingSquareData.modulePointDatas.Count; i++)
        {
            //string expectedName = marchingSquareData.modulePointDatas[i].moduleName + "(Clone)";
            string expectedName = marchingSquareData.modulePointDatas[i].moduleName;
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
        //foreach(GameObject module in modulePrefabs)
        //{
        //    if(module.name == marchingSquareData.modulePointDatas[moduleIndex].moduleName)
        //    {
        //        //Debug.Log(marchingSquareData.modulePointDatas[moduleIndex].moduleName);

        //        return Instantiate(module,
        //                       marchingSquareData.modulePointDatas[moduleIndex].pos,
        //                       module.transform.rotation); // 使用预制体的旋转数据
        //    }
        //}

        //Debug.LogWarning(marchingSquareData.modulePointDatas[moduleIndex].moduleName + "no module matched");

        //return null;

        string originalModuleName = marchingSquareData.modulePointDatas[moduleIndex].moduleName;
        var mapping = ModuleMapping(originalModuleName);
        string mappedName = mapping.baseModuleName;
        float rotation = mapping.rotation;
        Vector3 position = marchingSquareData.modulePointDatas[moduleIndex].pos;

        //Debug.Log($"查找{originalModuleName}模块旋转{rotation}度后的模块:{mappedName}");

        if (string.IsNullOrEmpty(mappedName))
        {
            Debug.LogWarning($"模块 {originalModuleName} 映射失败，无法找到基础模块");
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

        //GameObject instance = Instantiate(mappedModule, position, Quaternion.Euler(0, rotation, 0));
        GameObject instance = Instantiate(mappedModule, position, finalRotation);
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

    public bool IsInsideBrush(Vector3 posPoint, Vector3 brushPoint, float r)
    {
        float dis = Vector3.Distance(posPoint, brushPoint);
        return dis < r;
    }

    private void OnDrawGizmos()
    {
        if (marchingSquareData == null || marchingSquareData.objPointDatas == null)
        {
            //Debug.LogError("null");
            return;
        }

        Vector3 brushPoint = brush.transform.position;

        //brush
        foreach (MarchingCubeData.ObjPointData pointData in marchingSquareData.objPointDatas)
        {
            Vector3 worldPos = pointData.pos;
            if (IsInsideBrush(worldPos, brushPoint, brushSize / 2))
            {
                pointData.isActive = (currentMode == EditMode.CreateMode);
            }
        }

        //draw obj point
        foreach (MarchingCubeData.ObjPointData pointData in marchingSquareData.objPointDatas)
        {
            Vector3 worldPos = pointData.pos;
            Gizmos.color = pointData.isActive ? Color.red : Color.yellow;
            Gizmos.DrawSphere(worldPos, 0.05f);
        }

        //draw module point
        Gizmos.color = Color.grey;
        foreach (MarchingCubeData.ModulePointData modulePointData in marchingSquareData.modulePointDatas)
        {
            Vector3 worldPos = modulePointData.pos;
            Gizmos.DrawSphere(worldPos, 0.05f);
        }

        marchingSquareData.CalculateModuleName();

        if (isUpdate)
        {
            UpdateChangedModules();
        }

        //foreach (MarchingCubeData.ObjPointData pointData in marchingSquareData.objPointDatas)
        //{
        //    Debug.LogWarning(pointData.isActive);
        //}

        UpdateBrush();
    }

    public class MarchingCubeData
    {
        public int rows;
        public int columns;
        public float spacing;

        public List<ObjPointData> objPointDatas = new List<ObjPointData>();

        public MarchingCubeData(int rows, int columns, float spacing)
        {
            this.rows = rows;
            this.columns = columns;
            this.spacing = spacing;

            SetPointData();

            SetModulePointData();
        }

        public class ObjPointData
        {
            public int xIndex;
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
                    ObjPointData objPointData = new();

                    objPointData.xIndex = i;
                    objPointData.zIndex = j;
                    Vector3 p = new Vector3();
                    p.x = i * spacing + 1f / 2 * spacing;
                    p.z = j * spacing + 1f / 2 * spacing;
                    p.y = 0;
                    objPointData.pos = p;

                    objPointDatas.Add(objPointData);
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
                    ModulePointData modulePointData = new();

                    modulePointData.xIndex = i;
                    modulePointData.zIndex = j;

                    Vector3 p = new Vector3();
                    p.x = i * spacing;
                    p.z = j * spacing;
                    p.y = 0;
                    modulePointData.pos = p;

                    //modulePointData.moduleName = "0000";

                    modulePointDatas.Add(modulePointData);
                }
            }

            CalculateModuleName();
        }

        public void CalculateModuleName()
        {
            if (objPointDatas == null || modulePointDatas == null)
            {
                Debug.LogWarning("data is null");
                return;
            }

            foreach (ModulePointData modulePoint in modulePointDatas)
            {
                bool bottomLeft = GetObjPointState(modulePoint.xIndex - 1, modulePoint.zIndex - 1);
                bool topLeft = GetObjPointState(modulePoint.xIndex - 1, modulePoint.zIndex);
                bool topRight = GetObjPointState(modulePoint.xIndex, modulePoint.zIndex);
                bool bottomRight = GetObjPointState(modulePoint.xIndex, modulePoint.zIndex - 1);

                int bl = bottomLeft ? 1 : 0;
                int tl = topLeft ? 1 : 0;
                int tr = topRight ? 1 : 0;
                int br = bottomRight ? 1 : 0;

                modulePoint.moduleName = $"{bl}{tl}{tr}{br}";
                //Debug.Log(modulePoint.moduleName);
            }
        }

        private bool GetObjPointState(int x, int z)
        {
            if (x < 0 || x >= rows || z < 0 || z >= columns)
                return false;

            foreach (ObjPointData objPoint in objPointDatas)
            {
                if (objPoint.xIndex == x && objPoint.zIndex == z)
                {
                    return objPoint.isActive;
                }

            }

            return false;
        }
    }
}

