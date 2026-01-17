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

        if (GUILayout.Button("初始化"))
        {
            planetPipeline.Init();
        }

        if (GUILayout.Button("加载物件"))
        {
            planetPipeline.Load();
        }

        if (GUILayout.Button("存储物件"))
        {
            planetPipeline.Save();
        }

        //if (GUILayout.Button("激活物件"))
        //{
        //    planetPipeline.ActivateObj();
        //}

        //if (GUILayout.Button("生成物件"))
        //{
        //    planetPipeline.GenerateObj();
        //}

         bool isEditing = planetPipeline.isEditing;
        string buttonText = isEditing ? "结束编辑" : "开始编辑";

        if (planetPipeline.buildingSystemOnSphere != null)
        {
            string modeText = planetPipeline.buildingSystemOnSphere.currentBuildingMode == BuildingMode.Build
                ? "切换到销毁模式"
                : "切换到建造模式";
            if (GUILayout.Button(modeText))
            {
                var newMode = planetPipeline.buildingSystemOnSphere.currentBuildingMode == BuildingMode.Build
                    ? BuildingMode.Destroy
                    : BuildingMode.Build;
                planetPipeline.buildingSystemOnSphere.SwitchBuildingMode(newMode);
                Debug.Log($"已切换到{(newMode == BuildingMode.Build ? "建造" : "销毁")}模式");
            }
        }

        if (GUILayout.Button(buttonText))
        {
            if (!isEditing)
            {
                Debug.Log("开始编辑");
            }
            else
            {
                Debug.Log("结束编辑");
            }

            isEditing = !isEditing;
            planetPipeline.SwitchEditMode(isEditing);
        }

        if (GUILayout.Button("几何信息显/隐"))
        {
            planetPipeline.SwitchGeometry();
        }

        if (GUILayout.Button("Debug"))
        {
            planetPipeline.ShowDebug();
        }
    }
}
