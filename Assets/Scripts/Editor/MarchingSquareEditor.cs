using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MarchingSquare))]
public class MarchingSquareEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        MarchingSquare s = (MarchingSquare)target;

        if (GUILayout.Button("Init"))
        {
            s.Init();
        }

        if (GUILayout.Button("UpdateModules"))
        {
            s.UpdateChangedModules();
        }

        if (GUILayout.Button("Clear"))
        {
            s.Clear();
        }
    }
}
