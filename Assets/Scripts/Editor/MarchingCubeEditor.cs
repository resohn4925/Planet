using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MarchingCube))]
public class MarchingCubeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        MarchingCube s = (MarchingCube)target;

        if (GUILayout.Button("Init"))
        {
            s.Init();
        }

        if (GUILayout.Button("LoadPrefab"))
        {
            s.LoadPrefab();
        }

        if (GUILayout.Button("UpdateModules"))
        {
            s.UpdateChangedModules();
        }

        if (GUILayout.Button("Clear"))
        {
            s.Clear();
        }

        if (GUILayout.Button("OutPut"))
        {
            s.OutPut();
        }
    }
}
