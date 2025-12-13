using Planet;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlanetPipeline))]
public class PlanetPipelineEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        PlanetPipeline s = (PlanetPipeline)target;

        if (GUILayout.Button("Init"))
        {
            s.Init();
        }

        if (GUILayout.Button("GenerateMesh"))
        {
            s.GenerateMesh();
        }
    }
}
