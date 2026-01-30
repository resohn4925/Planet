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

    //编辑模式
    public BuildingMode currentBuildingMode = BuildingMode.Build;

    public GameObject hintObj;
    public GameObject modifiedHintRoot;

    private bool isEditing = false;
    private GameObject lastHitObj = null;
    private float _hintModifyHeight = 3.636364f;
    private Vector3 lastClickPosition;

    private RippleEffectURP rippleEffect;
    // 存储上一次的hint激活状态，用于增量更新
    private Dictionary<string, bool> _previousHintStates = new Dictionary<string, bool>();

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
        buildingSystemBase.CalculateHint(marchingCube, currentBuildingMode);
        marchingCube.UpdateHint(marchingCube);
        // 隐藏hint的mesh
        buildingSystemBase.UpdateAllHintMesh("HintRoot", false);
        ModifyAllHintModules();
    }

    public void UpdateHintIncremental()
    {
        buildingSystemBase.CalculateHint(marchingCube, currentBuildingMode);
        
        //计算changedhint索引
        List<string> newActiveHints = new List<string>();
        Dictionary<string, bool> currentHintStates = new Dictionary<string, bool>();
        
        foreach (var data in marchingCube.marchingCubeDatas)
        {
            var hintArray = data.hintObjPointArray;
            if (hintArray == null)
                continue;
            
            int xSize = hintArray.GetLength(0);
            int zSize = hintArray.GetLength(1);
            int ySize = hintArray.GetLength(2);
            
            for (int x = 0; x < xSize; x++)
            {
                for (int z = 0; z < zSize; z++)
                {
                    for (int y = 0; y < ySize; y++)
                    {
                        string hintKey = $"Hint_{data.cubeFace}_{x}_{z}_{y}";
                        bool isActive = hintArray[x, z, y].isActive;
                        currentHintStates[hintKey] = isActive;
                        
                        if (isActive && (!_previousHintStates.ContainsKey(hintKey) || !_previousHintStates[hintKey]))
                        {
                            newActiveHints.Add(hintKey);
                        }
                    }
                }
            }
        }
        
        if (newActiveHints.Count > 0)
        {
            foreach (string hintKey in newActiveHints)
            {
                Debug.Log($"changedhint索引为{hintKey}");
            }
        }
        
        _previousHintStates = currentHintStates;

        marchingCube.UpdateHint(marchingCube);
        // 隐藏hint的mesh
        buildingSystemBase.UpdateAllHintMesh("HintRoot", false);
        // 增量更新hint
        ModifyHintModulesIncremental(newActiveHints);
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
            GameObject currentHitObj = null;

        // 获取鼠标悬停的对象，优先选择modifiedhintmesh
        RaycastHit[] hits = Physics.RaycastAll(ray);

        // 按距离排序，近的在前
        Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));

        // 优先选择modifiedhintmesh（名称以"Hint_"开头且包含"_modified"的对象）
        foreach (RaycastHit hit in hits)
        {
            GameObject hitObj = hit.collider.gameObject;
            if (hitObj.name.StartsWith("Hint_") && hitObj.name.Contains("_modified"))
            {
                currentHitObj = hitObj;
                break;
            }
        }

        // 如果没有找到modifiedhintmesh，再尝试找到普通的hintmesh
        if (currentHitObj == null)
        {
            foreach (RaycastHit hit in hits)
                {
                    GameObject hitObj = hit.collider.gameObject;
                    if (hitObj.name.StartsWith("Hint_"))
                    {
                        currentHitObj = hitObj;
                        break;
                    }
                }
            }

        // 如果没有找到hintmesh，选择第一个命中的对象
        if (currentHitObj == null && hits.Length > 0)
        {
            currentHitObj = hits[0].collider.gameObject;
        }

        // 处理命中对象
        if (currentHitObj != null)
        {
            buildingSystemBase.GenerateHittedObj(currentHitObj);

            buildingSystemBase.UpdateAllHintMesh("ModifiedHintRoot", false);
            // 检查是否是原始 hint 物件，如果是则不显示
            if (!currentHitObj.name.StartsWith("Hint_"))
            {
                    buildingSystemBase.UpdateHintMesh(currentHitObj, true);
                }
                //根据名字计算currentobj的face,x,y,z索引
                //SetOverlapPoint逻辑计算该物件的overlap物件索引
                //激活overlap物件
                SetOverlapPoint(currentHitObj.name);

                if (e.type == EventType.MouseDown && e.button == 0)
                {
                    //获取currentHitObj的索引，根据索引激活基础obj
                    if (currentBuildingMode == BuildingMode.Build)
                    {
                        buildingSystemBase.ActivateModule(currentHitObj.name, marchingCube);
                    }
                    else if (currentBuildingMode == BuildingMode.Destroy)
                    {
                        buildingSystemBase.DeactivateModule(currentHitObj.name, marchingCube);
                    }

                //这里用全量更新会产生严重性能开销
                //CreateModule();
                IncrementalCreateModule();
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

    /// <summary>
    /// 全量更新
    /// </summary>
    private void CreateModule()
    {
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

        // 触发波纹效果
        TriggerRippleEffect();
    }

    /// <summary>
    /// 增量更新
    /// </summary>
    private void IncrementalCreateModule()
    {
        //pipeline中更新变形模块
        var pipelines = FindObjectsOfType<PlanetPipeline>();
        var targetPipeline = pipelines.FirstOrDefault(p => p.name == "PlanetPipeline");
        if (targetPipeline == null)
        {
            Debug.LogError("找不到planetpipeline,请先创建");
        }
        else { targetPipeline.GenerateObjIncremental(); }

        UpdateHintIncremental();

        // 触发波纹效果
        TriggerRippleEffect();
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
        // 隐藏modiefiedhint的mesh
        buildingSystemBase.UpdateAllHintMesh("ModifiedHintRoot", false);
    }

    /// <summary>
    /// 增量更新hint
    /// </summary>
    public void ModifyHintModulesIncremental(List<string> hintkey)
    {
        GameObject hintRoot = GameObject.Find("HintRoot");
        if (hintRoot == null)
        {
            Debug.LogError("未找到HintRoot节点！");
            return;
        }

        foreach (var hint in hintkey)
        {
            GameObject hintObj = GameObject.Find(hint);
            ModifyHintModule(hint, hintObj);

            GameObject hintRootModified = GameObject.Find(hint + "_modified");

            //隐藏增量更新的hint的mesh
            buildingSystemBase.UpdateHintMesh(hintRootModified, false);
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
                //Debug.LogError($"Invalid hint name format: {hintName}");
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

            //Debug.Log($"hintobj索引为{face},[{xIndex}, {zIndex}, {yIndex}]");
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
                    //Debug.Log($"{hintName}的重合点是{hintNameOverlap}");
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

    public void SwitchBuildingMode(BuildingMode mode)
    {
        currentBuildingMode = mode;

        if (isEditing)
        {
            UpdateHint();
        }
    }


    private void TriggerRippleEffect()
    {
        if (rippleEffect == null)
        {
            rippleEffect = FindObjectOfType<RippleEffectURP>();
            if (rippleEffect == null)
            {
                Debug.LogWarning("未找到RippleEffectURP组件，无法触发波纹效果");
                return;
            }
        }

        rippleEffect.ActivateRipple(lastClickPosition);
    }

    private Vector3 GetHitPoint(Ray ray, GameObject targetObj)
    {
        RaycastHit[] hits = Physics.RaycastAll(ray);

        foreach (RaycastHit hit in hits)
        {
            GameObject hitObj = hit.collider.gameObject;

            if (hitObj == targetObj)
            {
                return hit.point;
            }

            if (hitObj.name.StartsWith("Hint_") && hitObj.name.Contains("_modified"))
            {
                return hit.point;
            }
        }

        foreach (RaycastHit hit in hits)
        {
            GameObject hitObj = hit.collider.gameObject;

            if (hitObj.name.StartsWith("Hint_"))
            {
                return hit.point;
            }
        }

        if (targetObj != null)
        {
            return targetObj.transform.position;
        }

        return Vector3.zero;
    }
}
