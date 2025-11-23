using UnityEditor;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
using static UnityEditor.PlayerSettings;

public class BuildingSystem : EditorWindow
{
    //[SerializeField]
    //private Grid grid;

    [SerializeField]
    private GameObject hintObj;

    [SerializeField]
    private GameObject mesh;

    [SerializeField]
    private int currenLayers;

    private GameObject currentHint;

    private GameObject currentMesh;

    [SerializeField]
    private MarchingCube marchingCube;

    [SerializeField]
    private bool isEditing = false;

    private Vector3Int lastGridPos = new Vector3Int(-1, -1, -1);

    /// <summary>
    /// 初始化
    /// </summary>
    //static BuildingSystem()
    //{
    //    EditorApplication.delayCall += () => {
    //        var windows = Resources.FindObjectsOfTypeAll<BuildingSystem>();
    //        foreach (var window in windows)
    //        {
    //            window.Close();
    //        }
    //    };
    //}

    [MenuItem("Tools/Building System")]
    public static void OpenWindow()
    {
        var window = GetWindow<BuildingSystem>("Building System");
        window.minSize = new Vector2(300, 200);
        window.Show();
    }

    private void OnGUI()
    {
        //grid = (Grid)EditorGUILayout.ObjectField("Grid", grid, typeof(Grid), true);

        hintObj = (GameObject)EditorGUILayout.ObjectField("HintObj", hintObj, typeof(GameObject), false);

        mesh = (GameObject)EditorGUILayout.ObjectField("Mesh", mesh, typeof(GameObject), false);

        marchingCube = (MarchingCube)EditorGUILayout.ObjectField("MarchingCube", marchingCube, typeof(MarchingCube), true);

        currenLayers = EditorGUILayout.IntField("Layer", currenLayers);

        if (!isEditing)
        {
            if (GUILayout.Button("开始编辑", GUILayout.Height(30)))
            {
                StartEditing();
            }
        }
        else
        {
            if (GUILayout.Button("结束编辑", GUILayout.Height(30)))
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

        Event e = Event.current;
        Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 worldPosition = hit.point;

            //Debug.Log(worldPosition);

            ShowHint(worldPosition);

            if (e.type == EventType.MouseDown && e.button == 0 && e.modifiers == EventModifiers.None)
            {
                Create(worldPosition);
                e.Use();
            }

            if (e.type == EventType.MouseDown && e.button == 1 && e.modifiers == EventModifiers.None)
            {
                Destroy(worldPosition);
                e.Use();
            }
        }
    }

    private void ShowHint(Vector3 pos)
    {
        int gridX = Mathf.RoundToInt((pos.x - 1f) / 2f) * 2;
        int gridZ = Mathf.RoundToInt((pos.z - 1f) / 2f) * 2;

        Vector3Int currentGridPos = new Vector3Int(gridX, (currenLayers - 1) * 2, gridZ);

        if (currentGridPos == lastGridPos && currentHint != null)
            return;

        lastGridPos = currentGridPos;

        Vector3 posCenter = new Vector3(gridX + 1f, (currenLayers - 1) * 2f + 1f, gridZ + 1f);


        if (hintObj != null)
        {
            if (currentHint != null)
            {
                DestroyImmediate(currentHint);
            }

            currentHint = (GameObject)PrefabUtility.InstantiatePrefab(hintObj);
            currentHint.transform.position = posCenter;
            currentHint.transform.rotation = Quaternion.identity;
            currentHint.transform.localScale = new Vector3(2f, 2f, 2f);
        }

        //Debug.Log(posCenter);
    }

    private void Create(Vector3 pos)
    {
        Vector3 posCenter = new Vector3();

        int gridX = Mathf.FloorToInt(pos.x / 2) * 2;
        int gridZ = Mathf.FloorToInt(pos.z / 2) * 2;

        posCenter = new Vector3(gridX + 1f, (currenLayers - 1) * 2f + 1f, gridZ + 1f);

        //Debug.Log("exe create");

        foreach (MarchingCube.MarchingCubeData.ObjPointData pointData in marchingCube.marchingCubeData.objPointDatas)
        {
            if (pointData.pos == posCenter)
            {
                pointData.isActive = true;
            }
        }

        //Debug.Log(posCenter);

        marchingCube.UpdateChangedModules();
    }

    private void Destroy(Vector3 pos)
    {
        Vector3 posCenter = new Vector3();

        int gridX = Mathf.FloorToInt(pos.x / 2) * 2;
        int gridZ = Mathf.FloorToInt(pos.z / 2) * 2;

        posCenter = new Vector3(gridX + 1f, (currenLayers - 1) * 2f + 1f, gridZ + 1f);

        //Debug.Log("exe create");

        foreach (MarchingCube.MarchingCubeData.ObjPointData pointData in marchingCube.marchingCubeData.objPointDatas)
        {
            if (pointData.pos == posCenter)
            {
                pointData.isActive = false;
            }
        }

        marchingCube.UpdateChangedModules();
    }

    private void StartEditing()
    {
        isEditing = true;

        Repaint();

        foreach (SceneView sceneView in SceneView.sceneViews)
        {
            sceneView.Repaint();
        }

        Init();
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
            currentHint = null;
        }

        Repaint();

        foreach (SceneView sceneView in SceneView.sceneViews)
        {
            sceneView.Repaint();
        }
    }

    private void Init()
    {
        Debug.Log("网格初始化");

        int x = marchingCube.rows;
        int y = marchingCube.columns;

        currentMesh = (GameObject)PrefabUtility.InstantiatePrefab(mesh);

        Vector3 pos = new Vector3(x, (currenLayers - 1) * 2f, y);
        currentMesh.transform.position = pos;
        currentMesh.transform.rotation = Quaternion.identity;
        currentMesh.transform.localScale = new Vector3(x / 5f, 1f, y / 5f);
    }
}