using Enum;
using System.Collections.Generic;
using UnityEngine;
using Utility;
using static UnityEngine.Mesh;

namespace Planet
{
    public class PlanetPipeline : MonoBehaviour
    {
        [Header("脚本引用")]
        public MeshGenerator meshGenerator;
        public MarchingCube marchingCube;
        public ModifyMesh3d modifyMesh3D;
        public BuildingSystemOnSphere buildingSystemOnSphere;
        public RippleEffectURP rippleEffectURP;

        [Header("网格生成参数")]
        public int columnsPerFace;//每面行数
        public int meshSize;
        public float planetRadius;
        public int layers;
        public float height;
        public float heightOffSet;


        [Header("Debug参数")]
        public int faceIndex;
        public int activeObjXIndex;
        public int activeObjYIndex;
        public int activeObjZIndex;
        public bool isActivate;
        public bool showVerts;
        public bool showMeshs;
        public bool activeSurround = false;

        [Header("Prefab")]
        public GameObject light;

        [HideInInspector] public bool isEditing;

        private List<Vector3> modifyPointPos = new();
        private List<MarchingCube.MarchingCubeData.ModifyPointData> modifyPointDatas = new();
        private List<Vector3> modifyModulePos = new();
        private List<MarchingCube.MarchingCubeData.ModifyModuleData> modifyModuleDatas = new();
        private GameObject modifiedRoot;
        private GameObject root;

        #region 初始化
        public void Init()
        {
            //配置参数更新
            meshGenerator.meshNum = columnsPerFace * 2 + 2;
            meshGenerator.meshSize = meshSize;
            meshGenerator.sphereRadius = planetRadius;
            meshGenerator.showVerts = showVerts;
            meshGenerator.showMeshs = showMeshs;

            marchingCube.layers = layers;
            marchingCube.columns = columnsPerFace;
            marchingCube.rows = columnsPerFace;

            //建造系统初始化
            buildingSystemOnSphere.Init(marchingCube);
            //如果还在编辑模式，先退出
            buildingSystemOnSphere.SwitchEditMode(false);

            rippleEffectURP.Init();

            GenerateMesh();
            InitMarchingData();

            CreateRoot();
            CreateModifiedRoot();

            SetAllModifyPointDatas();
            SetAllObjPointDatas();

            ClearAllModules();
        }

        public void GenerateMesh()
        {
            meshGenerator.GenarateMesh();
        }

        public void InitMarchingData()
        {
            marchingCube.InitAllMarchingCubeDatas(6);
        }

        public void CreateRoot()
        {
            // 查找所有名为 "Root" 的节点，包括隐藏的
            GameObject[] rootObjects = Resources.FindObjectsOfTypeAll<GameObject>();
            root = null;
            foreach (GameObject obj in rootObjects)
            {
                if (obj.name == "Root" && obj.scene.IsValid()) // 确保是场景中的对象，不是预制体
                {
                    root = obj;
                    break;
                }
            }

            if (root == null)
            {
                root = new GameObject("Root");
                // 创建后立即将 Root 节点设置为不可见，这样它的子物件也会不可见
                root.SetActive(false);
                Debug.Log("创建 Root 节点并设置为不可见");
            }
            else
            {
                // 复用已存在的 Root 节点
                Debug.Log("复用已存在的 Root 节点");
            }
            marchingCube.moduleCollection = root;
        }

        public void CreateModifiedRoot()
        {
            modifiedRoot = GameObject.Find("ModifiedRoot");
            if (modifiedRoot != null)
            {
                DestroyImmediate(modifiedRoot);
            }
            modifiedRoot = new GameObject("ModifiedRoot");

            Debug.Log("创建 ModifiedRoot 节点");
        }

        public void SetAllModifyPointDatas()
        {
            foreach (var meshData in meshGenerator.meshDataList)
            {
                SetAllModifyModuleDatas(meshData);
                SetAllModifyPointDatas(meshData);
            }
        }

