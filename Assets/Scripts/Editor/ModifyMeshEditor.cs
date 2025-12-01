using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ModifyMesh))]
public class ModifyMeshEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ModifyMesh generator = (ModifyMesh)target;

        if (GUILayout.Button("GeneratePoint"))
        {
            generator.GeneratePoint();
        }

        if (GUILayout.Button("Modify Mesh"))
        {
            generator.ApplyModifyMesh();
        }
    }
}
