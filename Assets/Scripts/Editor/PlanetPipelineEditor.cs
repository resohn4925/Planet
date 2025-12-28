using Planet;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlanetPipeline))]
public class PlanetPipelineEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        PlanetPipeline planetPipeline = (PlanetPipeline)target;

        if (GUILayout.Button("Init"))
        {
            planetPipeline.Init();
        }

        if (GUILayout.Button("Load"))
        {
            planetPipeline.Load();
        }

        if (GUILayout.Button("Save"))
        {
            planetPipeline.Save();
        }

        if (GUILayout.Button("ActivateObj"))
        {
            planetPipeline.ActivateObj();
        }

        if (GUILayout.Button("GenerateObj"))
        {
            planetPipeline.GenerateObj();
        }

        if (GUILayout.Button("ShowDebug"))
        {
            planetPipeline.ShowDebug();
        }

        if (GUILayout.Button("ShowGeometry"))
        {
            planetPipeline.ShowGeometry();
        }
    }
}