        /// <summary>
        /// meshdata数据计算modifymodule,赋值给marchingcubedata
        /// </summary>
        /// <param name="meshData"></param>
        public void SetAllModifyModuleDatas(MeshData meshData)
        {
            if (columnsPerFace == -2) return;
            else
                height = (float)meshSize / (columnsPerFace + 2);
            int verNum = meshGenerator.meshNum + 1;

            modifyPointPos.Clear();
            modifyPointDatas.Clear();

            List<int> uniqueXIndices = new List<int>();
            List<int> uniqueYIndices = new List<int>();

            foreach (Vector3 indexVec in meshData.modifyVertices)
            {
                int originalX = (int)indexVec.x;
                int originalY = (int)indexVec.y;

                if (!(originalX % 2 == 0 && originalY % 2 == 0))
                    continue;

                if (!uniqueXIndices.Contains(originalX))
                    uniqueXIndices.Add(originalX);

                if (!uniqueYIndices.Contains(originalY))
                    uniqueYIndices.Add(originalY);
            }

            uniqueXIndices.Sort();
            uniqueYIndices.Sort();

            Dictionary<int, int> xIndexMap = new Dictionary<int, int>();
            Dictionary<int, int> yIndexMap = new Dictionary<int, int>();

            for (int i = 0; i < uniqueXIndices.Count; i++)
                xIndexMap[uniqueXIndices[i]] = i;

            for (int i = 0; i < uniqueYIndices.Count; i++)
                yIndexMap[uniqueYIndices[i]] = i;

            List<MarchingCube.MarchingCubeData.ModifyPointData> layer0Data = new List<MarchingCube.MarchingCubeData.ModifyPointData>();

            foreach (Vector3 indexVec in meshData.modifyVertices)
            {
                int originalX = (int)indexVec.x;
                int originalY = (int)indexVec.y;

                if (!(originalX % 2 == 0 && originalY % 2 == 0))
                    continue;

                int vertexIndex = originalY * verNum + originalX;

                if (vertexIndex >= 0 && vertexIndex < meshData.vertices.Count)
                {
                    Vector3 actualPos = meshData.vertices[vertexIndex];
                    Vector3 contractedPos = ExpandPosition(actualPos, -height / 2f - heightOffSet);
                    modifyPointPos.Add(contractedPos);

                    int mappedX = xIndexMap[originalX];
                    int mappedY = yIndexMap[originalY];

                    MarchingCube.MarchingCubeData.ModifyPointData data = new MarchingCube.MarchingCubeData.ModifyPointData
                    {
                        xIndex = mappedX,
                        yIndex = 0,
                        zIndex = mappedY,
                        pos = contractedPos,
                        normal = actualPos.normalized
                    };

                    layer0Data.Add(data);
                    modifyPointDatas.Add(data);
                }
            }

            int totalLayers = layers + 1;
            GenerateMultiLayerData(layer0Data, totalLayers);

            foreach (var marchingCubeData in marchingCube.marchingCubeDatas)
            {
                if (marchingCubeData.cubeFace == meshData.faceType)
                {
                    marchingCubeData.SetModifyPointData(modifyPointDatas);
                }
            }
        }

