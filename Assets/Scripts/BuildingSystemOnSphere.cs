using Planet;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class BuildingSystemOnSphere : MonoBehaviour
{
    public BuildingSystemBase buildingSystemBase;
    [HideInInspector] public MarchingCube marchingCube;
    public ModifyMesh3d modifyMesh3D;

    public GameObject hintObj;
    public GameObject modifiedHintRoot;

    private bool isEditing = false;
    private GameObject lastHitObj = null;
    private float _hintModifyHeight = 3.636364f;

    //测试用变量
    //public GameObject testHintObj;


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
        buildingSystemBase.UpdateAllHintMesh("ModifiedHintRoot", false);
    }

    private void StopEditing()
    {
        isEditing = false;

        foreach (SceneView sceneView in SceneView.sceneViews)
        {
            sceneView.Repaint();
        }

        marchingCube.ClearAllHintInstances();
        ClearAllModifiedHints();
    }

    public void UpdateHint()
    {
        buildingSystemBase.CalculateHint(marchingCube);
        marchingCube.UpdateHint(marchingCube);
        ModifyAllHintModules();
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

            buildingSystemBase.UpdateAllHintMesh("ModifiedHintRoot", false);
            buildingSystemBase.UpdateHintMesh(currentHitObj, true);
            //根据名字计算currentobj的face,x,y,z索引
            //SetOverlapPoint逻辑计算该物件的overlap物件索引
            //激活overlap物件
            SetOverlapPoint(currentHitObj.name);

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

        //pipeline中更新变形模块
        var pipelines = FindObjectsOfType<PlanetPipeline>();
        var targetPipeline = pipelines.FirstOrDefault(p => p.name == "PlanetPipeline");
        if (targetPipeline == null)
        {
            Debug.LogError("找不到planetpipeline,请先创建");
        }
        else { targetPipeline.GenerateObj(); }

        //表现层,展示所有可建造的地块
        UpdateHint();
        ClearAllModifiedHints();
        ModifyAllHintModules();

        buildingSystemBase.UpdateAllHintMesh("HintRoot", false);
        buildingSystemBase.UpdateAllHintMesh("ModifiedHintRoot", false);
    }

    private void OnMouseExitHitObj(GameObject exitedObj)
    {
        //Debug.Log($"鼠标移出了对象: {exitedObj.name}");
        //必须是"Hint_开头的物体"
        if (exitedObj != null && !string.IsNullOrEmpty(exitedObj.name) && exitedObj.name.StartsWith("Hint_"))
        {
            buildingSystemBase.UpdateHintMesh(exitedObj, false);
        }
    }

    /// <summary>
    /// 批量处理HintRoot下所有Hint物件的空间变换
    /// </summary>
    public void ModifyAllHintModules()
    {
        GameObject hintRoot = GameObject.Find("HintRoot");
        if (hintRoot == null)
        {
            Debug.LogError("未找到HintRoot节点！");
            return;
        }

        foreach (Transform childTrans in hintRoot.transform)
        {
            GameObject childObj = childTrans.gameObject;
            string objName = childObj.name;

            if (objName.StartsWith("Hint_"))
            {
                ModifyHintModule(objName, childObj);
            }
        }
    }

    public void ClearAllModifiedHints()
    {
        if (modifiedHintRoot == null)
        {
            Debug.LogError("未找到ModifiedHintRoot节点！");
            return;
        }

        //销毁所有子物件
        List<GameObject> childObjs = new List<GameObject>();
        foreach (Transform childTrans in modifiedHintRoot.transform)
        {
            childObjs.Add(childTrans.gameObject);
        }
        int destroyedCount = 0;
        foreach (GameObject childObj in childObjs)
        {
            if (childObj == null) continue;

            // 编辑模式用DestroyImmediate，运行时用Destroy
            if (Application.isEditor && !Application.isPlaying)
            {
                DestroyImmediate(childObj);
            }
            else
            {
                Destroy(childObj);
            }
            destroyedCount++;
        }

    }

    /// <summary>
    /// 把单个hintobj进行网格变形
    /// </summary>
    public void ModifyHintModule(string hintName, GameObject hintObj)
    {
        //获取hintobj索引
        //string hintName = testHintObj.name;
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

            //Debug.Log($"hintobj索引为{face},[{xIndex}, {zIndex}, {yIndex}]");
        }
        catch (Exception ex)
        {
            Debug.LogError($"激活{hintName}失败: {ex.Message}");
            return;
        }

        //此处读取pipeline中传入的高度
        float height = 3.636364f;
        List<Vector3> modifyPointPos = new();
        modifyPointPos = marchingCube.marchingCubeDatas[((int)face)].GetModifyModulePointsAroundModule(xIndex, yIndex, zIndex);

        Matrix4x4 worldMatrix = hintObj.transform.localToWorldMatrix;
        modifyMesh3D.GenerateModule(modifyPointPos, hintObj, modifiedHintRoot, height, worldMatrix);

        Debug.Log("激活");
    }

    /// <summary>
    /// 设置所有重合点的信息
    /// </summary>
    public void SetOverlapPoint(string hintName)
    {
        CubeFace face;
        int xIndex; int yIndex; int zIndex;

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
            xIndex = xIndexTemp; yIndex = yIndexTemp; zIndex = zIndexTemp;

            Debug.Log($"hintobj索引为{face},[{xIndex}, {zIndex}, {yIndex}]");
        }

        catch (Exception ex)
        {
            Debug.LogError($"激活{hintName}失败: {ex.Message}");
            return;
        }


        Vector3 pos = marchingCube.marchingCubeDatas[(int)face].objPointArray[xIndex, zIndex, 0].pos;
        foreach (var marchingCubeData in marchingCube.marchingCubeDatas)
        {
            foreach (var objPointData in marchingCubeData.objPointDatas)
            {
                if (objPointData.pos == pos)
                {
                    string hintNameOverlap = $"Hint_{marchingCubeData.cubeFace}_{objPointData.xIndex}_{objPointData.zIndex}_{yIndex}_modified";
                    Debug.Log($"{hintName}的重合点是{hintNameOverlap}");
                    GameObject currentHintObj = GameObject.Find(hintNameOverlap);
                    if (currentHintObj == null)
                    {
                        Debug.LogWarning($"{hintName}未找到重合点{hintNameOverlap}");
                    }
                    buildingSystemBase.UpdateHintMesh(currentHintObj, true);
                }
            }
        }

        //if(hintName == "Hint_Left_7_10_0_modified")
        //{
        //    GameObject overlapObj = GameObject.Find("Hint_Top_0_3_0_modified");
        //    buildingSystemBase.UpdateHintMesh(overlapObj, true);
        //}

    }
}
