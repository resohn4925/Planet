using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXGenerator : MonoBehaviour
{
    public GameObject vfxPrefab;
    private Dictionary<string, GameObject> vfxInstances = new Dictionary<string, GameObject>();

    public void Init()
    {
        //销毁所有vfxInstances
    }

    public void GenerateVFXWithIndex(Vector3 position, Vector3 direction, int faceIndex, int x, int y, int z)
    {
        string key = GenerateKey(faceIndex, x, y, z);
        
        if (vfxPrefab != null)
        {
            GameObject vfxInstance = Instantiate(vfxPrefab, position, Quaternion.LookRotation(direction));
            vfxInstances[key] = vfxInstance;
        }
    }

    public void DestroyVFXByIndex(int faceIndex, int x, int y, int z)
    {
        string key = GenerateKey(faceIndex, x, y, z);

        if (vfxInstances.TryGetValue(key, out GameObject vfxInstance))
        {
            if (vfxInstance != null)
            {
                Destroy(vfxInstance);
                vfxInstances.Remove(key);
            }
            else
            {
                vfxInstances.Remove(key);
            }
        }
    }

    private string GenerateKey(int faceIndex, int x, int y, int z)
    {
        return $"{faceIndex}_{x}_{y}_{z}";
    }
}