        /// <summary>
        /// meshdata数据计算modifypoint,赋值给marchingcubedata
        /// </summary>
        /// <param name="meshData"></param>
        public void SetAllModifyPointDatas(MeshData meshData)
        {
            if (columnsPerFace == -2) return;
            height = (float)meshSize / (columnsPerFace + 2);
            int verNum = meshGenerator.meshNum + 1;
            int maxIndex = verNum - 1;

            modifyModulePos.Clear();
            modifyModuleDatas.Clear();

            List<Vector3> filteredModifyVertices = new List<Vector3>();
            foreach (Vector3 indexVec in meshData.modifyVertices)
            {
                int originalX = (int)indexVec.x;
                int originalY = (int)indexVec.y;

                bool isXValid = (originalX == 0 || originalX == 1)
                             || (originalX >= 3 && originalX % 2 == 1)
                             || originalX == maxIndex;
                bool isYValid = (originalY == 0 || originalY == 1)
                             || (originalY >= 3 && originalY % 2 == 1)
                             || originalY == maxIndex;

                if (isXValid && isYValid)
                {
                    filteredModifyVertices.Add(indexVec);
                }
            }

            List<int> uniqueXIndices = new List<int>();
            List<int> uniqueYIndices = new List<int>();

            foreach (Vector3 indexVec in filteredModifyVertices)
            {
                int originalX = (int)indexVec.x;
                int originalY = (int)indexVec.y;

                if (!uniqueXIndices.Contains(originalX))
                    uniqueXIndices.Add(originalX);

                if (!uniqueYIndices.Contains(originalY))
                    uniqueYIndices.Add(originalY);
            }

            uniqueXIndices.Sort();
            uniqueYIndices.Sort();

            Dictionary<int, int> xIndexMap = new Dictionary<int, int>();
            Dictionary<int, int> yIndexMap = new Dictionary<int, int>();

            for (int i = 0; i < uniqueXIndices.Count; i++)
                xIndexMap[uniqueXIndices[i]] = i;

            for (int i = 0; i < uniqueYIndices.Count; i++)
                yIndexMap[uniqueYIndices[i]] = i;

            List<MarchingCube.MarchingCubeData.ModifyModuleData> layer0Data = new List<MarchingCube.MarchingCubeData.ModifyModuleData>();

            foreach (Vector3 indexVec in filteredModifyVertices)
            {
                int originalX = (int)indexVec.x;
                int originalY = (int)indexVec.y;
                int vertexIndex = originalY * verNum + originalX;

                if (vertexIndex >= 0 && vertexIndex < meshData.vertices.Count)
                {
                    Vector3 actualPos = meshData.vertices[vertexIndex];

                    // 第一层向内收缩,距离为地表偏移
                    Vector3 contractedPos = ExpandPosition(actualPos, -heightOffSet);
                    modifyModulePos.Add(contractedPos);

                    int mappedX = xIndexMap[originalX];
                    int mappedY = yIndexMap[originalY];

                    MarchingCube.MarchingCubeData.ModifyModuleData data = new MarchingCube.MarchingCubeData.ModifyModuleData
                    {
                        xIndex = mappedX,
                        yIndex = 0,
                        zIndex = mappedY,
                        pos = contractedPos,
                        normal = actualPos.normalized
                    };

                    layer0Data.Add(data);
                    modifyModuleDatas.Add(data);
                }
            }

            // 其他层数据
            int totalLayers = layers;
            GenerateMultiLayerModuleData(layer0Data, totalLayers);

            // 赋值给marchingCubeData
            foreach (var marchingCubeData in marchingCube.marchingCubeDatas)
            {
                if (marchingCubeData.cubeFace == meshData.faceType)
                {
                    marchingCubeData.SetModifyModuleData(modifyModuleDatas);
                }
            }
        }

        /// <summary>
        /// 生成Module点的多层数据
        /// </summary>
        /// <param name="baseLayerData">基础层数据（layer0）</param>
        /// <param name="totalLayers">总层数</param>
        private void GenerateMultiLayerModuleData(List<MarchingCube.MarchingCubeData.ModifyModuleData> baseLayerData, int totalLayers)
        {
            if (baseLayerData == null || baseLayerData.Count == 0)
                return;

            // 从第二层开始生成
            for (int layerIndex = 1; layerIndex < totalLayers; layerIndex++)
            {
                float currentLayerHeight = layerIndex * height;

                foreach (var basePoint in baseLayerData)
                {
                    // 计算当前层的位置
                    Vector3 layerPos = ExpandPosition(basePoint.pos, currentLayerHeight);

                    MarchingCube.MarchingCubeData.ModifyModuleData layerData = new MarchingCube.MarchingCubeData.ModifyModuleData
                    {
                        xIndex = basePoint.xIndex,
                        yIndex = layerIndex, // 层索引递增
                        zIndex = basePoint.zIndex,
                        pos = layerPos,
                        normal = basePoint.normal
                    };

                    modifyModuleDatas.Add(layerData);
                }

                Debug.Log($"Module点生成了第{layerIndex}层的{baseLayerData.Count}个点");
            }
        }

        /// <summary>
        /// 把objpointdata从平面赋值给六面体
        /// </summary>
        public void SetAllObjPointDatas()
        {
            if (marchingCube == null || marchingCube.marchingCubeDatas == null) return;

            foreach (var data in marchingCube.marchingCubeDatas)
            {
                float halfSize = data.rows * data.spacing * 0.5f;

                Quaternion faceRotation = GetFaceRotation(data.cubeFace);

                foreach (var point in data.objPointDatas)
                {
                    float centeredX = point.pos.x - halfSize;
                    float centeredZ = point.pos.z - halfSize;

                    float surfaceY = halfSize + point.pos.y;

                    Vector3 localPos = new Vector3(centeredX, surfaceY, centeredZ);

                    point.pos = faceRotation * localPos;
                }
            }
        }

