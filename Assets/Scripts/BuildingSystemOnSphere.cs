using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BuildingSystemOnSphere : MonoBehaviour
{
    public BuildingSystemBase buildingSystemBase;
    [HideInInspector] public MarchingCube marchingCube;
    public ModifyMesh3d modifyMesh3D;

    public GameObject hintObj;

    private bool isEditing = false;
    private GameObject lastHitObj = null;

    //测试用变量
    public GameObject testHintObj;
    public GameObject newParentObj;

    public void Init(MarchingCube marchingCube)
    {
        //读取marchingCube
        this.marchingCube = marchingCube;
    }

    /// <summary>
    /// 是否进入编辑模式
    /// </summary>
    public void SwitchEditMode(bool isEditing)
    {
        this.isEditing = isEditing;

        //更新hintmesh
        if (lastHitObj != null)
        {
            buildingSystemBase?.UpdateHintMesh(lastHitObj, false);
            lastHitObj = null;
        }

        //注册与销毁OnSceneGUI
        if(isEditing)
        {
            StartEditing();
            SceneView.duringSceneGui += OnSceneGUI;
        }
        else
        {
            StopEditing();
            SceneView.duringSceneGui -= OnSceneGUI;
        }
    }

    private void StartEditing()
    {
        foreach (SceneView sceneView in SceneView.sceneViews)
        {
            sceneView.Repaint();
        }

        //表现层,展示所有可建造的地块
        UpdateHint();
        buildingSystemBase.UpdateAllHintMesh("HintRoot", false);
    }

    private void StopEditing()
    {
        isEditing = false;

        foreach (SceneView sceneView in SceneView.sceneViews)
        {
            sceneView.Repaint();
        }

        marchingCube.ClearAllHintInstances();
    }

    public void UpdateHint()
    {
        buildingSystemBase.CalculateHint(marchingCube);
        marchingCube.UpdateHint(marchingCube.marchingCubeDatas[0]);
    }

    private void OnSceneGUI(SceneView sceneView)
    {
        if (hintObj == null)
            return;

        Event e = Event.current;

        bool needTakeControl = false;

        if (e.type == EventType.Layout || e.type == EventType.Repaint)
        {
            HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
        }
        if (e.alt)
        {
            HandleUtility.AddDefaultControl(0);
            return;
        }

        Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
        RaycastHit hit;
        GameObject currentHitObj = null;

        // 获取鼠标悬停的module所在obj索引
        if (Physics.Raycast(ray, out hit))
        {
            currentHitObj = hit.collider.gameObject;
            buildingSystemBase.GenerateHittedObj(currentHitObj);
            buildingSystemBase.UpdateHintMesh(currentHitObj, true);

            if (e.type == EventType.MouseDown && e.button == 0)
            {
                //获取currentHitObj的索引，根据索引激活基础obj
                buildingSystemBase.ActivateModule(currentHitObj.name, marchingCube);

                CreateModule();
                e.Use();
            }
        }

        // 检测鼠标移出事件
        if (lastHitObj != null && (currentHitObj == null || currentHitObj != lastHitObj))
        {
            OnMouseExitHitObj(lastHitObj);
        }

        if (e.type == EventType.Layout || e.type == EventType.Repaint)
        {
            if (needTakeControl)
            {
                HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
            }
            else
            {
                HandleUtility.AddDefaultControl(0);
            }
        }

        lastHitObj = currentHitObj;
    }

    private void CreateModule()
    {
        buildingSystemBase.UpdateModule(marchingCube);
        UpdateHint();
        buildingSystemBase.UpdateAllHintMesh("HintRoot", false);
    }

    private void OnMouseExitHitObj(GameObject exitedObj)
    {
        Debug.Log($"鼠标移出了对象: {exitedObj.name}");

        buildingSystemBase.UpdateHintMesh(exitedObj, false);
    }

    /// <summary>
    /// 把hintroot下的所有hintobj进行网格变形
    /// </summary>
    private void ModifyAllHintModules()
    {

    }

    /// <summary>
    /// 把单个hintobj进行网格变形
    /// </summary>
    public void ModifyHintModule()
    {
        //获取hintobj索引
        string hintName = testHintObj.name;
        CubeFace face;
        int xIndex;int yIndex;int zIndex;

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
            CubeFace faceTemp = (CubeFace)System.Enum.Parse(typeof(CubeFace), parts[1], true);
            int xIndexTemp = int.Parse(parts[2]);
            int zIndexTemp = int.Parse(parts[3]);

            int yIndexTemp = int.Parse(parts[4]);

            // 查找对应的 MarchingCubeData
            MarchingCube.MarchingCubeData targetData = null;
            foreach (var data in marchingCube.marchingCubeDatas)
            {
                if (data.cubeFace == faceTemp)
                {
                    targetData = data;
                    break;
                }
            }

            if (targetData == null)
            {
                Debug.LogError($"No MarchingCubeData found for face: {faceTemp}");
                return;
            }
            face = faceTemp;
            xIndex = xIndexTemp;yIndex = yIndexTemp;zIndex = zIndexTemp;

            Debug.Log($"hintobj索引为{face},[{xIndex}, {zIndex}, {yIndex}]");
        }
        catch (Exception ex)
        {
            Debug.LogError($"激活{hintName}失败: {ex.Message}");
            return;
        }

        float height = 3.6f;
        List<Vector3> modifyPointPos = new();
        modifyPointPos.Add(new Vector3(28.76f, -4.36f, 0));
        modifyPointPos.Add(new Vector3(29.09f, 0, 0));
        modifyPointPos.Add(new Vector3(28.76f, 0, 4.36f));
        modifyPointPos.Add(new Vector3(28.44f, -4.32f, 4.32f));

        Matrix4x4 worldMatrix = testHintObj.transform.localToWorldMatrix;
        modifyMesh3D.GenerateModule(modifyPointPos, testHintObj, newParentObj, height, worldMatrix);
    }
}
