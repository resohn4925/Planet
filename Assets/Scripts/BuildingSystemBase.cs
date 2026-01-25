using Planet;
using System;
using System.Collections.Generic;
using System.Linq;
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
                //targetData.objPointArray[xIndex, zIndex, yIndex].isActive = true;
                var pipelines = FindObjectsOfType<PlanetPipeline>();
                var targetPipeline = pipelines.FirstOrDefault(p => p.name == "PlanetPipeline");
                if (targetPipeline == null)
                {
                    Debug.LogError("找不到planetpipeline,请先创建");
                }
                else { targetPipeline.SetOverlapPoint((int)face, xIndex, zIndex, yIndex); }
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
    // 修改方法签名：新增模式参数
    public void CalculateHint(MarchingCube marchingCube, BuildingMode currentMode)
    {
        if (marchingCube == null || marchingCube.marchingCubeDatas == null || marchingCube.marchingCubeDatas.Count == 0)
        {
            Debug.LogError("MarchingCube数据为空，无法计算提示位置！");
            return;
        }

        foreach (var data in marchingCube.marchingCubeDatas)
        {
            var hintArray = data.hintObjPointArray;
            var objArray = data.objPointArray;

            if (hintArray == null || objArray == null)
            {
                Debug.LogError($"CubeFace {data.cubeFace} 的hint/obj数组为空！");
                continue;
            }

            int xSize = hintArray.GetLength(0);
            int zSize = hintArray.GetLength(1);
            int ySize = hintArray.GetLength(2);

            for (int x = 0; x < xSize; x++)
            {
                for (int z = 0; z < zSize; z++)
                {
                    for (int y = 0; y < ySize; y++)
                    {
                        if (currentMode == BuildingMode.Build)
                        {
                            // 建造模式规则：y=0激活，y>0继承下层状态
                            hintArray[x, z, y].isActive = y == 0
                                ? true
                                : (y - 1 >= 0 && x < objArray.GetLength(0) && z < objArray.GetLength(1) && y - 1 < objArray.GetLength(2))
                                    ? objArray[x, z, y - 1].isActive
                                    : false;
                        }
                        else if (currentMode == BuildingMode.Destroy)
                        {
                            // 销毁模式规则：仅已激活的地块显示Hint
                            hintArray[x, z, y].isActive = (x < objArray.GetLength(0) && z < objArray.GetLength(1) && y < objArray.GetLength(2))
                                ? objArray[x, z, y].isActive
                                : false;
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
    }

    /// <summary>
    /// 根据击中的物体的索引生成基础obj
    /// </summary>
    public GameObject GenerateHittedObj(GameObject hittedObj)
    {
        //Debug.Log($"射线击中{hittedObj.name}");
        return hittedObj;
    }

    public void UpdateAllHintMesh(string rootName, bool isVisible)
    {
        GameObject hintRoot = GameObject.Find(rootName);

        if (hintRoot == null)
        {
            return;
        }

        // 隐藏或显示所有子物件的渲染器
        MeshRenderer[] meshRenderers = hintRoot.GetComponentsInChildren<MeshRenderer>(true);
        if (meshRenderers.Length > 0)
        {
            foreach (MeshRenderer renderer in meshRenderers)
            {
                renderer.enabled = isVisible;
            }
        }

        // 隐藏所有子物件的碰撞器（针对 hintroot）
        if (rootName == "HintRoot")
        {
            // 处理 BoxCollider（hint 物件使用的碰撞器）
            BoxCollider[] boxColliders = hintRoot.GetComponentsInChildren<BoxCollider>(true);
            if (boxColliders.Length > 0)
            {
                foreach (BoxCollider collider in boxColliders)
                {
                    collider.enabled = false;
                }
            }
            
            // 同时处理 MeshCollider（以防万一）
            MeshCollider[] meshColliders = hintRoot.GetComponentsInChildren<MeshCollider>(true);
            if (meshColliders.Length > 0)
            {
                foreach (MeshCollider collider in meshColliders)
                {
                    collider.enabled = false;
                }
            }
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

    /// <summary>
    /// 取消激活顶点（销毁模式核心逻辑）
    /// </summary>
    public void DeactivateModule(string hintName, MarchingCube marchingCube)
    {
        try
        {
            string[] parts = hintName.Split('_');
            if (parts.Length < 5)
            {
                Debug.LogError($"Hint名称格式错误: {hintName}");
                return;
            }

            // 解析Hint名称（和建造模式逻辑一致）
            CubeFace face = (CubeFace)System.Enum.Parse(typeof(CubeFace), parts[1], true);
            int xIndex = int.Parse(parts[2]);
            int zIndex = int.Parse(parts[3]);
            int yIndex = int.Parse(parts[4]);

            // 查找对应面的MarchingCubeData
            var targetData = marchingCube.marchingCubeDatas.FirstOrDefault(d => d.cubeFace == face);
            if (targetData == null)
            {
                Debug.LogError($"未找到面{face}的MarchingCubeData");
                return;
            }

            // 校验索引并取消激活（同步重合点）
            if (xIndex >= 0 && xIndex < targetData.objPointArray.GetLength(0) &&
                zIndex >= 0 && zIndex < targetData.objPointArray.GetLength(1) &&
                yIndex >= 0 && yIndex < targetData.objPointArray.GetLength(2))
            {
                var targetPipeline = FindObjectsOfType<PlanetPipeline>().FirstOrDefault(p => p.name == "PlanetPipeline");
                if (targetPipeline == null)
                {
                    Debug.LogError("找不到PlanetPipeline！");
                    return;
                }
                // 调用SetOverlapPoint并标记为销毁（取消激活）
                targetPipeline.SetOverlapPoint((int)face, xIndex, zIndex, yIndex, isDestroy: true);
                Debug.Log($"销毁{face}上的点: [{xIndex}, {zIndex}, {yIndex}]");
            }
            else
            {
                Debug.LogError($"点[{xIndex}, {zIndex}, {yIndex}]索引越界");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"销毁{hintName}失败: {ex.Message}");
        }
    }

    public void UpdateModule(MarchingCube marchingCube)
    {
        marchingCube.UpdateModules(marchingCube.marchingCubeDatas[0]);
    }
    #endregion
}

public enum BuildingMode
{
    Build,    // 建造模式
    Destroy   // 销毁模式
}