using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MeshGenerator))]
public class MeshGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        MeshGenerator generator = (MeshGenerator)target;

        if (GUILayout.Button("Generate Mesh"))
        {
            generator.GenarateMesh();
        }

        if (GUILayout.Button("Clear"))
        {
            generator.Init();
        }
    }
}