using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ModifyMesh3d))]
public class ModifyMesh3dEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ModifyMesh3d generator = (ModifyMesh3d)target;

        if (GUILayout.Button("GeneratePoint"))
        {
            generator.GeneratePoint();
        }

        //if (GUILayout.Button("Modify Mesh"))
        //{
        //    generator.ApplyModifyMesh();
        //}
    }
}
