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

    #region DataSetting
    public void SetPara(float height)
    {
        moduleHeight = height;
    }

    public void SetMarchingCube(MarchingCube marchingCube)
    {
        this.marchingCube = marchingCube;
    }
    #endregion

    #region 表现层
    public void ShowHint(bool isOnSphere)
    {
        if(marchingCube == null)
        {
            Debug.LogWarning("请先初始化");
            return;
        }

        //foreach (var marchingCubeData in marchingCube.marchingCubeDatas)
        //{
        //    foreach (var objPointData in marchingCubeData.objPointDatas)
        //    {
        //        if (objPointData.pos == pos)
        //        {
        //            marchingCubeData.objPointArray[objPointData.xIndex, objPointData.zIndex, activeObjZIndex].isActive = isActivate;
        //        }
        //    }
        //}

        //marchingCube.marchingCubeData.objPointArray[1, 1, 0].isActive = true;
        //marchingCube.UpdateModules(marchingCube.marchingCubeData);
    }
    #endregion


}
