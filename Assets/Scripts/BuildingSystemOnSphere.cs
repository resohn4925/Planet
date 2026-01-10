using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BuildingSystemOnSphere : MonoBehaviour
{
    public BuildingSystemBase buildingSystemBase;
    [HideInInspector] public MarchingCube marchingCube;

    public GameObject hintObj;

    private bool isEditing = false;
    private GameObject lastHitObj = null;

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
}
