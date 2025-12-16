using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Searcher.SearcherWindow.Alignment;
using static UnityEngine.Rendering.DebugUI.Table;

namespace Planet
{
    public class PlanetPipeline : MonoBehaviour
    {
        [Header("脚本引用")]
        public MeshGenerator meshGenerator;
        public MarchingCube marchingCube;
        public ModifyMesh3d modifyMesh3D;

        [Header("网格生成参数")]
        public int columnsPerFace;//每面行数
        public int meshSize;
        public float planetRadius;
        public int layers;
        public int height;

        [Header("Debug参数")]
        public bool showVerts;
        public bool showMeshs;

        public GameObject testObj;

        private List<Vector3> modifyPointPos = new();
        private List<MarchingCube.MarchingCubeData.ModifyPointData> modifyPointDatas = new();

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

            GenerateMesh();
            InitMarchingData();
            SetModifyPointData();
        }

        public void GenerateMesh()
        {
            meshGenerator.GenarateMesh();
        }

        public void InitMarchingData()
        {
            marchingCube.Init();
        }

        public void SetModifyPointData()
        {
            MeshData meshData = meshGenerator.meshDataList[2];
            int verNum = meshGenerator.meshNum + 1;

            modifyPointPos.Clear();
            modifyPointDatas.Clear();

            List<int> uniqueXIndices = new List<int>();
            List<int> uniqueYIndices = new List<int>();

            // 收集所有唯一索引
            foreach (Vector3 indexVec in meshData.modifyVertices)
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

            List<MarchingCube.MarchingCubeData.ModifyPointData> layer0Data = new List<MarchingCube.MarchingCubeData.ModifyPointData>();

            foreach (Vector3 indexVec in meshData.modifyVertices)
            {
                int originalX = (int)indexVec.x;
                int originalY = (int)indexVec.y;
                int vertexIndex = originalY * verNum + originalX;

                if (vertexIndex >= 0 && vertexIndex < meshData.vertices.Count)
                {
                    Vector3 actualPos = meshData.vertices[vertexIndex];

                    // 第一层向内收缩,距离为height/2+地表偏移
                    Vector3 contractedPos = ContractPosition(actualPos, height / 2f + 0.5f);
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

            // 其他层数据
            int totalLayers = layers + 1;
            GenerateMultiLayerData(layer0Data, totalLayers);

            marchingCube.marchingCubeData.SetModifyPointData(modifyPointDatas);
        }

        /// <summary>
        /// 将位置向内收缩指定距离
        /// </summary>
        /// <param name="position">原始位置</param>
        /// <param name="contractionAmount">收缩距离</param>
        /// <returns>收缩后的位置</returns>
        private Vector3 ContractPosition(Vector3 position, float contractionAmount)
        {
            if (position == Vector3.zero)
                return position;

            Vector3 radialDirection = position.normalized;
            // 向球心方向收缩（减去法线方向的距离）
            return position - radialDirection * contractionAmount;
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
        /// 将位置向外扩展指定距离
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

        /// <summary>
        /// 计算模块高度
        /// </summary>
        /// <returns></returns>
        private float CalculateOptimalLayerHeight()
        {
            float estimatedSpacing = (2f * Mathf.PI * planetRadius) / (columnsPerFace * 4);

            if (meshGenerator.meshDataList.Count > 0)
            {
                MeshData data = meshGenerator.meshDataList[0];
                if (data.vertices.Count > 10)
                {
                    float sumDist = 0;
                    int count = 0;
                    for (int i = 0; i < data.vertices.Count - 1; i += 10)
                    {
                        for (int j = i + 1; j < Mathf.Min(i + 5, data.vertices.Count); j++)
                        {
                            sumDist += Vector3.Distance(data.vertices[i], data.vertices[j]);
                            count++;
                        }
                    }
                    return sumDist / Mathf.Max(1, count);
                }
            }

            return estimatedSpacing;
        }
        #endregion

        #region 模块生成
        /// <summary>
        /// 生成测试模块
        /// </summary>
        public void GenerateObj()
        {
            ModifyModule();
        }

        public void ModifyModule()
        {
            List<Vector3> modifyPointPos = new();

            //生成obj进行原型测试
            //marchingCube.marchingCubeData.objPointArray[0, 0, 0].isActive = true;
            //marchingCube.marchingCubeData.objPointArray[1, 1, 0].isActive = true;
            //marchingCube.marchingCubeData.objPointArray[1, 2, 0].isActive = true;
            //marchingCube.marchingCubeData.objPointArray[2, 2, 0].isActive = true;
            marchingCube.UpdateModules();

            //获取所有module
            int moduleCount = marchingCube.marchingCubeData.modulePointDatas.Count;
            Debug.Log($"总模块数量：{moduleCount}，已生成实例数量：{marchingCube.moduleInstances.Count}");

            for (int i = 0; i < moduleCount; i++)
            {
                MarchingCube.MarchingCubeData.ModulePointData modulePointData = marchingCube.marchingCubeData.modulePointDatas[i];

                if (modulePointData.moduleName == "00000000")
                    continue;

                GameObject targetModuleObj = null;
                if (i < marchingCube.moduleInstances.Count)
                {
                    targetModuleObj = marchingCube.moduleInstances[i];
                }

                if (targetModuleObj == null)
                {
                    Debug.LogWarning($"模块索引[{modulePointData.xIndex},{modulePointData.yIndex},{modulePointData.zIndex}] 无对应实例，名称：{modulePointData.moduleName}");
                    continue;
                }

                Debug.Log($"模块索引{modulePointData.xIndex}{modulePointData.yIndex}{modulePointData.zIndex}，对应物件：{targetModuleObj.name}，位置：{targetModuleObj.transform.position}");

                modifyPointPos = marchingCube.marchingCubeData.GetModifyPointsAroundModule(
                    modulePointData.xIndex,
                    modulePointData.yIndex,
                    modulePointData.zIndex
                );

                modifyMesh3D.GetGenerateModule(modifyPointPos, targetModuleObj, height);
            }
        }
        #endregion

        public void ShowDebug()
        {
            meshGenerator.ShowDebug();
        }

        public void ShowGeometry()
        {
            marchingCube.isShowGeo = true;
        }
    }
}