        /// <summary>
        /// 辅助函数：根据面类型获取旋转四元数
        /// </summary>
        private Quaternion GetFaceRotation(CubeFace faceType)
        {
            switch (faceType)
            {
                case CubeFace.Top:
                    return Quaternion.Euler(180, 0, 0);
                case CubeFace.Bottom:
                    return Quaternion.Euler(0, 0, 0);
                case CubeFace.Front:
                    return Quaternion.Euler(90, 0, 0);
                case CubeFace.Back:
                    return Quaternion.Euler(-90, 0, 0);
                case CubeFace.Left:
                    return Quaternion.Euler(90, 0, 90);
                case CubeFace.Right:
                    return Quaternion.Euler(-90, 0, -90);
                default:
                    return Quaternion.identity;
            }
        }

        /// <summary>
        /// 生成每层的modifypointdata
        /// </summary>
        /// <param name="baseLayerData">基础层数据（已收缩的第一层）</param>
        /// <param name="totalLayers">总层数</param>
        private void GenerateMultiLayerData(List<MarchingCube.MarchingCubeData.ModifyPointData> baseLayerData, int totalLayers)
        {
            if (baseLayerData == null || baseLayerData.Count == 0)
                return;

            // 从第二层开始生成
            for (int layerIndex = 1; layerIndex < totalLayers; layerIndex++)
            {
                float currentLayerHeight = layerIndex * height;

                foreach (var basePoint in baseLayerData)
                {
                    // 计算当前层的位置
                    Vector3 layerPos = ExpandPosition(basePoint.pos, currentLayerHeight);

                    MarchingCube.MarchingCubeData.ModifyPointData layerData = new MarchingCube.MarchingCubeData.ModifyPointData
                    {
                        xIndex = basePoint.xIndex,
                        yIndex = layerIndex,
                        zIndex = basePoint.zIndex,
                        pos = layerPos,
                        normal = basePoint.normal
                    };

                    modifyPointDatas.Add(layerData);
                }

                Debug.Log($"生成了第{layerIndex}层的{baseLayerData.Count}个点");
            }
        }

        /// <summary>
        /// 将位置扩展指定距离
        /// </summary>
        /// <param name="position">原始位置</param>
        /// <param name="expansionAmount">扩展距离</param>
        /// <returns>扩展后的位置</returns>
        private Vector3 ExpandPosition(Vector3 position, float expansionAmount)
        {
            if (position == Vector3.zero)
                return position;

            Vector3 radialDirection = position.normalized;
            return position + radialDirection * expansionAmount;
        }
        #endregion

        public void Load()
        {
            //如果还在建造模式，先退出
            buildingSystemOnSphere.SwitchEditMode(false);

            marchingCube.LoadTerrainData();
            GenerateObj();
        }

        public void Save()
        {
            marchingCube.SaveTerrainData();
        }

        #region 模块生成
        /// <summary>
        /// 生成测试模块
        /// </summary>
        public void GenerateObj()
        {
            CreateRoot();
            CreateModifiedRoot();
            ClearAllModules();
            ModifyAllModules();
        }

