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

    //临时变量
    private Vector3 lastGridPos = new Vector3Int(-1, -1, -1);
    private const float GRID_THRESHOLD = 0.05f;
    private GameObject currentHint;
    private GameObject currentMesh;

    private TerrainDataSO currentTerrainData;

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
        EditorGUILayout.LabelField("编辑地形", EditorStyles.boldLabel);
        currentLayers = EditorGUILayout.IntField("当前层数", currentLayers);
        if (!isEditing)
        {
            if (GUILayout.Button("编辑地形", GUILayout.Height(30)))
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

        if (GUILayout.Button("网格变形", GUILayout.Height(30)))
        {
            marchingCube.ApplyModifyMesh();
        }

        EditorGUILayout.Space();
        EditorGUILayout.HelpBox("Left Click: Place Object\nRight Click: Exit Placement Mode", MessageType.Info);
    }

    private void OnEnable()
    {
        SceneView.duringSceneGui += OnSceneGUI;

        if (marchingCube == null)
        {
            TryFindMarchingCube();
        }

        if (marchingCube != null && marchingCube.marchingCubeData == null)
        {
            DefaultSetting();//此处本应读取marchingcube中的数据，先用默认设置代替
            rootObj = GameObject.Find("Root");
            if (rootObj != null)
            {
                DestroyImmediate(rootObj);
                rootObj = new GameObject("Root");
                rootObj.transform.position = Vector3.zero;
            }

            SetData();//初始化
            marchingCube.Init();
        }
    }

    private bool TryFindMarchingCube()
    {
        marchingCube = FindObjectOfType<MarchingCube>();
        if (marchingCube != null)
        {
            return true;
        }

        return false;
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

        RaycastHit[] hits = Physics.RaycastAll(ray);
        RaycastHit? meshHit = null;
        foreach (var h in hits)
        {
            if (h.collider.gameObject.name == "Mesh" ||
                h.collider.gameObject.name.StartsWith("Mesh"))
            {
                meshHit = h;
                break;
            }
        }

        if (meshHit.HasValue)
        {
            Vector3 worldPosition = meshHit.Value.point;
            ShowHint(worldPosition);

            if (e.type == EventType.MouseDown && e.button == 0 && e.modifiers == EventModifiers.None)
            {
                Selection.activeObject = null;
                CreateModule(worldPosition);
                e.Use();
                GUIUtility.hotControl = 0;
            }

            if (e.type == EventType.MouseDown && e.button == 1 && e.modifiers == EventModifiers.None)
            {
                Selection.activeObject = null;
                DestroyModule(worldPosition);
                e.Use();
                GUIUtility.hotControl = 0;
            }
        }

        if (Selection.activeObject != null &&
            Selection.activeObject != this &&
            Selection.activeObject != marchingCube)
        {
            EditorApplication.delayCall += () => {
                Selection.activeObject = null;
            };
        }
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

    private void ReadData()
    {
        modulePath = marchingCube.modulePath;
        rows = marchingCube.rows;
        layers = marchingCube.layers;
        columns = marchingCube.columns;
        spacing = marchingCube.spacing;
    }

    private void ResetModule()
    {
        modulePath = "Assets/Resources/Prefabs/Modules3d_Terrain";
        marchingCube.modulePath = modulePath;
    }

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

        ReadData();

        currentLayers = 1;
        marchingCube.moduleCollection = rootObj;

        Repaint();
    }

    private void FindOrCreateMarchingCube()
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

    private void Init()
    {
        SetData();
        marchingCube.Init();
    }

    private void ShowHint(Vector3 pos)
    {
        if (spacing == 0)
            return;
        float posX = ((int)(pos.x / spacing) + 0.5f) * spacing;
        float posZ = ((int)(pos.z / spacing) + 0.5f) * spacing;

        Vector3 currentGridPos = new Vector3(posX, (currentLayers - 1/2f) * spacing, posZ);

        if (currentGridPos == lastGridPos && currentHint != null)
            return;

        lastGridPos = currentGridPos;

        if (hintObj != null)
        {
            if (currentHint == null)
            {
                if (editMode == EditMode.Slope)
                {
                    currentHint = (GameObject)PrefabUtility.InstantiatePrefab(hintObjSlope);
                }
                else if(editMode == EditMode.Terrain)
                {
                    currentHint = (GameObject)PrefabUtility.InstantiatePrefab(hintObj);
                }
                else if(editMode == EditMode.Cliff)
                {
                    currentHint = (GameObject)PrefabUtility.InstantiatePrefab(hintObjSlope);
                }
                currentHint.hideFlags = HideFlags.DontSave;
                float rotation = 0f;
                if (selectedOption == "XNegative" || selectedOption == "XPositive")
                {
                    rotation = 90f;
                }
                currentHint.transform.rotation = Quaternion.Euler(0, rotation, 0);
                Vector3 prefabScale = currentHint.transform.localScale;
                currentHint.transform.localScale = new Vector3(
                    prefabScale.x * spacing,
                    prefabScale.y * spacing,
                    prefabScale.z * spacing
                 );
            }

            currentHint.transform.position = currentGridPos;
        }
    }

    /// <summary>
    /// create terrain or slope
    /// </summary>
    /// <param name="pos"></param>
    private void CreateModule(Vector3 pos)
    {
        int xIndex = (int)(pos.x / spacing);
        int zIndex = (int)(pos.z / spacing);
        int yIndex = currentLayers - 1;

        bool canCreate = false;

        //判断生成类型
        switch (editMode)
        {
            case EditMode.Terrain:
                if (currentLayers == 1)
                {
                    canCreate = true;
                }
                else
                {
                    int lowerYIndex = yIndex - 1;
                    if (marchingCube.marchingCubeData.objPointArray != null &&
                        xIndex >= 0 && xIndex < marchingCube.rows &&
                        zIndex >= 0 && zIndex < marchingCube.columns &&
                        lowerYIndex >= 0 && lowerYIndex < marchingCube.layers)
                    {
                        canCreate = marchingCube.marchingCubeData.objPointArray[xIndex, zIndex, lowerYIndex].isActive;
                    }
                }

                if (canCreate)
                {
                    if (marchingCube.marchingCubeData == null)
                    {
                        Debug.LogError("请先初始化!");
                        return;
                    }
                    if (marchingCube.marchingCubeData.objPointArray != null &&
                        xIndex >= 0 && xIndex < marchingCube.rows &&
                        zIndex >= 0 && zIndex < marchingCube.columns &&
                        yIndex >= 0 && yIndex < marchingCube.layers)
                    {
                        marchingCube.marchingCubeData.objPointArray[xIndex, zIndex, yIndex].isActive = true;
                        marchingCube.UpdateModules();

                        EditorApplication.delayCall += () =>
                        {
                            Selection.activeObject = null;
                        };
                    }
                }
                return;

            case EditMode.Slope:
                Debug.Log(currentHint.transform.localScale);
                int xRadius = (int)(currentHint.transform.localScale.z / spacing) - 1;
                int zRadius = (int)(currentHint.transform.localScale.x / spacing) - 1;
                if (selectedOption == "ZPositive" || selectedOption == "ZNegative")
                {
                    zRadius = (int)(currentHint.transform.localScale.z / spacing) - 1;
                    xRadius = (int)(currentHint.transform.localScale.x / spacing) - 1;
                }
                Debug.Log($"判断周围{xRadius}*{zRadius}内是否有斜坡");

                canCreate = !HasNearByPoint(xIndex, zIndex, xRadius, zRadius);

                if (canCreate)
                {
                    //根据斜坡方向设置旋转角度
                    switch (selectedOption)
                    {
                        case "ZNegative":
                            marchingCube.UpdateChangedObj(xIndex, zIndex, yIndex, true, 0, MarchingCube.EditMode.Slope);
                            return;
                        case "XNegative":
                            marchingCube.UpdateChangedObj(xIndex, zIndex, yIndex, true, 90, MarchingCube.EditMode.Slope);
                            return;
                        case "ZPositive":
                            marchingCube.UpdateChangedObj(xIndex, zIndex, yIndex, true, 180, MarchingCube.EditMode.Slope);
                            return;
                        case "XPositive":
                            marchingCube.UpdateChangedObj(xIndex, zIndex, yIndex, true, 270, MarchingCube.EditMode.Slope);
                            return;
                    }
                }
                return;

            case EditMode.Cliff:
                xRadius = (int)(currentHint.transform.localScale.z / spacing) - 1;
                zRadius = (int)(currentHint.transform.localScale.x / spacing) - 1;
                if (selectedOption == "ZPositive" || selectedOption == "ZNegative")
                {
                    zRadius = (int)(currentHint.transform.localScale.z / spacing) - 1;
                    xRadius = (int)(currentHint.transform.localScale.x / spacing) - 1;
                }
                Debug.Log($"判断周围{xRadius}*{zRadius}内是否有悬崖");

                canCreate = !HasNearByPoint(xIndex, zIndex, xRadius, zRadius);

                if (canCreate)
                {
                    //根据悬崖方向设置旋转角度
                    switch (selectedOption)
                    {
                        case "ZNegative":
                            marchingCube.UpdateChangedObj(xIndex, zIndex, yIndex, true, 0, MarchingCube.EditMode.Cliff);
                            return;
                        case "XNegative":
                            marchingCube.UpdateChangedObj(xIndex, zIndex, yIndex, true, 90, MarchingCube.EditMode.Cliff);
                            return;
                        case "ZPositive":
                            marchingCube.UpdateChangedObj(xIndex, zIndex, yIndex, true, 180, MarchingCube.EditMode.Cliff);
                            return;
                        case "XPositive":
                            marchingCube.UpdateChangedObj(xIndex, zIndex, yIndex, true, 270, MarchingCube.EditMode.Cliff);
                            return;
                    }
                }
                return;
        }
    }
    
    /// <summary>
    /// destroy module
    /// </summary>
    /// <param name="pos"></param>
    private void DestroyModule(Vector3 pos)
    {
        int xIndex = (int)(pos.x / spacing);
        int zIndex = (int)(pos.z / spacing);
        int yIndex = currentLayers - 1;

        switch (editMode)
        {
            case EditMode.Terrain:
                if (marchingCube.marchingCubeData.objPointArray != null &&
xIndex >= 0 && xIndex < marchingCube.rows &&
zIndex >= 0 && zIndex < marchingCube.columns &&
yIndex >= 0 && yIndex < marchingCube.layers)
                {
                    // 检查上方是否有激活的模块
                    bool hasModuleAbove = false;
                    int upperYIndex = yIndex + 1;

                    if (upperYIndex < marchingCube.layers)
                    {
                        hasModuleAbove = marchingCube.marchingCubeData.objPointArray[xIndex, zIndex, upperYIndex].isActive;
                    }

                    if (hasModuleAbove)
                    {
                        return;
                    }

                    // 如果没有上方模块可以销毁当前模块
                    marchingCube.marchingCubeData.objPointArray[xIndex, zIndex, yIndex].isActive = false;
                    marchingCube.UpdateModules();

                    EditorApplication.delayCall += () =>
                    {
                        Selection.activeObject = null;
                    };
                }
                return;

            case EditMode.Slope:
                //判断斜坡HintObj所在范围内是否有斜坡，如果有，销毁
                int xRadius = (int)(currentHint.transform.localScale.z / spacing) - 1;
                int zRadius = (int)(currentHint.transform.localScale.x / spacing) - 1;

                for (int y = 0; y < layers; y++)
                {
                    for (int x = xIndex - xRadius; x < xIndex + xRadius; x++)
                    {
                        for (int z = zIndex - zRadius; z < zIndex + zRadius; z++)
                        {
                            if (x >= 0 && y >= 0 && z >= 0)
                            {
                                if (marchingCube.marchingCubeData.objPointArray[x, z, y].isSlope == true)
                                {
                                    marchingCube.UpdateChangedObj(x, z, y, false, 0, MarchingCube.EditMode.Slope);
                                    return;
                                }
                            }
                        }
                    }
                }
                return;

            case EditMode.Cliff:
                xRadius = (int)(currentHint.transform.localScale.z / spacing) - 1;
                zRadius = (int)(currentHint.transform.localScale.x / spacing) - 1;

                for (int y = 0; y < layers; y++)
                {
                    for (int x = xIndex - xRadius; x < xIndex + xRadius; x++)
                    {
                        for (int z = zIndex - zRadius; z < zIndex + zRadius; z++)
                        {
                            if (x >= 0 && y >= 0 && z >= 0)
                            {
                                if (marchingCube.marchingCubeData.objPointArray[x, z, y].isCliff == true)
                                {
                                    marchingCube.UpdateChangedObj(x, z, y, false, 0, MarchingCube.EditMode.Cliff);
                                    return;
                                }
                            }
                        }
                    }
                }
                return;
        }
    }

    /// <summary>
    /// 判断立方点阵范围内是否有激活点
    /// </summary>
    /// <returns></returns>
    private bool HasNearByPoint(int xIndex, int zIndex, int xRadius, int zRadius)
    {
        bool hasNearby = false;

        for (int dx = -xRadius; dx <= xRadius; dx++)
        {
            for (int dz = -zRadius; dz <= zRadius; dz++)
            {
                if (hasNearby) break;

                int checkX = xIndex + dx;
                int checkZ = zIndex + dz;

                if (checkX >= 0 && checkX < marchingCube.rows &&
                    checkZ >= 0 && checkZ < marchingCube.columns)
                {
                    for (int checkY = 0; checkY < marchingCube.layers; checkY++)
                    {
                        switch (editMode)
                        {
                            case EditMode.Slope:
                                if (marchingCube.marchingCubeData.objPointArray[checkX, checkZ, checkY].isSlope)
                                {
                                    hasNearby = true;
                                }
                                break;
                            case EditMode.Cliff:
                                if (marchingCube.marchingCubeData.objPointArray[checkX, checkZ, checkY].isCliff)
                                {
                                    hasNearby = true;
                                }
                                break;
                        }

                        if (hasNearby) break;
                    }
                }

                if (hasNearby) break;
            }

            if (hasNearby) break;
        }

        return hasNearby;
    }

    private void StartEditing()
    {
        isEditing = true;

        Repaint();

        foreach (SceneView sceneView in SceneView.sceneViews)
        {
            sceneView.Repaint();
        }

        InitMesh();
    }

    private void StopEditing()
    {
        isEditing = false;

        if (currentHint != null)
        {
            DestroyImmediate(currentHint);
            currentHint = null;
        }

        if (currentMesh != null)
        {
            DestroyImmediate(currentMesh);
            currentMesh = null;
        }

        Repaint();

        foreach (SceneView sceneView in SceneView.sceneViews)
        {
            sceneView.Repaint();
        }
    }

    private void InitMesh()
    {
        Debug.Log("网格初始化");

        int x = marchingCube.rows;
        int y = marchingCube.columns;

        currentMesh = (GameObject)PrefabUtility.InstantiatePrefab(mesh);

        Vector3 pos = new Vector3(x * spacing / 2, (currentLayers - 1) * spacing, y * spacing / 2);
        currentMesh.transform.position = pos;
        currentMesh.transform.rotation = Quaternion.identity;
        currentMesh.transform.localScale = new Vector3(x * spacing / 10f, 1f, y * spacing / 10f);

        currentMesh.hideFlags = HideFlags.DontSave;
    }
}