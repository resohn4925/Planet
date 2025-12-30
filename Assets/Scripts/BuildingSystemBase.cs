using System.Collections;
using System.Collections.Generic;
using TowerStacker;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Experimental.GraphView.GraphView;
using static UnityEditor.PlayerSettings;
using static UnityEngine.Rendering.DebugUI.Table;

public class BuildingSystemBase : MonoBehaviour
{
    private float moduleHeight = 0f;

    public string modulePath;

    public GameObject root;

    public List<GameObject> moduleList;

    //临时变量，记录所有可放置点的索引和激活状态
    private MarchingCube.MarchingCubeData.ObjPointData[,,] objPointArrayCurrent;

    #region DataSetting

    public void SetPipelineData(float height)
    {
        moduleHeight = height;
    }
    #endregion

    #region 表现层
    /// <summary>
    /// 计算可放置的位置
    /// </summary>
    public void CalculateHint(MarchingCube marchingCube)
    {
        //根据obj激活情况激活hintobj
        //marchingCube.marchingCubeDatas[0].hintObjPointArray[0, 0, 0].isActive = true;
        //激活一层所有物体
        int xSize = marchingCube.marchingCubeDatas[0].hintObjPointArray.GetLength(0);
        int ySize = marchingCube.marchingCubeDatas[0].hintObjPointArray.GetLength(1);
        int zSize = marchingCube.marchingCubeDatas[0].hintObjPointArray.GetLength(2);

        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                marchingCube.marchingCubeDatas[0].hintObjPointArray[x, y, 0].isActive = true;
            }
        }
    }

    public void ResetHint(MarchingCube marchingCube)
    {
        //初始化hintobj
        foreach (var datas in marchingCube.marchingCubeDatas)
        {
            foreach (var data in datas.hintObjPointArray)
            {
                data.isActive = false;
            }
        }
        marchingCube.marchingCubeDatas[0].hintObjPointArray[0, 0, 0].isActive = true;
    }

    /// <summary>
    /// 根据击中的物体的索引生成基础obj
    /// </summary>
    public GameObject GenerateHittedObj(GameObject hittedObj)
    {
        Debug.Log($"射线击中{hittedObj.name}");
        return hittedObj;
    }

    public void UpdateAllHintMesh(string rootName, bool isVisible)
    {
        GameObject hintRoot = GameObject.Find(rootName);

        if (hintRoot == null)
        {
            return;
        }

        MeshRenderer[] meshRenderers = hintRoot.GetComponentsInChildren<MeshRenderer>(true);

        if (meshRenderers.Length == 0)
        {
            return;
        }

        foreach (MeshRenderer renderer in meshRenderers)
        {
            renderer.enabled = isVisible;
        }
    }

    public void UpdateHintMesh(GameObject hintObj, bool isVisible)
    {
        //获取hintobj的meshrenderer，设置为isvisible
        if (hintObj != null)
        {
            MeshRenderer meshRenderer = hintObj.GetComponent<MeshRenderer>();
            if (meshRenderer != null)
            {
                meshRenderer.enabled = isVisible;
            }
            else
            {
                Debug.LogWarning($"对象 {hintObj.name} 上没有MeshRenderer组件");
            }
        }
    }

    public void CreateModule(MarchingCube marchingCube)
    {
        marchingCube.marchingCubeDatas[0].objPointArray[1, 1, 0].isActive = true;
        marchingCube.UpdateModules(marchingCube.marchingCubeDatas[0]);
    }
    #endregion
}
