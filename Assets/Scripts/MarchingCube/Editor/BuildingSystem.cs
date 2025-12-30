using UnityEditor;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;
using static MarchingCube;

public class BuildingSystem : EditorWindow
{
    [SerializeField]
    private GameObject hintObj;

    [SerializeField]
    private GameObject hintObjSlope;

    [SerializeField]
    private GameObject mesh;

    [SerializeField]
    private int currentLayers;

    [SerializeField]
    private string modulePath;

    [SerializeField]
    private GameObject rootObj;

    [SerializeField]
    private BuildingSystemBase buildingSystemBase;

    [SerializeField]
    private MarchingCube marchingCube;

    [SerializeField]
    private float spacing;

    [SerializeField]
    private int rows;

    [SerializeField]
    private int columns;

    [SerializeField]
    private int layers;

    [SerializeField]
    private bool isEditing = false;

    private EditMode editMode;

    //斜坡朝向
    private string selectedOption;

    private GameObject lastHitObj = null;

    [MenuItem("Tools/Building System")]
    public static void OpenWindow()
    {
        var window = GetWindow<BuildingSystem>("Building System");
        window.minSize = new Vector2(300, 200);
        window.Show();
    }

    private void OnGUI()
    {
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("资源配置", EditorStyles.boldLabel);
        hintObj = (GameObject)EditorGUILayout.ObjectField("HintObj", hintObj, typeof(GameObject), false);

        hintObjSlope = (GameObject)EditorGUILayout.ObjectField("HintObjSlope", hintObjSlope, typeof(GameObject), false);

        mesh = (GameObject)EditorGUILayout.ObjectField("Mesh", mesh, typeof(GameObject), false);

        buildingSystemBase = (BuildingSystemBase)EditorGUILayout.ObjectField("BuildingSystemBase", buildingSystemBase, typeof(BuildingSystemBase), true);

        marchingCube = (MarchingCube)EditorGUILayout.ObjectField("MarchingCube", marchingCube, typeof(MarchingCube), true);

        rootObj = (GameObject)EditorGUILayout.ObjectField("根节点", rootObj, typeof(GameObject), false);

        EditorGUI.BeginChangeCheck();
        modulePath = EditorGUILayout.TextField("Module资源路径", modulePath);
        if (EditorGUI.EndChangeCheck())
        {
            SetData();
        }

        if (GUILayout.Button("一键配置", GUILayout.Height(30)))
        {
            DefaultSetting();
        }

        if (GUILayout.Button("重置Module资源路径", GUILayout.Height(30)))
        {
            ResetModule();
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("参数配置", EditorStyles.boldLabel);
        spacing = EditorGUILayout.FloatField("模块尺寸", spacing);
        rows = EditorGUILayout.IntField("X行数量", rows);
        layers = EditorGUILayout.IntField("Y行数量", layers);
        columns = EditorGUILayout.IntField("Z行数量", columns);

        if (GUILayout.Button("初始化(将清除未保存模型)", GUILayout.Height(30)))
        {
            Init();
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("读存数据", EditorStyles.boldLabel);
        if (GUILayout.Button("加载地形", GUILayout.Height(30)))
        {
            marchingCube.Clear();
            marchingCube.LoadTerrainData();
        }

        if (GUILayout.Button("保存地形", GUILayout.Height(30)))
        {
            marchingCube.SaveTerrainData();
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("编辑Module", EditorStyles.boldLabel);
        currentLayers = EditorGUILayout.IntField("当前层数", currentLayers);
        if (!isEditing)
        {
            if (GUILayout.Button("编辑Module", GUILayout.Height(30)))
            {
                editMode = EditMode.Terrain;
                StartEditing();
            }
        }
        else
        {
            if (GUILayout.Button("结束", GUILayout.Height(30)))
            {
                StopEditing();
            }
        }

        string[] options = { "XPositive", "XNegative", "ZPositive", "ZNegative" };
        int currentIndex = System.Array.IndexOf(options, selectedOption);
        if (currentIndex < 0) currentIndex = 0;

        selectedOption = options[EditorGUILayout.Popup("斜坡方向", currentIndex, options)];
        if (!isEditing)
        {
            if (GUILayout.Button("编辑斜坡", GUILayout.Height(30)))
            {
                editMode = EditMode.Slope;
                StartEditing();
            }
        }
        else
        {
            if (GUILayout.Button("结束", GUILayout.Height(30)))
            {
                StopEditing();
            }
        }

        if (!isEditing)
        {
            if (GUILayout.Button("编辑悬崖包边", GUILayout.Height(30)))
            {
                editMode = EditMode.Cliff;
                StartEditing();
            }
        }
        else
        {
            if (GUILayout.Button("结束", GUILayout.Height(30)))
            {
                StopEditing();
            }
        }

        EditorGUILayout.Space();
        EditorGUILayout.HelpBox("Left Click: Place Object\nRight Click: Exit Placement Mode", MessageType.Info);
    }

    private void OnEnable()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
    }

    private void OnSceneGUI(SceneView sceneView)
    {
        if (!isEditing || hintObj == null)
            return;

        if (focusedWindow != this)
        {
            Focus();
        }

        Event e = Event.current;

        if (e.type == EventType.Layout || e.type == EventType.Repaint)
        {
            HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
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


                CreateModule();
                e.Use();
            }
        }

        // 检测鼠标移出事件
        if (lastHitObj != null && (currentHitObj == null || currentHitObj != lastHitObj))
        {
            OnMouseExitHitObj(lastHitObj);
        }

        lastHitObj = currentHitObj;
    }

    private void OnMouseExitHitObj(GameObject exitedObj)
    {
        Debug.Log($"鼠标移出了对象: {exitedObj.name}");

        buildingSystemBase.UpdateHintMesh(exitedObj, false);
    }

    #region 初始化
    private void Init()
    {
        if (marchingCube != null && marchingCube.marchingCubeDatas == null)
        {
            DefaultSetting();
            rootObj = GameObject.Find("Root");
            if (rootObj != null)
            {
                DestroyImmediate(rootObj);
                rootObj = new GameObject("Root");
                rootObj.transform.position = Vector3.zero;
            }
        }

        SetData();
        marchingCube.InitAllMarchingCubeDatas(1);
        marchingCube.moduleCollection = rootObj;

        DataDebug();
    }

    public void DataDebug()
    {
        //marchingCube.marchingCubeDatas[0].objPointArray[0, 0, 0].isActive = true;
        //marchingCube.UpdateModules(marchingCube.marchingCubeDatas[0]);

        //marchingCube.ClearFaceHintInstances(marchingCube.marchingCubeDatas[0]);
    }


    private void SetData()
    {
        if (marchingCube == null) return;
        marchingCube.modulePath = modulePath;
        marchingCube.rows = rows;
        marchingCube.layers = layers;
        marchingCube.columns = columns;
        marchingCube.spacing = spacing;
        marchingCube.moduleCollection = rootObj;
    }
    #endregion

    #region 配置
    private void DefaultSetting()
    {
        string hintPath = "Assets/Resources/Prefabs/Template/HintObj.prefab";
        hintObj = AssetDatabase.LoadAssetAtPath<GameObject>(hintPath);
        if (hintObj == null)
        {
            Debug.LogWarning($"未找到HintObj路径: {hintPath}");
        }

        string hintSlopePath = "Assets/Resources/Prefabs/Template/HintObjSlope.prefab";
        hintObjSlope = AssetDatabase.LoadAssetAtPath<GameObject>(hintSlopePath);
        if (hintObjSlope == null)
        {
            Debug.LogWarning($"未找到HintObjSlope路径: {hintSlopePath}");
        }

        string meshPath = "Assets/Resources/Prefabs/Template/Mesh.prefab";
        mesh = AssetDatabase.LoadAssetAtPath<GameObject>(meshPath);
        if (mesh == null)
        {
            Debug.LogWarning($"未找到Mesh路径: {meshPath}");
        }

        rootObj = GameObject.Find("Root");
        if (rootObj == null)
        {
            Debug.Log("null");
            rootObj = new GameObject("Root");
            rootObj.transform.position = Vector3.zero;

            EditorUtility.SetDirty(this);
        }

        FindOrCreateMarchingCube();
        FindOrCreateBuildingSystemBase();

        ReadData();
        currentLayers = 1;

        marchingCube.moduleCollection = rootObj;

        Repaint();
    }

    private void ResetModule()
    {
        modulePath = "Assets/Resources/Prefabs/Modules_Building_Yoka";
        marchingCube.modulePath = modulePath;
    }

    private void ReadData()
    {
        modulePath = marchingCube.modulePath;
        rows = marchingCube.rows;
        layers = marchingCube.layers;
        columns = marchingCube.columns;
        spacing = marchingCube.spacing;
    }

    public void FindOrCreateMarchingCube()
    {
        GameObject mcGameObject = GameObject.Find("MarchingCube");

        if (mcGameObject != null)
        {
            marchingCube = mcGameObject.GetComponent<MarchingCube>();
            if (marchingCube == null)
            {
                marchingCube = mcGameObject.AddComponent<MarchingCube>();
            }
        }
        else
        {
            GameObject newMC = new GameObject("MarchingCube");
            marchingCube = newMC.AddComponent<MarchingCube>();
            newMC.transform.position = Vector3.zero;
        }
    }

    public void FindOrCreateBuildingSystemBase()
    {
        GameObject bsbGameObject = GameObject.Find("BuildingSystemBase");

        if (bsbGameObject != null)
        {
            buildingSystemBase = bsbGameObject.GetComponent<BuildingSystemBase>();
            if (buildingSystemBase == null)
            {
                buildingSystemBase = bsbGameObject.AddComponent<BuildingSystemBase>();
            }
        }
        else
        {
            GameObject newBSB = new GameObject("BuildingSystemBase");
            buildingSystemBase = newBSB.AddComponent<BuildingSystemBase>();
            newBSB.transform.position = Vector3.zero;
        }
    }
    #endregion

    private void CreateModule()
    {
        buildingSystemBase.CreateModule(marchingCube);
        UpdateHint();
        buildingSystemBase.UpdateAllHintMesh("HintRoot", false);
    }

    private void StartEditing()
    {
        isEditing = true;

        Repaint();

        foreach (SceneView sceneView in SceneView.sceneViews)
        {
            sceneView.Repaint();
        }

        //表现层,展示所有可建造的地块
        UpdateHint();
        buildingSystemBase.UpdateAllHintMesh("HintRoot", false);
    }

    public void UpdateHint()
    {
        buildingSystemBase.CalculateHint(marchingCube);
        marchingCube.UpdateHint(marchingCube.marchingCubeDatas[0]);
    }

    private void StopEditing()
    {
        isEditing = false;

        Repaint();

        foreach (SceneView sceneView in SceneView.sceneViews)
        {
            sceneView.Repaint();
        }

        //buildingSystemBase.ResetHint(marchingCube);
        //marchingCube.UpdateHint(marchingCube.marchingCubeDatas[0]);
        marchingCube.ClearAllHintInstances();
    }
}