        public void ActivateObj()
        {
            //测试逻辑
            if (this.activeSurround)
            {
                int x = activeObjXIndex;
                int y = activeObjYIndex;
                int z = activeObjZIndex;
                Face currentFace = (Face)(faceIndex + 1);

                // 1. 激活核心点
                marchingCube.marchingCubeDatas[faceIndex].objPointArray[x, y, z].isActive = isActivate;

                // 2. 激活周围点（实点）- 解决逻辑连通
                List<Vector3> surroundPoints = UCalculate.CalculateSurroundPoint(new Vector3(x, y, z), currentFace, columnsPerFace);

                Debug.Log($"<color=green>=== 开始激活 [{faceIndex}, {x}, {y}] 的周围点 ===</color>"); // 方便在控制台区分每一组

                foreach (Vector3 p in surroundPoints)
                {
                    int nFaceIdx = (int)p.z - 1;
                    int nx = (int)p.x;
                    int ny = (int)p.y;

                    if (nFaceIdx >= 0 && nFaceIdx < 6)
                    {
                        // 打印详细信息：也就是它激活了 "第几个面" 的 "X, Y" 坐标
                        Debug.Log($"激活周围点 -> Face: {nFaceIdx}, Pos: ({nx}, {ny}, {z})");

                        // 确保不越界
                        marchingCube.marchingCubeDatas[nFaceIdx].objPointArray[nx, ny, z].isActive = isActivate;
                    }
                    else
                    {
                        Debug.LogWarning($"计算出的周围点面索引越界或无效: Face {nFaceIdx}");
                    }
                }

                // // 3. 激活桥梁点 (当前面的虚点 0 或 size+1)
                // // 这一步是为了让 Marching Cube 在当前面渲染时，网格能闭合到边缘
                // List<Vector3> bridgePoints = UCalculate.CalculateBridge(new Vector3(x, y, z), currentFace, marchingCube.marchingCubeDatas.ToArray(), columnsPerFace);
                // foreach (Vector3 b in bridgePoints)
                // {
                //     int bx = (int)b.x;
                //     int by = (int)b.y;
                //     int bz = (int)b.z;

                //     // 激活当前面的边缘虚点
                //     marchingCube.marchingCubeDatas[faceIndex].objPointArray[bx, by, bz].isActive = isActivate;
                // }
                //测试逻辑结束
            }
            SetOverlapPoint(faceIndex, activeObjXIndex, activeObjYIndex, activeObjZIndex);
        }

        public void SwitchEditMode(bool isEditing)
        {
            this.isEditing = isEditing;
            buildingSystemOnSphere.SwitchEditMode(isEditing);
        }

        //需要约定faceIndex=几
        public void OnClick(int xIndex, int yIndex, int zIndex, Face faceIndex, BuildingType type)
        {
            //共用逻辑
            marchingCube.marchingCubeDatas[(int)faceIndex].objPointArray[xIndex, yIndex, zIndex].isActive = true;
            //美术逻辑
            //计算出因为新建筑而生成的桥梁点
            List<Vector3> bridgePoint = UCalculate.CalculateBridge(new Vector3(xIndex, yIndex, zIndex), faceIndex, this.marchingCube.marchingCubeDatas.ToArray(), 3);
            //计分逻辑


        }


        /// <summary>
        /// 设置所有重合点的信息
        /// </summary>
        public void SetOverlapPoint(int faceIndex, int x, int y, int z, bool isDestroy = false)
        {
            Vector3 pos = marchingCube.marchingCubeDatas[faceIndex].objPointArray[x, y, 0].pos;
            foreach (var marchingCubeData in marchingCube.marchingCubeDatas)
            {
                foreach (var objPointData in marchingCubeData.objPointDatas)
                {
                    if (objPointData.pos == pos)
                    {
                        // 销毁模式
                        marchingCubeData.objPointArray[objPointData.xIndex, objPointData.zIndex, z].isActive = !isDestroy;
                    }
                }
            }
            // 同步更新
            marchingCube.marchingCubeDatas[faceIndex].objPointArray[x, y, z].isActive = !isDestroy;
        }

        // 兼容
        public void SetOverlapPoint(int faceIndex, int x, int y, int z)
        {
            SetOverlapPoint(faceIndex, x, y, z, false);
        }

        public void ClearAllModules()
        {
            if (marchingCube.moduleInstances != null)
            {
                foreach (GameObject module in marchingCube.moduleInstances)
                {
                    if (module != null)
                    {
                        DestroyImmediate(module);
                    }
                }
                marchingCube.moduleInstances.Clear();
            }

            if (marchingCube.marchingCubeDatas != null)
            {
                foreach (var cubeData in marchingCube.marchingCubeDatas)
                {
                    if (cubeData.faceModuleInstances != null)
                    {
                        foreach (GameObject module in cubeData.faceModuleInstances)
                        {
                            if (module != null)
                            {
                                DestroyImmediate(module);
                            }
                        }
                        cubeData.faceModuleInstances.Clear();
                    }
                }
            }

            modifyMesh3D.ClearAllModules();
        }

