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

    private MarchingCube marchingCube = new();
    private MarchingCube marchingCubeHintObj;

    public GameObject root;

    #region DataSetting

    public void SetPipelineData(float height)
    {
        moduleHeight = height;
    }

    public void SetMarchingCubeData(MarchingCube marchingCube)
    {
        this.marchingCube = marchingCube;
    }

    public void InitMarchingCubeHintObj(MarchingCube marchingCube)
    {
        marchingCubeHintObj = new();

        marchingCubeHintObj.modulePath = marchingCube.modulePath;
        marchingCubeHintObj.rows = marchingCube.rows;
        marchingCubeHintObj.layers = marchingCube.layers;
        marchingCubeHintObj.columns = marchingCube.columns;
        marchingCubeHintObj.spacing = marchingCube.spacing;
        marchingCubeHintObj.moduleCollection = root;

        marchingCubeHintObj.InitAllMarchingCubeDatas(1);

        marchingCubeHintObj.marchingCubeDatas[0].objPointArray[1, 1, 0].isActive = true;
        Debug.Log(marchingCubeHintObj.marchingCubeDatas[0].objPointArray[0, 0, 0].isActive);
        marchingCubeHintObj.UpdateModules(marchingCubeHintObj.marchingCubeDatas[0]);
    }

    public void DataDebug()
    {

    }
    #endregion

    #region 表现层
    /// <summary>
    /// 计算可放置的位置
    /// </summary>
    public void CalculateHint()
    {
        int rows = marchingCube.rows;
        int columns = marchingCube.columns;
        int layers = marchingCube.layers;

        List<(MarchingCube.MarchingCubeData cubeData, MarchingCube.MarchingCubeData.ObjPointData objPoint)> activatablePoints = new();

        Vector3Int[] neighborOffsets = new Vector3Int[]
{
        new Vector3Int(1, 0, 0),   // x+1
        new Vector3Int(-1, 0, 0),  // x-1
        new Vector3Int(0, 1, 0),   // z+1
        new Vector3Int(0, -1, 0),  // z-1
        new Vector3Int(0, 0, 1),   // y+1
        new Vector3Int(0, 0, -1)   // y-1
};


    }

    /// <summary>
    /// 顶点索引是否越界
    /// </summary>
    /// <returns></returns>
    public bool IsPointOutOfRange()
    {
        return false;
    }



    /// <summary>
    /// 返回被射线击中的物体的索引
    /// </summary>
    public GameObject GetHittedObj(GameObject hittedObj)
    {
        Debug.Log($"射线击中{hittedObj.name}");
        return hittedObj;
    }

    public void ShowHint(bool isOnSphere, bool isVisible, int faceIndex, int xIndex, int zIndex, int yIndex)
    {
        if (marchingCube == null)
        {
            Debug.LogWarning("请先初始化");
            return;
        }
        marchingCube.marchingCubeDatas[faceIndex].objPointArray[xIndex, zIndex, yIndex].isActive = true;
        marchingCube.UpdateModules(marchingCube.marchingCubeData);
    }


    #endregion
}
