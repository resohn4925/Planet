using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

namespace Planet
{
    public class PlanetPipeline : MonoBehaviour
    {
        [Header("脚本引用")]
        public MeshGenerator meshGenerator;

        [Header("网格生成参数")]
        public int meshNum;
        public int meshSize;
        public float planetRadius;

        [Header("Debug参数")]
        public bool showVerts;
        public bool showMeshs;

        public void GenerateMesh()
        {
            meshGenerator.GenarateMesh();
        }

        public void Init()
        {
            //配置参数更新
            meshGenerator.meshNum = meshNum;
            meshGenerator.meshSize = meshSize;
            meshGenerator.sphereRadius = planetRadius;
            meshGenerator.showVerts = showVerts;
            meshGenerator.showMeshs = showMeshs;
        }


    }
}