        public void ModifyAllModules()
        {
            foreach (var marchingCubeData in marchingCube.marchingCubeDatas)
            {
                ModifyModule(marchingCubeData);
            }
        }

        public void ModifyModule(MarchingCube.MarchingCubeData marchingCubeData)
        {
            List<Vector3> modifyPointPos = new();
            List<Vector3> cannotBeModified_Light_posList = new();
            marchingCube.UpdateModules(marchingCubeData);

            // 获取所有module
            int moduleCount = marchingCubeData.modulePointDatas.Count;
            Debug.Log($"总模块数量：{moduleCount}，当前面实例数量：{marchingCubeData.faceModuleInstances.Count}");

            for (int i = 0; i < moduleCount; i++)
            {
                MarchingCube.MarchingCubeData.ModulePointData modulePointData = marchingCubeData.modulePointDatas[i];

                if (modulePointData.moduleName == "00000000")
                    continue;

                GameObject targetModuleObj = null;
                GameObject parentObj = null;

                cannotBeModified_Light_posList.Clear();

                // 使用当前面的模块实例列表
                if (i < marchingCubeData.faceModuleInstances.Count)
                {
                    parentObj = marchingCubeData.faceModuleInstances[i];

                    if (parentObj != null)
                    {
                        //子对象作为变形模块
                        foreach (Transform child in parentObj.transform)
                        {
                            if (child.CompareTag("CanBeModified"))
                            {
                                targetModuleObj = child.gameObject;
                            }

                            if (child.CompareTag("CannotBeModified_Light"))
                            {
                                cannotBeModified_Light_posList.Add(child.localPosition);
                            }
                        }

                        GameObject newParentObj = new();
                        newParentObj.name = parentObj.name + "_modified";
                        newParentObj.transform.SetParent(modifiedRoot.transform);

                        if (targetModuleObj == null)
                        {
                            Debug.LogWarning($"模块索引[{modulePointData.xIndex},{modulePointData.yIndex},{modulePointData.zIndex}] 无对应实例，名称：{modulePointData.moduleName}");
                            continue;
                        }

                        Debug.Log($"模块索引{modulePointData.xIndex}{modulePointData.yIndex}{modulePointData.zIndex}，对应物件：{targetModuleObj.name}，位置：{targetModuleObj.transform.position}");

                        modifyPointPos = marchingCubeData.GetModifyPointsAroundModule(
                            modulePointData.xIndex,
                            modulePointData.yIndex,
                            modulePointData.zIndex
                        );

                        //foreach(var pos in modifyPointPos)
                        //{
                        //    Debug.Log(pos);
                        //}

                        Matrix4x4 worldMatrix = parentObj.transform.localToWorldMatrix * targetModuleObj.transform.localToWorldMatrix;
                        modifyMesh3D.GenerateModule(modifyPointPos, targetModuleObj, newParentObj, height, worldMatrix);

                        int lightIndex = 0;

                        //生成灯光obj
                        foreach (var pos in cannotBeModified_Light_posList)
                        {
                            Quaternion rotation = parentObj.transform.rotation;
                            Vector3 modifiedPos = rotation * pos;
                            Vector3 lightModifiedPos = modifyMesh3D.GetDeformedPoint(modifiedPos, modifyPointPos, height);

                            GameObject lightModified = Instantiate(light);
                            lightModified.name = "Light_" + lightIndex + "_modified";
                            lightModified.transform.position = lightModifiedPos;
                            lightModified.transform.SetParent(newParentObj.transform);
                            lightIndex++;
                        }
                    }
                }
            }
        }

        public void ModifyHintModule(MarchingCube.MarchingCubeData marchingCubeData)
        {

        }
        #endregion

        public void ShowDebug()
        {
            //meshGenerator.ShowDebug();
            //buildingSystemOnSphere.ModifyHintModule();
            buildingSystemOnSphere.ModifyAllHintModules();
        }

        public void SwitchGeometry()
        {
            marchingCube.isShowGeo = !marchingCube.isShowGeo;
        }

        private void OnDrawGizmos()
        {
            //Gizmos.color = Color.white;
            //foreach (Vector3 indexVec in meshGenerator.meshDataList[0].vertices)
            //{

            //    Gizmos.DrawSphere(indexVec, 0.25f);
            //}
        }
    }
}