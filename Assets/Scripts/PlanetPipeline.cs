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

        [Header("Debug参数")]
        public bool showVerts;
        public bool showMeshs;

        public GameObject TestMesh;

        private List<Vector3> modifyPointPos = new();
        private List<MarchingCube.MarchingCubeData.ModifyPointData> modifyPointDatas = new();

        #region 初始化
        public void Init()
        {
            //配置参数更新
            meshGenerator.meshNum = columnsPerFace * 2;
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
            MeshData meshData = meshGenerator.meshDataList[0];
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

            // 第一层数据
            List<MarchingCube.MarchingCubeData.ModifyPointData> layer0Data = new List<MarchingCube.MarchingCubeData.ModifyPointData>();

            foreach (Vector3 indexVec in meshData.modifyVertices)
            {
                int originalX = (int)indexVec.x;
                int originalY = (int)indexVec.y;
                int vertexIndex = originalY * verNum + originalX;

                if (vertexIndex >= 0 && vertexIndex < meshData.vertices.Count)
                {
                    Vector3 actualPos = meshData.vertices[vertexIndex];
                    modifyPointPos.Add(actualPos);

                    int mappedX = xIndexMap[originalX];
                    int mappedY = yIndexMap[originalY];

                    MarchingCube.MarchingCubeData.ModifyPointData data = new MarchingCube.MarchingCubeData.ModifyPointData
                    {
                        xIndex = mappedX,
                        yIndex = 0,
                        zIndex = mappedY,
                        pos = actualPos,
                        normal = Vector3.up
                    };

                    layer0Data.Add(data);
                    modifyPointDatas.Add(data);
                }
            }

            // 其他层数据
            int totalLayers = layers; // 使用PlanetPipeline中的layers参数
            GenerateMultiLayerData(layer0Data, totalLayers);

            marchingCube.marchingCubeData.SetModifyPointData(modifyPointDatas);
        }

        /// <summary>
        /// 生成每层的modifypointdata
        /// </summary>
        /// <param name="baseLayerData"></param>
        /// <param name="totalLayers"></param>
        private void GenerateMultiLayerData(List<MarchingCube.MarchingCubeData.ModifyPointData> baseLayerData, int totalLayers)
        {
            if (baseLayerData == null || baseLayerData.Count == 0)
                return;

            float layerHeightIncrement = CalculateOptimalLayerHeight();

            // 从第二层开始生成
            for (int layerIndex = 1; layerIndex < totalLayers; layerIndex++)
            {
                float currentLayerHeight = layerIndex * layerHeightIncrement;

                foreach (var basePoint in baseLayerData)
                {
                    // 计算当前层的位置
                    Vector3 layerPos = UpLayerPos(basePoint.pos, new Vector3(0, currentLayerHeight, 0));

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

                Debug.Log($"生成了第{layerIndex + 1}层的{baseLayerData.Count}个点");
            }
        }

        /// <summary>
        /// 计算上层位置
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        private Vector3 UpLayerPos(Vector3 pos, Vector3 height)
        {
            if (pos == Vector3.zero)
                return height;

            Vector3 radialDirection = pos.normalized;
            float distance = height.y; 
            return pos + radialDirection * distance;
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
            marchingCube.marchingCubeData.objPointArray[0, 0, 0].isActive = true;
            marchingCube.UpdateModules();

            ModifyModule();
        }

        public void ModifyModule()
        {
            List<Vector3> modifyPointPos = new();
            modifyPointPos = marchingCube.marchingCubeData.PrintModifyPointsAroundModule(1, 1, 2);
            //输出单一模块的四个底部点
            foreach (Vector3 pos in modifyPointPos)
            {
                //Debug.Log(pos);
            }

            MeshFilter meshFilter = TestMesh.GetComponent<MeshFilter>();
            Mesh deformedMesh = modifyMesh3D.DeformMeshByFourPoints(modifyPointPos, meshFilter.sharedMesh);
            meshFilter.mesh = deformedMesh;
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