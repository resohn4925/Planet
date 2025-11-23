using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ModuleCalculate
{
    private List<List<string>> nameLists = new();
    private Dictionary<string, (string baseModule, int rotation, bool mirrored)> moduleMapping = new Dictionary<string, (string, int, bool)>();

    [ContextMenu("计算模块映射")]
    public void ModuleCalcu()
    {
        moduleMapping.Clear();
        nameLists = new List<List<string>>();
        HashSet<string> allNames = new HashSet<string>();

        for (int i = 0; i < 256; i++)
        {
            string moduleName = System.Convert.ToString(i, 2).PadLeft(8, '0');

            if (allNames.Contains(moduleName))
                continue;

            List<string> transformations = GetAllTransformations(moduleName);
            string baseModule = transformations.OrderBy(x => x).First();

            // 为每个变换计算映射
            foreach (string transformedModule in transformations)
            {
                (int rotation, bool mirrored) = GetTransformParameters(baseModule, transformedModule);
                moduleMapping[transformedModule] = (baseModule, rotation, mirrored);
            }

            allNames.UnionWith(transformations);
            nameLists.Add(transformations);
        }

        Debug.Log($"预计算完成！共有 {nameLists.Count} 个基础模块");
    }

    public void OutPutMapping()
    {
        //var result = GetModuleMapping(testModuleName);
        //Debug.Log($"基础模块: {result.baseModule}, 旋转: {result.rotation}°, 镜像: {result.mirrored}");
    }

    /// <summary>
    /// 获取模块映射信息（使用预计算结果）
    /// </summary>
    public (string baseModule, int rotation, bool mirrored) GetModuleMapping(string moduleName)
    {
        if (moduleMapping.TryGetValue(moduleName, out var mapping))
        {
            return mapping;
        }
        else
        {
            Debug.LogError($"模块模块 {moduleName} 不在映射表中，请先运行ModuleCalcu");
            return (null, 0, false);
        }
    }

    /// <summary>
    /// 获取从基础模块到目标模块的变换参数
    /// </summary>
    private (int rotation, bool mirrored) GetTransformParameters(string baseModule, string targetModule)
    {
        if (baseModule == targetModule)
            return (0, false);

        // 检查旋转
        string rotated = baseModule;
        for (int rotation = 90; rotation <= 270; rotation += 90)
        {
            rotated = Rotate90(rotated);
            if (rotated == targetModule)
                return (rotation, false);
        }

        // 检查镜像+旋转
        string mirrored = MirrorX(baseModule);
        if (mirrored == targetModule)
            return (0, true);

        rotated = mirrored;
        for (int rotation = 90; rotation <= 270; rotation += 90)
        {
            rotated = Rotate90(rotated);
            if (rotated == targetModule)
                return (rotation, true);
        }

        Debug.LogError($"无法确定从 {baseModule} 到 {targetModule} 的变换");
        return (0, false);
    }

    /// <summary>
    /// 返回模块编码经过y轴旋转和x轴镜像合计8种变换后的名字列表
    /// </summary>
    private List<string> GetAllTransformations(string binaryString)
    {
        HashSet<string> transformations = new HashSet<string>();

        // 4种旋转
        transformations.Add(binaryString);
        transformations.Add(Rotate90(binaryString));
        transformations.Add(Rotate90(Rotate90(binaryString)));
        transformations.Add(Rotate90(Rotate90(Rotate90(binaryString))));

        // 4种镜像+旋转
        string mirrored = MirrorX(binaryString);
        transformations.Add(mirrored);
        transformations.Add(Rotate90(mirrored));
        transformations.Add(Rotate90(Rotate90(mirrored)));
        transformations.Add(Rotate90(Rotate90(Rotate90(mirrored))));

        return new List<string>(transformations);
    }

    private string Rotate90(string binaryString)
    {
        string layer1 = binaryString.Substring(0, 4);
        string layer2 = binaryString.Substring(4, 4);
        return Rotate4Bits(layer1) + Rotate4Bits(layer2);
    }

    private string Rotate4Bits(string fourBits)
    {
        return $"{fourBits[3]}{fourBits[0]}{fourBits[1]}{fourBits[2]}";
    }

    private string MirrorX(string binaryString)
    {
        string layer1 = binaryString.Substring(0, 4);
        string layer2 = binaryString.Substring(4, 4);
        return Mirror4Bits(layer1) + Mirror4Bits(layer2);
    }

    private string Mirror4Bits(string fourBits)
    {
        return $"{fourBits[1]}{fourBits[0]}{fourBits[3]}{fourBits[2]}";
    }

    public void Output()
    {
        if (nameLists != null)
        {
            foreach (List<string> transformationGroup in nameLists)
            {
                if (transformationGroup.Count > 0)
                {
                    string groupString = string.Join(" ", transformationGroup);
                    string originalName = transformationGroup[0];
                    Debug.Log($"{groupString} from {originalName}");
                }
            }
            Debug.Log($"Total groups: {nameLists.Count}");
        }
    }
}