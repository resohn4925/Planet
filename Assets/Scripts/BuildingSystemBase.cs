using System;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSystemBase : MonoBehaviour
{
    private float moduleHeight = 0f;

    [HideInInspector]public List<GameObject> moduleList;

    //临时变量，记录所有可放置点的索引和激活状态
    private MarchingCube.MarchingCubeData.ObjPointData[,,] objPointArrayCurrent;

    #region DataSetting

    public void SetPipelineData(float height)
    {
        moduleHeight = height;
    }
    #endregion

    #region 数据层
    /// <summary>
    /// 激活顶点
    /// </summary>
    /// <param name="hintName"></param>
    /// <param name="marchingCube"></param>
    public void ActivateModule(string hintName, MarchingCube marchingCube)
    {
        try
        {
            // 解析名字
            string[] parts = hintName.Split('_');

            if (parts.Length < 5)
            {
                Debug.LogError($"Invalid hint name format: {hintName}");
                return;
            }

            // 解析参数
            CubeFace face = (CubeFace)System.Enum.Parse(typeof(CubeFace), parts[1], true);
            int xIndex = int.Parse(parts[2]);
            int zIndex = int.Parse(parts[3]);

            int yIndex = int.Parse(parts[4]);

            // 查找对应的 MarchingCubeData
            MarchingCube.MarchingCubeData targetData = null;
            foreach (var data in marchingCube.marchingCubeDatas)
            {
                if (data.cubeFace == face)
                {
                    targetData = data;
                    break;
                }
            }

            if (targetData == null)
            {
                Debug.LogError($"No MarchingCubeData found for face: {face}");
                return;
            }

            // 激活对应的点
            if (xIndex >= 0 && xIndex < targetData.objPointArray.GetLength(0) &&
                zIndex >= 0 && zIndex < targetData.objPointArray.GetLength(1) &&
                yIndex >= 0 && yIndex < targetData.objPointArray.GetLength(2))
            {
                targetData.objPointArray[xIndex, zIndex, yIndex].isActive = true;
                Debug.Log($"激活{face}上的点,索引为[{xIndex}, {zIndex}, {yIndex}]");
            }
            else
            {
                Debug.LogError($"点[{xIndex}, {zIndex}, {yIndex}]超出索引");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"激活{hintName}失败: {ex.Message}");
        }
    }
    #endregion

    #region 表现层
    /// <summary>
    /// 计算可放置的位置
    /// </summary>
    public void CalculateHint(MarchingCube marchingCube)
    {
        if (marchingCube == null || marchingCube.marchingCubeDatas == null || marchingCube.marchingCubeDatas.Count == 0)
        {
            Debug.LogError("MarchingCube数据为空，无法计算提示位置！");
            return;
        }

        var hintArray = marchingCube.marchingCubeDatas[0].hintObjPointArray;
        var objArray = marchingCube.marchingCubeDatas[0].objPointArray;

        // 校验数组是否为空
        if (hintArray == null || objArray == null)
        {
            Debug.LogError("hintObjPointArray或objPointArray为空！");
            return;
        }

        // 获取三维数组各维度的长度
        int xSize = hintArray.GetLength(0);
        int zSize = hintArray.GetLength(1);
        int ySize = hintArray.GetLength(2);

        // 遍历所有x、z、y维度的点，按规则激活hint
        // 规则:y=0时,所有hint激活
        // 规则:y>0时，仅当y-1层对应位置的物体点激活，当前hint才激活
        for (int x = 0; x < xSize; x++)
        {
            for (int z = 0; z < zSize; z++)
            {
                for (int y = 0; y < ySize; y++)
                {
                    if (y == 0)
                    {
                        hintArray[x, z, y].isActive = true;
                    }
                    else
                    {
                        // 防止越界
                        if (y - 1 >= 0 &&
                            x < objArray.GetLength(0) &&
                            z < objArray.GetLength(1) &&
                            y - 1 < objArray.GetLength(2))
                        {
                            hintArray[x, z, y].isActive = objArray[x, z, y - 1].isActive;
                        }
                        else
                        {
                            hintArray[x, z, y].isActive = false;
                            Debug.LogWarning($"[{x}, {z}, {y}] 下方层索引越界，无法激活hint！");
                        }
                    }
                }
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

    public void UpdateModule(MarchingCube marchingCube)
    {
        marchingCube.UpdateModules(marchingCube.marchingCubeDatas[0]);
    }
    #endregion
}
